using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core;
using BlockArray.Core.GoogleMapsApi;
using BlockArray.Core.Mapping;
using BlockArray.ServiceModel;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI;
using FreightTrust.EDI.Wrappers;
using FreightTrust.Handlers.Shipments;
using FreightTrust.Interface.Shipments;
using FreightTrust.Modules.Equipment;
using FreightTrust.Modules.OrderItem;
using FreightTrust.Modules.Shipment;
using FreightTrust.Modules.Stop;
using FreightTrust.Modules.StopEquipmentItem;
using FreightTrust.Transactions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FreightTrust.Modules.EdiTender
{
    public class EdiLoadTenderHandler : ShipmentEdiHandler<TS204>
    {
        private readonly ITenancyContextProvider _tenancyContextProvider;
        private readonly IMapperService _mapperService;
        private readonly ILocationCalculator _locationCalculator;


        public EdiLoadTenderHandler(ICompanyRepositoryProvider db, IMediator mediator,
            ITenancyContextProvider tenancyContextProvider, IMapperService mapperService,
            ILocationCalculator locationCalculator) : base(db, mediator)
        {
            _tenancyContextProvider = tenancyContextProvider;
            _mapperService = mapperService;
            _locationCalculator = locationCalculator;
        }

        protected override async Task HandleDocument(string companyId, TradingChannel.TradingChannel channel,
            TradingPartner.TradingPartner partner, TS204 doc, TradingPartnerMessage.TradingPartnerMessage message)
        {
            var shipments = Db.Get<Shipment.Shipment>(companyId);
            var stops = Db.Get<Stop.Stop>(companyId);  
            var transaction = AutoMapper.Mapper.Map<LoadTenderTransaction>(doc);

            if (transaction.Purpose == LoadTenderTransactionPurpose.Original ||
                transaction.Purpose == LoadTenderTransactionPurpose.OriginalNoResponse) // Original
            {
                var requiresResponse = transaction.Purpose == LoadTenderTransactionPurpose.OriginalNoResponse;

                transaction.Shipment.Status = ShipmentStatus.Tendered;
                transaction.Shipment.FromPartnerId = partner.Id;

                var result = await Mediator.Send(new SaveShipmentRequest()
                {
                    ServiceModel = transaction.Shipment
                });
                //await shipments.Save(transaction.Shipment);

                foreach (var stop in transaction.Shipment.Stops)
                {
                    stop.ShipmentId = result.Id;
                    var stopServiceModel = await Mediator.Send(new SaveStopRequest()
                    {
                        ServiceModel = stop,
                        SkipRefresh = true
                    });

                    foreach (var item in stop.EquipmentItems)
                    {
                        // Save the equipment information 
                        if (item.Equipment != null)
                        {
                            var equipmentServiceModel = await Mediator.Send(new SaveEquipmentRequest()
                            {
                                ServiceModel = item.Equipment
                            });

                            item.EquipmentId = equipmentServiceModel.Id;
                        }

                        item.ShipmentId = result.Id;
                        item.StopId = stopServiceModel.Id;

                        var orderItemServiceModel = await Mediator.Send(
                            new SaveStopEquipmentItemRequest()
                            {
                                ServiceModel = item
                            });
                    }

                    foreach (var item in stop.OrderItems)
                    {
                        item.ShipmentId = result.Id;
                        item.StopId = stopServiceModel.Id;

                        var orderItemServiceModel = await Mediator.Send(
                            new SaveOrderItemRequest()
                            {
                                ServiceModel = item
                            });
                    }
                }


                try
                {
                    var refreshStopsHandler =
                        new RefreshStopsHandler(shipments, stops, _mapperService, _locationCalculator);
                    await refreshStopsHandler.Handle(new RefreshStopsRequest()
                    {
                        ShipmentId = result.Id
                    }, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
//                var shipment = await shipments.FirstOrDefaultAsync(p =>
//                    p.ShipmentIdentificationNumber == data.ShipmentIdentificationNumber);
//                
//                await Mediator.Send(new CancelRequest()
//                {
//                    Id = shipment.Id
//                });
            }
        }

        private StopAction ConvertAction(string itemStopReasonCode)
        {
            switch (itemStopReasonCode)
            {
                case "CL":
                    return StopAction.CompleteLoad;
                case "CU":
                    return StopAction.CompleteUnload;
                case "LD":
                    return StopAction.Load;
                case "PL":
                    return StopAction.PartLoad;
                case "PU":
                    return StopAction.PartUnload;
                case "UL":
                    return StopAction.Unload;
                default:
                    throw new Exception($"Can't map stop reason code {itemStopReasonCode}");
            }
        }
    }
}