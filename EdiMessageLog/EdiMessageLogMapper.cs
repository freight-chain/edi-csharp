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
namespace FreightTrust.Modules.EdiMessageLog
{
    public class EdiMessageLogMapper : BaseEdiMessageLogMapper
    {
        public EdiMessageLogMapper(IServiceProvider serviceProvider) {
            ServiceProvider = serviceProvider;
        }

        public override void MapMore(EdiMessageLog source, EdiMessageLogServiceModel target)
        {
            // Additional Mappings ...
        }
    }
}
