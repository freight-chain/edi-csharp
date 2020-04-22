using System;
using System.Collections.Generic;
using System.Globalization;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework;
using EdiFabric.Framework.Writers;
using EdiFabric.Sdk.Demo;
using EdiFabric.Templates.X12004010;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FreightTrust.EDI
{
    /// <summary>
    /// file:///Users/micahosborne/Desktop/BusinessAnalyst/carrier_990.pdf
    /// </summary>
    public class LoadTenderResponseMessage : IEdiMessage
    {
        private TS990 _message;

       // public LoadTenderResponseAction Action { get; set; }
        
        [JsonIgnore, BsonIgnore]
        public TS990 Message
        {
            get => _message ?? (_message = new TS990()
            {
                ST = new ST()
                {
                    TransactionSetIdentifierCode_01 = "990"
                }
            });
            set => _message = value;
        }

        public string ControlNumber
        {
            get => Message.ST.TransactionSetControlNumber_02;
            set => Message.ST.TransactionSetControlNumber_02 = value.PadLeft(9, '0');
        }
        
        #region N9 - Optional
        protected N9 N9 => Message.N9 ?? (Message.N9 = new N9());
        
        public string ReferenceIdentificationQualifier
        {
            get => N9.ReferenceIdentificationQualifier_01;
            set => N9.ReferenceIdentificationQualifier_01 = value;
        }    
        public string ReferenceIdentification
        {
            get => N9.ReferenceIdentification_02;
            set => N9.ReferenceIdentification_02 = value;
        }    
        public string FreeformDescription
        {
            get => N9.FreeformDescription_03;
            set => N9.FreeformDescription_03 = value;
        }    
//        public string Date
//        {
//            get => N9.Date_04;
//            set => N9.Date_04 = value;
//        }    
//        public string TimeCode
//        {
//            get => N9.TimeCode_06;
//            set => N9.TimeCode_06 = value;
//        }    
//        public string Time
//        {
//            get => N9.Time_05;
//            set => N9.Time_05 = value;
//        }        
//        public string ReferenceIdentifier
//        {
//            get => N9.ReferenceIdentifier_07;
//            set => N9.ReferenceIdentifier_07 = value;
//        }
        #endregion
        
        #region N7 Equipment Info - Optional
        protected N7 N7 => Message.N7 ?? (Message.N7 = new N7());
             
        public string EquipmentInitial
        {
            get => N7.EquipmentInitial_01;
            set => N7.EquipmentInitial_01 = value;
        }  
        
        
        
        #endregion
        
        #region B1 - Mandatory - Beginning Segment for Booking or Pick-up/Delivery
        protected B1 B1 => Message.B1 ?? (Message.B1 = new B1());
        public string StandardCarrierAlphaCode
        {
            get => B1.StandardCarrierAlphaCode_01;
            set => B1.StandardCarrierAlphaCode_01 = value;
        }
        public string ShipmentIdentificationNumber
        {
            get => B1.ShipmentIdentificationNumber_02;
            set => B1.ShipmentIdentificationNumber_02 = value;
        }
//        public LoadTenderResponseAction ActionCode
//        {
//            get
//            {
//               
//                return Enum.Parse<LoadTenderResponseAction>(Message.B1.ReservationActionCode_04 ?? "A", true);
//            }
//            set
//            {
//                B1.ReservationActionCode_04 = value.ToString();
//            }
//        }
        public DateTime Date
        {
            get
            {
                
                if (string.IsNullOrEmpty(B1.Date_03))
                    return DateTime.UtcNow;
                
                return DateTime.ParseExact($"{B1.Date_03}", "yyyyMMdd",
                    CultureInfo.InvariantCulture);
            }
            set
            {
                //Message.G62 = Message.G62 ?? new G62();
                B1.Date_03 = value.Date.ToString("yyyyMMdd");
                //Message.G62.Time_04 = value.Date.ToString("HHmm");
            }
        }

        #endregion
        
        protected List<L9> L9 => Message.L9 ?? (Message.L9 = new List<L9>());
        
        protected List<G62> G62 => Message.G62 ?? (Message.G62 = new List<G62>());
        protected List<K1> K1 => Message.K1 ?? (Message.K1 = new List<K1>());
        protected List<V9> V9 => Message.V9 ?? (Message.V9 = new List<V9>());
        protected List<Loop_S5_990> S5Loop => Message.S5Loop ?? (Message.S5Loop = new List<Loop_S5_990>());
        
        
        
        public LoadTenderResponseMessage(TS990 message = null)
        {
            _message = message;
        }


        public string ToEdi()
        {
            return Message.ToEdi(new X12WriterSettings());
        }

       
    }
}