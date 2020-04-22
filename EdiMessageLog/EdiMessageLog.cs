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
namespace FreightTrust.Modules.EdiMessageLog
{
    [BsonIgnoreExtraElements]
    public partial class EdiMessageLog : BaseMongoDocument
    {
        
        public System.String ReferenceId {get;set;}
        
        public BlockArray.ServiceModel.EdiMessageType Type {get;set;}
        
        public BlockArray.ServiceModel.EdiMessageStatus Status {get;set;}
        
        public System.String EdiTypeNumber {get;set;}
        
        public System.String MessageTypeName {get;set;}
        
        public string TradingChannelId {get;set;}
        
        
        public string TradingPartnerId {get;set;}
        
        
        public System.String Content {get;set;}
        
        public System.String ErrorMessage {get;set;}
        
    }

}
