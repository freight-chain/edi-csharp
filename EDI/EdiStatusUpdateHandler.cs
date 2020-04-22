using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.ServiceModel;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI.Wrappers;
using FreightTrust.Modules.Equipment;
using FreightTrust.Modules.Shipment;
using FreightTrust.Modules.StopUpdate;
using FreightTrust.Transactions;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public class EdiStatusUpdateHandler : ShipmentEdiHandler<TS214>
    {
        private readonly BaseRepository<Company.Company> _companyRepo;
        private readonly ITradingPartnerSender _sender;

        public EdiStatusUpdateHandler(ICompanyRepositoryProvider db, IMediator mediator, BaseRepository<Company.Company> companyRepo, ITradingPartnerSender sender) : base(db, mediator)
        {
            _companyRepo = companyRepo;
            _sender = sender;
        }

        protected override async Task HandleDocument(string companyId, TradingChannel.TradingChannel channel, TradingPartner.TradingPartner partner, TS214 ts214, TradingPartnerMessage.TradingPartnerMessage message)
        {
            var data = AutoMapper.Mapper.Map<LoadTenderStatusUpdateTransaction>(ts214);
            
            var shipments = Db.Get<Shipment.Shipment>(companyId);
            var shipment = await shipments.FirstOrDefaultAsync(p =>p.ShipmentIdentificationNumber == data.ShipmentIdentificationNumber);

            if (data.EquipmentItem != null)
            {
                await Mediator.Send(new SaveEquipmentRequest()
                {
                    ServiceModel = data.EquipmentItem
                });
            }
            
            if (shipment.CurrentStopId == null)
            {
                try
                {
                    await Mediator.Send(new BeginRequest()
                    {
                        Id = shipment.Id
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                await Mediator.Send(new SaveStopUpdateRequest()
                {
                    CompanyId = companyId,
                   
                    ServiceModel = new StopUpdateServiceModel()
                    {
                        StopId = shipment.CurrentStopId,
                        Lat = data.StopUpdate.Lat,
                        Lng = data.StopUpdate.Lng,
                        DriverId = data.StopUpdate.DriverId,
                        Timestamp = data.StopUpdate.Timestamp,
                        StatusCodeId = data.StopUpdate.StatusCodeId,
                        StatusUpdateTypeId = data.StopUpdate.StatusUpdateTypeId
                    }
                });
            }
            message.ReferenceId = shipment.ShipmentIdentificationNumber;
        }
    }
}