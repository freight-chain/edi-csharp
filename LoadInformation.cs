using System;
using System.Collections.Generic;
using System.Linq;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FreightTrust.EDI
{
    public class LoadInformation
    {
        private Loop_L5_204 _item;

        public LoadInformation(Loop_L5_204 loopL5204 = null)
        {
            Item = loopL5204 ?? new Loop_L5_204();
           
        }

        [JsonIgnore, BsonIgnore]
        public Loop_L5_204 Item
        {
            get => _item ?? (_item = new Loop_L5_204());
            set => _item = value;
        }

        protected L5 L5 => Item.L5 ?? (Item.L5 = new L5());
        protected AT8 AT8 => Item.AT8 ?? (Item.AT8 = new AT8());
        
        protected List<Loop_G61_204> G61 => Item.G61Loop ?? (Item.G61Loop = new List<Loop_G61_204>());
        
        public IEnumerable<LoadContactInfo> LoadContactInfo
        {
            get { return G61.Select(x => new LoadContactInfo(x)); }
            set
            {
                G61.Clear();
                G61.AddRange(value.Select(x=>x.Item));
            }
        }
        public WeightQualifier WeightQualifier
        {
            get => Enum.Parse<WeightQualifier>(AT8.WeightQualifier_01, true);
            set => AT8.WeightQualifier_01 = value.ToString();
        }          
        
        public WeightUnitCode WeightUnitCode
        {
            get => Enum.Parse<WeightUnitCode>(AT8.WeightUnitCode_02, true);
            set => AT8.WeightUnitCode_02 = value.ToString();
        }           
        
        
        public string Weight
        {
            get => AT8.Weight_03;
            set => AT8.Weight_03 = value;
        }                 
        
        /// <summary>
        /// Carton
        /// </summary>
        public string LadingQuantity
        {
            get => AT8.LadingQuantity_04;
            set => AT8.LadingQuantity_04 = value;
        }     
        
        /// <summary>
        /// pallet or
        /// slip sheet
        /// </summary>
        public string HandlingUnitQuantity
        {
            get => AT8.LadingQuantity_05;
            set => AT8.LadingQuantity_05 = value;
        }               
        public string VolumeUnitQualifier
        {
            get => AT8.VolumeUnitQualifier_06;
            set => AT8.VolumeUnitQualifier_06 = value;
        }             
        
        public string Volume
        {
            get => AT8.Volume_07;
            set => AT8.Volume_07 = value;
        }           
        
        public string LadingDescription
        {
            get => L5.LadingDescription_02;
            set => L5.LadingDescription_02 = value;
        }        
        /// <summary>
        /// Always NMFC number
        /// </summary>
        public string CommodityCode
        {
            get => L5.CommodityCode_03;
            set => L5.CommodityCode_03 = value;
        }      
        /// <summary>
        /// Always NMFC number
        /// </summary>
        public string CommodityCodeQualifier
        {
            get => L5.CommodityCodeQualifier_04;
            set => L5.CommodityCodeQualifier_04 = value;
        }    
        
        public string LadingLineItemNumber
        {
            get => L5.LadingLineItemNumber_01;
            set => L5.LadingLineItemNumber_01 = value;
        }
        
        public string PackagingCode
        {
            get => L5.PackagingCode_05;
            set => L5.PackagingCode_05 = value;
        }   
//        public string PackagingCode
//        {
//            get => L5.;
//            set => L5.PackagingCode_05 = value;
//        }
    }
}