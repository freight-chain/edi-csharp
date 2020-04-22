using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FreightTrust.EDI
{
    public class LocationInfo
    {
        private Loop_N1_204_2 _item;

        [JsonIgnore, BsonIgnore]
        public Loop_N1_204_2 Item
        {
            get => _item ?? (_item = new Loop_N1_204_2());
            set => _item = value;
        }

        public LocationInfo(Loop_N1_204_2 item = null)
        {
            Item = item ?? new Loop_N1_204_2();
        }

        protected N1 N1 => Item.N1 ?? (Item.N1 = new N1());
        protected List<N3> N3 => Item.N3 ?? (Item.N3 = new List<N3>());
        protected N4 N4 => Item.N4 ?? (Item.N4 = new N4());
        protected List<G61> G61 => Item.G61 ?? (Item.G61 = new List<G61>());
      

        public string Name
        {
            get => N1.Name_02;
            set => N1.Name_02 = value;
        }

        public string IdentificationCode
        {
            get => N1.IdentificationCode_04;
            set => N1.IdentificationCode_04 = value;
        }

        public string IdentificationCodeQualifier
        {
            get => N1.IdentificationCodeQualifier_03;
            set => N1.IdentificationCodeQualifier_03 = value;
        }

        public string EntityIdentifierCode
        {
            get => N1.EntityIdentifierCode_01;
            set => N1.EntityIdentifierCode_01 = value;
        }

        public string EntityRelationshipCode
        {
            get => N1.EntityRelationshipCode_05;
            set => N1.EntityRelationshipCode_05 = value;
        }

        public IEnumerable<AddressInfo> AddressInfos
        {
            get { return N3.Select(x => new AddressInfo(x)); }
            set
            {
                N3.Clear();
                N3.AddRange(value.Select(x=>x.Item));
            }
        }
        
        public IEnumerable<ContactInfo> ContactInfos
        {
            get { return G61.Select(x => new ContactInfo(x)); }
            set
            {
                G61.Clear();
                G61.AddRange(value.Select(x=>x.Item));
            }
        }
 
        
        public string CityName
        {
            get => N4.CityName_01;
            set => N4.CityName_01 = value;
        }        
        public string CountryCode
        {
            get => N4.CountryCode_04;
            set => N4.CountryCode_04 = value;
        }        
        public string StateorProvinceCode
        {
            get => N4.StateorProvinceCode_02;
            set => N4.StateorProvinceCode_02 = value;
        }        
        public string PostalCode
        {
            get => N4.PostalCode_03;
            set => N4.PostalCode_03 = value;
        }        
        public string LocationIdentifier
        {
            get => N4.LocationIdentifier_06;
            set => N4.LocationIdentifier_06 = value;
        }        
        public string LocationQualifier
        {
            get => N4.LocationQualifier_05;
            set => N4.LocationQualifier_05 = value;
        }
        
        
    }

    public enum EntityIdentifierCode
    {
        CN,// Consignee
        SF,// Ship From
        SH,// Shipper
        ST,// Ship To 
    }

    
}