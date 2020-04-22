using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using MongoDB.Bson.Serialization.Attributes;
using BlockArray.Harbor.Authorization;
using MongoDB.Driver;

namespace FreightTrust.Modules.EdiTender
{
    public partial class EdiTenderInitialData : IModelInitialData<EdiTender>
    {
        public IEnumerable<EdiTender> Get(IMongoDatabase db) {
            yield break;
        }
    }

}
