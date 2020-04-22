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
namespace FreightTrust.Modules.EdiTender
{
    [BsonIgnoreExtraElements]
    public partial class EdiTender : BaseMongoDocument
    {
        
        public System.String ControlNumber {get;set;}
        
        public System.String Scac {get;set;}
        
        public System.String BillOfLading {get;set;}
        
        public System.String ShipmentOwnerId {get;set;}
        
        public System.String LoadTenderType {get;set;}
        
        public System.Int32 MethodOfPayment {get;set;}
        
        public System.DateTime TenderDateTime {get;set;}
        
        public System.Int32 TotalWeight {get;set;}
        
        public System.Int32 TotalQuantity {get;set;}
        
        public System.Int32 TotalCharges {get;set;}
        
        public System.String Notes {get;set;}
        
        public System.String EquipmentNumber {get;set;}
        
        public System.String ReferenceInfo {get;set;}
        
        public System.String EntityCode {get;set;}
        
        public System.String EntityName {get;set;}
        
        public System.String LocationCode {get;set;}
        
        public System.String EdiDocument {get;set;}
        
        public BlockArray.ServiceModel.EdiStatus Status {get;set;}
        
        public System.String ShipmentId {get;set;}
        
    }

}
