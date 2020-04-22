using System;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FreightTrust.EDI
{
    public class LadingInfo
    {
        private LAD _item;

        public LadingInfo(LAD lad = null)
        {
            Item = lad ?? new LAD();
        }

        [JsonIgnore, BsonIgnore]
        public LAD Item
        {
            get => _item ?? (_item = new LAD());
            set => _item = value;
        }

        public PackagingFormCode PackagingFormCode
        {
            get => Enum.Parse<PackagingFormCode>(Item.PackagingFormCode_01, true);
            set => Item.PackagingFormCode_01 = value.ToString();
        }
        public string LadingQuantity
        {
            get => Item.LadingQuantity_02;
            set => Item.LadingQuantity_02 = value;
        }
        public string LadingDescription
        {
            get => Item.LadingDescription_13;
            set => Item.LadingDescription_13 = value;
        }
    }
}