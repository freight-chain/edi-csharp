using System.Collections.Generic;
using System.Linq;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FreightTrust.EDI
{
    public class LoadContactInfo
    {
        private Loop_G61_204 _item;

        public LoadContactInfo(Loop_G61_204 loopG61204 = null)
        {
            Item = loopG61204 ?? new Loop_G61_204();
        }

        [JsonIgnore, BsonIgnore]
        public Loop_G61_204 Item
        {
            get => _item ?? (_item = new Loop_G61_204());
            set => _item = value;
        }

        protected List<L11> L11 => Item.L11 ?? (Item.L11 = new List<L11>());
        protected G61 G61 => Item.G61 ?? (Item.G61 = new G61());
        
        public IEnumerable<ReferenceIdentification> ReferenceIdentifications
        {
            get { return L11.Select(x => new ReferenceIdentification(x)); }
            set
            {
                L11.Clear();
                L11.AddRange(value.Select(x=>x.L11));
            }
        }
        
        public string Name
        {
            get => G61.Name_02;
            set => G61.Name_02 = value;
        }
        public string CommunicationNumberQualifier
        {
            get => G61.CommunicationNumberQualifier_03;
            set => G61.CommunicationNumberQualifier_03 = value;
        }
        public string CommunicationNumber
        {
            get => G61.CommunicationNumber_04;
            set => G61.CommunicationNumber_04 = value;
        }
        public string ContactFunctionCode
        {
            get => G61.ContactFunctionCode_01;
            set => G61.ContactFunctionCode_01 = value;
        }
        public string ContactInquiryReference
        {
            get => G61.ContactInquiryReference_05;
            set => G61.ContactInquiryReference_05 = value;
        }
        
    }
}