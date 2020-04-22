using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BlockArray.Attributes;
using EdiFabric.Core.Annotations.Edi;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework;
using EdiFabric.Framework.Writers;
using EdiFabric.Sdk.Demo;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using TS997 = EdiFabric.Templates.X12004010.TS997;

namespace FreightTrust.EDI
{

    [BsonIgnoreExtraElements]
    public class LoadTenderMessage : IEdiMessage
    {
        private TS204 _message;
        private L3 _l3;

        public LoadTenderMessage(TS204 edi = null)
        {
            Message = edi ?? new TS204();
        }

        [JsonIgnore, BsonIgnore]
        public TS204 Message
        {
            get => _message ?? (_message = new TS204());
            set => _message = value;
        }

        #region ST Standard Transaction Header - Done

        public string TransactionSetIdentifierCode
        {
            get
            {
                Message.ST = Message.ST ?? new ST();
                return Message.ST.TransactionSetIdentifierCode_01;
            }

            set
            {
                Message.ST = Message.ST ?? new ST();
                Message.ST.TransactionSetIdentifierCode_01 = value;
            }
        }

        public string TransactionSetControlNumber
        {
            get
            {
                return Message.ST.TransactionSetControlNumber_02?.PadLeft(9,'0') ?? "000000000";
            }
            set
            {
                Message.ST = Message.ST ?? new ST();
                if (value != null)
                Message.ST.TransactionSetControlNumber_02 = value.PadLeft(9,'0');
            }
        }

        #endregion

        #region B2 - Done

        public string StandardCarrierAlphaCode
        {
            get
            {
                Message.B2 = Message.B2 ?? new B2();
                return Message.B2.StandardCarrierAlphaCode_02;
            }
            set
            {
                Message.B2 = Message.B2 ?? new B2();
                Message.B2.StandardCarrierAlphaCode_02 = value;
            }
        }

        /// <summary>
        /// Identification number assigned to the shipment by the shipper that uniquely identifies the shipment from origin to ultimate
        /// destination and is not subject to modification; (Does not contain blanks or special characters)
        /// </summary>
        public string ShipmentIdentificationNumber
        {
            get
            {
                Message.B2 = Message.B2 ?? new B2();
                return Message.B2.ShipmentIdentificationNumber_04;
            }
            set => Message.B2.ShipmentIdentificationNumber_04 = value;
        }

        /// <summary>
        /// K Kilograms
        /// L Pounds
        /// </summary>
        public WeightUnitCode WeightUnitCode
        {
            get
            {
                Message.B2 = Message.B2 ?? new B2();
                return Enum.Parse<WeightUnitCode>(Message.B2.WeightUnitCode_05 ?? EDI.WeightUnitCode.L.ToString(), true);
            }
            set
            {
                Message.B2 = Message.B2 ?? new B2();
                Message.B2.WeightUnitCode_05 = value.ToString();
            }
        }

        public PaymentMethod PaymentMethod
        {
            get
            {
                Message.B2 = Message.B2 ?? new B2();
                return Enum.Parse<PaymentMethod>(Message.B2.PaymentMethodCode_12 ?? "PP", true);
            }
            set
            {
                Message.B2 = Message.B2 ?? new B2();
                Message.B2.PaymentMethodCode_12 = value.ToString();
            }
        }

        public TransactionSetPurpose TransactionSetPurpose
        {
            get
            {
                Message.B2A = Message.B2A ?? new B2A();
                switch (Message.B2A.TransactionSetPurposeCode_01)
                {
                    case "01":
                        return TransactionSetPurpose.Cancellation;
                    case "04":
                        return TransactionSetPurpose.Change;
                    case "49":
                        return TransactionSetPurpose.OriginalNoResponseNecessary;
                    default:
                        return TransactionSetPurpose.Original;
                }
            }
            set
            {
                Message.B2A = Message.B2A ?? new B2A();
                switch (value)
                {
                    case TransactionSetPurpose.Change:
                        Message.B2A.TransactionSetPurposeCode_01 = "04";
                        break;
                    case TransactionSetPurpose.Cancellation:
                        Message.B2A.TransactionSetPurposeCode_01 = "01";
                        break;
                    case TransactionSetPurpose.OriginalNoResponseNecessary:
                        Message.B2A.TransactionSetPurposeCode_01 = "49";
                        break;
                    default:
                        Message.B2A.TransactionSetPurposeCode_01 = "00";
                        break;
                }
            }
        }

        #endregion

        #region L11 Purpose - Done

        public IEnumerable<ReferenceIdentification> ReferenceIdentifications
        {
            get
            {
                Message.L11 = Message.L11 ?? new List<L11>();
                foreach (var item in Message.L11)
                {
                    yield return new ReferenceIdentification(item);
                }
            }
            set
            {
                Message.L11 = new List<L11>();
                foreach (var item in value)
                {
                    Message.L11.Add(item.L11);
                }
            }
        }

        #endregion

        #region G62 DateTime Stuff - Done

        public DateTime RespondBy
        {
            get
            {
                Message.G62 = Message.G62 ?? new G62();
                if (string.IsNullOrEmpty(Message.G62.Date_02) || string.IsNullOrEmpty(Message.G62.Time_04))
                    return DateTime.UtcNow;
                return DateTime.ParseExact($"{Message.G62.Date_02}{Message.G62.Time_04}", "yyyyMMddHHmm",
                    CultureInfo.InvariantCulture);
            }
            set
            {
                Message.G62 = Message.G62 ?? new G62();
                Message.G62.Date_02 = value.Date.ToString("yyyyMMdd");
                Message.G62.Time_04 = value.Date.ToString("HHmm");
            }
        }

        public string RespondByTimeCode
        {
            get
            {
                Message.G62 = Message.G62 ?? new G62();
                return Message.G62.TimeCode_05;
            }
            set
            {
                Message.G62 = Message.G62 ?? new G62();
                Message.G62.TimeCode_05 = value;
            }
        }

        #endregion

        #region AT5 Bill Of Lading Requirements - Done

        public IEnumerable<BolHandlingRequirements> HandlingRequirements
        {
            get
            {
                Message.AT5 = Message.AT5 ?? new List<AT5>();
                foreach (var item in Message.AT5)
                {
                    yield return new BolHandlingRequirements(item);
                }
            }
            set
            {
                Message.AT5 = new List<AT5>();
                foreach (var item in value)
                {
                    Message.AT5.Add(item.AT5);
                }
            }
        }

        #endregion

        #region LH6 Hazardous Certifcation

        #endregion

        // Done
        public string SpecialInstructions
        {
            get
            {
                if (Message.NTE == null) return string.Empty;
                return string.Join(string.Empty, Message.NTE.Select(x => x.Description_02));
            }
            set
            {
                Message.NTE = Enumerable.Range(0, value.Length / 80)
                    .Select(i => value.Substring(i * 80, 80)).Select(x => new NTE() {Description_02 = x}).ToList();
            }
        }

        #region N1 - Parties - Done

        public IEnumerable<PartyInfo> Parties
        {
            get
            {
                Message.N1Loop = Message.N1Loop ?? new List<Loop_N1_204>();

                foreach (var item in Message.N1Loop)
                {
                    yield return new PartyInfo(item);
                }
            }
            set
            {
                Message.N1Loop = new List<Loop_N1_204>();
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        Message.N1Loop.Add(item.Item);
                    }
                }
            }
        }

        #endregion

        #region N7 Equipment Details - Done

        protected List<Loop_N7_204> N7 => Message.N7Loop ?? (Message.N7Loop = new List<Loop_N7_204>());
        
        public IEnumerable<EquipmentInfo> EquipmentInfos
        {
            get { return N7.Select(x => new EquipmentInfo(x)); }
            set
            {
                N7.Clear();
                N7.AddRange(value.Select(x=>x.Item));
            }
        }
        #endregion
        
        #region S5 Stops - Done

        protected List<Loop_S5_204> S5 => Message.S5Loop ?? (Message.S5Loop = new List<Loop_S5_204>());
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<StopInfo> StopInfos
        {
            get { return S5.Select(x => new StopInfo(x)); }
            set
            {
                S5.Clear();
                S5.AddRange(value.Select(x=>x.Item));
            }
        }
        #endregion

        public string Weight
        {
            get => L3.Weight_01;
            set => L3.Weight_01 = value;
        }
        public WeightQualifier WeightQualifier
        {
            get => Enum.Parse<WeightQualifier>(L3.WeightQualifier_02?? WeightQualifier.G.ToString(), true);
            set => L3.WeightQualifier_02 = value.ToString();
        }
        public string FreightRate
        {
            get => L3.FreightRate_03;
            set => L3.FreightRate_03 = value;
        }

        public string Charge
        {
            get => L3.Charge_05;
            set => L3.Charge_05 = value;
        }
        public string Advances
        {
            get => L3.Advances_06;
            set => L3.Advances_06 = value;
        }
        public string PrepaidAmount
        {
            get => L3.PrepaidAmount_07;
            set => L3.PrepaidAmount_07 = value;
        }
        public string SpecialChargeorAllowanceCode
        {
            get => L3.SpecialChargeorAllowanceCode_08;
            set => L3.SpecialChargeorAllowanceCode_08 = value;
        }
        public string Volume
        {
            get => L3.Volume_09;
            set => L3.Volume_09 = value;
        }
        public VolumeUnitQualifier VolumeUnitQualifier
        {
            get => Enum.Parse<VolumeUnitQualifier>(L3.VolumeUnitQualifier_10 ?? VolumeUnitQualifier.E.ToString(), true);
            set => L3.VolumeUnitQualifier_10 = value.ToString();
        }
        public string LadingQuantity
        {
            get => L3.LadingQuantity_11;
            set => L3.LadingQuantity_11 = value;
        }
        public WeightUnitCode TotalWeightUnitCode
        {
            get => Enum.Parse<WeightUnitCode>(L3.WeightUnitCode_12 ?? WeightUnitCode.L.ToString(), true);
            set => L3.WeightUnitCode_12 = value.ToString();
        }
        public string TariffNumber
        {
            get => L3.TariffNumber_13;
            set => L3.TariffNumber_13 = value;
        }
        public string DeclaredValue
        {
            get => L3.DeclaredValue_14;
            set => L3.DeclaredValue_14 = value;
        }
        public RateValueQualifier RateValueQualifier
        {
            get => Enum.Parse<RateValueQualifier>(L3.RateValueQualifier_04 ?? RateValueQualifier.FR.ToString(), true);
            set => L3.RateValueQualifier_04 = value.ToString();
        }
        protected L3 L3
        {
            get => _l3 ?? (_l3 = new L3());
            set => _l3 = value;
        }

        public string ToEdi()
                 {
                     return Message.ToEdi(new X12WriterSettings());
                 }
    }
    public enum CommunicationNumberQualifier
    {
        /// <summary>
        /// Email
        /// </summary>
        EM,
        /// <summary>
        /// Telephone
        /// </summary>
        TL
    }
    public enum WeightQualifier
    {
        /// <summary>
        /// Gross weight
        /// </summary>
        G
    }
    public enum VolumeUnitQualifier
    {
        /// <summary>
        /// Cubic Feet
        /// </summary>
        E
    }
    public enum RateValueQualifier
    {
        /// <summary>
        /// Flat Rate
        /// </summary>
        FR,
        /// <summary>
        /// Rate Per Hundred Weight Per Mile
        /// </summary>
        HM,
        /// <summary>
        /// Minimum per Service
        /// </summary>
        MA,
        /// <summary>
        /// Per Mile per Service
        /// </summary>
        MB,
        /// <summary>
        /// Per load
        /// </summary>
        PL,
        /// <summary>
        /// Per Mile
        /// </summary>
        PM
    }
}