using System;
using System.Xml.Serialization;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FreightTrust.EDI
{
    public class BolHandlingRequirements
    {
        private AT5 _at5;

        [JsonIgnore, BsonIgnore]
        public AT5 AT5 => _at5 ?? (_at5 = new AT5());

        public BolHandlingRequirements(AT5 at5 = null)
        {
            _at5 = at5 ?? new AT5();
        }

        public string SpecialHandlingDescription
        {
            get => AT5.SpecialHandlingDescription_03;
            set => AT5.SpecialHandlingDescription_03 = value;
        }

        public SpecialHandlingCode SpecialHandlingCode
        {
            get => Enum.Parse<SpecialHandlingCode>(AT5.SpecialHandlingCode_01, true);
            set => AT5.SpecialHandlingCode_01 = value.ToString();
        }

        public SpecialServicesCode SpecialServicesCode
        {
            get => Enum.Parse<SpecialServicesCode>(AT5.SpecialServicesCode_02 ?? SpecialServicesCode.D1.ToString(), true);
            set => AT5.SpecialServicesCode_02 = value.ToString();
        }
    }
}