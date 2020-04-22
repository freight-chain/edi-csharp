using System;
using System.IO;
using System.Net;

namespace FreightTrust.EDI.AS2
{
    public class AS2Send
    {
        private static string HEADER_RECEIVED_CONTENT_MIC = "Received-Content-MIC";
        private static string HEADER_DISPOSITION = "Disposition";
        private static string HEADER_ORIGINAL_MESSAGE_ID = "Original-Message-ID";
        private static string HEADER_FINAL_RECIPIENT = "Final-Recipient";
        private static string HEADER_ORIGINAL_RECIPIENT = "Original-Recipient";
        private static string HEADER_REPORTING_UA = "Reporting-UA";
        public static string MDNA_REPORTING_UA = "REPORTING_UA";
        public static string MDNA_ORIG_RECIPIENT = "ORIGINAL_RECIPIENT";
        public static string MDNA_FINAL_RECIPIENT = "FINAL_RECIPIENT";
        public static string MDNA_ORIG_MESSAGEID = "ORIGINAL_MESSAGE_ID";
        public static string MDNA_DISPOSITION = "DISPOSITION";
        public static string MDNA_MIC = "MIC";
        public static string DEFAULT_DATE_FORMAT = "ddMMuuuuHHmmssZ";
        public static HttpStatusCode SendResponse(Uri uri, bool sign)
        {
            return HttpStatusCode.Unused;
        }

        public static HttpStatusCode SendResponse(Uri uri, string filename, byte[] fileData, string from, string to,
            ProxySettings proxySettings, int timeoutMs, object signingCert, string signingCertPassword,
            object recipientCert)
        {
            return HttpStatusCode.Unused;
        }
        public static HttpStatusCode SendFile(Uri uri, string filename, byte[] fileData, string from, string to, ProxySettings proxySettings, int timeoutMs, object signingCert, string signingCertPassword, object recipientCert)
        {
            if (String.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");
 
            if (fileData.Length == 0) throw new ArgumentException("filedata");
 
            byte[] content = fileData;
 
            //Initialise the request
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(uri);
            
            if (!String.IsNullOrEmpty(proxySettings.Name))
            {
                WebProxy proxy = new WebProxy(proxySettings.Name);
                
                NetworkCredential proxyCredential = new NetworkCredential();
                proxyCredential.Domain = proxySettings.Domain;
                proxyCredential.UserName = proxySettings.Username;
                proxyCredential.Password = proxySettings.Password;
 
                proxy.Credentials = proxyCredential;
                
                http.Proxy = proxy;
            }
 
            //Define the standard request objects
            http.Method = "POST";
 
            http.AllowAutoRedirect = true;
 
            http.KeepAlive = true;
            http.PreAuthenticate = false; //Means there will be two requests sent if Authentication required.
            http.SendChunked = false;
            http.UserAgent = "Freight Trust";
            //These Headers are common to all transactions
            http.Headers.Add("Mime-Version", "1.0");
            http.Headers.Add("AS2-Version", "1.2"); 
            http.Headers.Add("AS2-From", from);
            http.Headers.Add("AS2-To", to);
            http.Headers.Add("Subject", filename);
            http.Headers.Add("Message-Id", "<AS2_" + DateTime.Now.ToString("hhmmssddd") + ">");
            http.Timeout = timeoutMs;
 
            string contentType = (Path.GetExtension(filename) == ".xml") ? "application/xml" : "application/EDIFACT";
 
            bool encrypt = recipientCert != null;
            bool sign = signingCert != null;
 
            if (!sign && !encrypt)
            {
                http.Headers.Add("Content-Transfer-Encoding", "binary");
                http.Headers.Add("Content-Disposition", "inline; filename=\"" + filename + "\"");
            }
            if (sign)
            {
                // Wrap the file data with a mime header
                content = AS2MIMEUtilities.CreateMessage(contentType, "binary", "", content);
 
                content = AS2MIMEUtilities.Sign(content, signingCert, signingCertPassword, out contentType);
 
                http.Headers.Add("EDIINT-Features", "multiple-attachments");
 
            } 
            if (encrypt)
            {
 
                byte[] signedContentTypeHeader = System.Text.ASCIIEncoding.ASCII.GetBytes("Content-Type: " + contentType + Environment.NewLine);
                byte[] contentWithContentTypeHeaderAdded = AS2MIMEUtilities.ConcatBytes(signedContentTypeHeader, content);
 
                content = AS2Encryption.Encrypt(contentWithContentTypeHeaderAdded, recipientCert, EncryptionAlgorithm.DES3);
                
 
                contentType = "application/pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\"";
            }
            
            http.ContentType = contentType;           
            http.ContentLength = content.Length;
 
            SendWebRequest(http, content);
 
            return HandleWebResponse(http);
        }
 
        private static HttpStatusCode HandleWebResponse(HttpWebRequest http)
        {
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            var statusCode = response.StatusCode;
            response.Close();
            return statusCode;
        }
 
        private static void SendWebRequest(HttpWebRequest http, byte[] fileData)
        {
            Stream oRequestStream = http.GetRequestStream();
            oRequestStream.Write(fileData, 0, fileData.Length);
            oRequestStream.Flush(); 
            oRequestStream.Close();
        }
    }
}