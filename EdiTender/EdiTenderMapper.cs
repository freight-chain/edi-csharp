using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using Microsoft.Extensions.DependencyInjection;
namespace FreightTrust.Modules.EdiTender
{
    public class EdiTenderMapper : BaseEdiTenderMapper
    {
        public EdiTenderMapper(IServiceProvider serviceProvider) {
            ServiceProvider = serviceProvider;
        }

        public override void MapMore(EdiTender source, EdiTenderServiceModel target)
        {
            target.Stops = source.Stops;
            // Additional Mappings ...
        }
    }
}
