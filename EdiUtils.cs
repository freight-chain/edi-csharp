using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework;
using EdiFabric.Framework.Writers;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI.Wrappers;

namespace FreightTrust.EDI
{
    public static class EdiUtils
    {
        public static string PICKUP_APPOINTMENT_SCHEDULED_DATE = "77";
        public static string DELIVERY_APPOINTMENT_SCHEDULED_DATE = "78";
        
        public static string PICKUP_REQUESTED_SCHEDULED_TIME = "4";
        public static string DELIVERY_REQUESTED_SCHEDULED_TIME = "5";
        
        public static string PICKUP_APPOINTMENT_SCHEDULED_TIME = "4";
        public static string DELIVERY_APPOINTMENT_SCHEDULED_TIME = "5";
        
        public static string REQUESTED_DELIVERY_DATE = "68";
        public static string REQUEST_SHIP_PICKUP_DATE = "10";
        public static string REQUESTED_DELIVERY_TIME = "5";
        public static string REQUEST_PICKUP_TIME = "4";
        public static string DELIVER_NO_LATER_DATE = "54";
        public static string SHIP_NO_LATER_DATE = "38";
        public static string DELIVER_NO_LATER_TIME = "L";
        public static string SHIP_NO_LATER_TIME = "K";

        
        public static async Task<string> CreateTransaction(EdiMessage message, string controlNumber, 
            string senderId = "SENDER1", 
            string receiverId = "RECEIVER1", 
            bool ackRequested = true, 
            string testIndicator = "T")
        {
            //var sb = new StringBuilder();
            using (var stream = new MemoryStream())
            {
                using(var writer = new X12Writer(stream, new X12WriterSettings(Separators.X12)))
                {
                    var isa = BuildIsa(controlNumber, senderId, "14", receiverId, "16", ackRequested ? "1" : "0",
                        testIndicator);
                    // construct the interchange header ...
                    await writer.WriteAsync(isa);

                    var gs = BuildGs(controlNumber, senderId, receiverId);
                    await writer.WriteAsync(gs);  
                                      
                
                    await writer.WriteAsync(message);
                }

                stream.Position = 0;
                StreamReader reader = new StreamReader( stream );
                string text = reader.ReadToEnd();

                return text;

            }
          
            
        }
        public static GS BuildGs(string controlNumber, 
            string senderId = "SENDER1", 
            string receiverId = "RECEIVER1")
        {
            return new GS
            {
                //  Functional ID Code
                CodeIdentifyingInformationType_1 = "IN",
                //  Application Senders Code
                SenderIDCode_2 = senderId,
                //  Application Receivers Code
                ReceiverIDCode_3 = receiverId,
                //  Date
                Date_4 = DateTime.Now.ToString("yyyyMMdd"),
                //  Time
                Time_5 = DateTime.Now.ToString("HHmm"),
                //  Group Control Number
                //  Must be unique to both partners for this interchange
                GroupControlNumber_6 = controlNumber.PadLeft(9, '0'),
                //  Responsible Agency Code
                TransactionTypeCode_7 = "X",
                //  Version/Release/Industry id code
                VersionAndRelease_8 = "004010"
            };
        }
        public static ISA BuildIsa(string controlNumber, 
            string senderId = "SENDER1", 
            string senderQ = "14", 
            string receiverId = "RECEIVER1", 
            string receiverQ = "16", 
            string ackRequested = "1", 
            string testIndicator = "T")
        {
            return new ISA
            {
                //  Authorization Information Qualifier
                AuthorizationInformationQualifier_1 = "00",
                //  Authorization Information
                AuthorizationInformation_2 = "".PadRight(10),
                //  Security Information Qualifier
                SecurityInformationQualifier_3 = "00",
                //  Security Information
                SecurityInformation_4 = "".PadRight(10),
                //  Interchange ID Qualifier
                SenderIDQualifier_5 = senderQ,
                //  Interchange Sender
                InterchangeSenderID_6 = senderId.PadRight(15),
                //  Interchange ID Qualifier
                ReceiverIDQualifier_7 = receiverQ,
                //  Interchange Receiver
                InterchangeReceiverID_8 = receiverId.PadRight(15),
                //  Date
                InterchangeDate_9 = DateTime.UtcNow.ToString("yyyyMMdd"),
                //  Time
                InterchangeTime_10 = DateTime.UtcNow.ToString("HHmm"),
                //  Standard identifier
                InterchangeControlStandardsIdentifier_11 = "U",
                //  Interchange Version ID
                //  This is the ISA version and not the transaction sets versions
                InterchangeControlVersionNumber_12 = "00204",
                //  Interchange Control Number
                InterchangeControlNumber_13 = controlNumber.PadLeft(9, '0'),
                //  Acknowledgment Requested (0 or 1)
                AcknowledgementRequested_14 = ackRequested,
                //  Test Indicator
                UsageIndicator_15 = testIndicator,
            };
        }

        public static string FromNotes(IEnumerable<Note> nte)
        {
            return string.Join(string.Empty, nte.Select(x => x.Description));
        }

        public static DateTime ToDate(string tDate, string timeCode)
        {
            return DateTime.ParseExact($"{tDate}", "yyyyMMdd",
                CultureInfo.InvariantCulture);
        }
        
        public static DateTime ToDate(string tDate, string tTime, string timeCode)
        {
            if (string.IsNullOrEmpty(tDate)) return DateTime.UtcNow;
            if (string.IsNullOrEmpty(tTime)) return ToDate(tDate, timeCode);
            
            
            return DateTime.ParseExact($"{tDate}{tTime}", "yyyyMMddHHmm",
                CultureInfo.InvariantCulture);
        }

        public static string ToDateString(DateTime value)
        {
            return value.ToString("yyyyMMdd");
        }
        public static string ToTimeString(DateTime value)
        {
            return value.ToString("HHmm");
        }
    }
}