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
namespace FreightTrust.Modules.EdiConfig
{
    [BsonIgnoreExtraElements]
    public partial class EdiConfig : BaseMongoDocument
    {
        
        public System.Boolean Enabled {get;set;}
        
        public System.String Username {get;set;}
        
        public System.String Password {get;set;}
        
        public System.String RsaKey {get;set;}
        
        public string CompanyId {get;set;}
        
        
    }

}
