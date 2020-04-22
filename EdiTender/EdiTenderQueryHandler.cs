using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Services;
using BlockArray.Model.Mongo;
using BlockArray.ServiceModel;
using BlockArray.ServiceModel.PdfForms;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public partial class EdiTenderQueryRequest
    {

    }
    public class EdiTenderQueryHandler : EdiTenderQueryHandlerBase  {
        public EdiTenderQueryHandler(
                BaseRepository<EdiTender> repo,
                EdiTenderSearchEngine searchEngine,
                IMapperService mapper
                ) : base(repo, searchEngine, mapper)
        {

        }
    }

    public class EdiTenderToShipmentMapper : IMapper<EdiTender, Shipment.Shipment>
    {
        
        public Shipment.Shipment Map(EdiTender source, Shipment.Shipment target)
        {
            
            
            return target;
        }
    }
}
