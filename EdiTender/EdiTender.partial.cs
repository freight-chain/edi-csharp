using System.Linq;
using System.Collections.Generic;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using FreightTrust.Modules.SamsaraConfig;
using MongoDB.Bson.Serialization.Attributes;

namespace FreightTrust.Modules.EdiTender
{
    public partial class EdiTender
    {
        public List<EdiTenderStopServiceModel> Stops { get; set; } = new List<EdiTenderStopServiceModel>();
    }
}