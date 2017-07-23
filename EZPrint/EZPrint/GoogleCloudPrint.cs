﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using GoogleCloudPrintServices.DTO;
namespace GoogleCloudPrintServices.Support
{
    public class GoogleCloudPrint
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
        private const int ServiceTimeout = 10000;
        public GoogleCloudPrint(String source)
        {
            Source = source;
        }
        public CloudPrintJob PrintDocument(string printerId, string title, byte[] document, String mimeType)
        {
            try
            {
                string authCode;
                if (!Authorize(out authCode))
                    return new CloudPrintJob { success = false };
                var b64 = Convert.ToBase64String(document);
                var request = (HttpWebRequest)WebRequest.Create("http://www.google.com/cloudprint/submit?output=json&printerid=" + printerId);
                request.Method = "POST";
                // Setup the web request
                SetupWebRequest(request);
                // Add the headers
                request.Headers.Add("X-CloudPrint-Proxy", Source);
                request.Headers.Add("Authorization", "GoogleLogin auth=" + authCode);
                var p = new PostData();
                p.Params.Add(new PostDataParam { Name = "printerid", Value = printerId, Type = PostDataParamType.Field });
                p.Params.Add(new PostDataParam { Name = "capabilities", Value = "{\"capabilities\":[{}]}", Type = PostDataParamType.Field });
                p.Params.Add(new PostDataParam { Name = "contentType", Value = "dataUrl", Type = PostDataParamType.Field });
                p.Params.Add(new PostDataParam { Name = "title", Value = title, Type = PostDataParamType.Field });
                p.Params.Add(new PostDataParam
                {
                    Name = "content",
                    Type = PostDataParamType.Field,
                    Value = "data:" + mimeType + ";base64," + b64
                });
                var postData = p.GetPostData();
                Trace.WriteLine(postData);
                byte[] data = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "multipart/form-data; boundary=" + p.Boundary;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                // Get response
                var response = (HttpWebResponse)request.GetResponse();
                var responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var serializer = new DataContractJsonSerializer(typeof(CloudPrintJob));
                var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseContent));
                var printJob = serializer.ReadObject(ms) as CloudPrintJob;
                return printJob;
            }
            catch (Exception ex)
            {
                return new CloudPrintJob { success = false, message = ex.Message };
            }
        }
        public CloudPrinters Printers
        {
            get
            {
                var printers = new CloudPrinters();
                string authCode;
                if (!Authorize(out authCode))
                    return new CloudPrinters { success = false };
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create("http://www.google.com/cloudprint/search?output=json");
                    request.Method = "POST";
                    // Setup the web request
                    SetupWebRequest(request);
                    // Add the headers
                    request.Headers.Add("X-CloudPrint-Proxy", Source);
                    request.Headers.Add("Authorization", "GoogleLogin auth=" + authCode);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = 0;
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var serializer = new DataContractJsonSerializer(typeof(CloudPrinters));
                    var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseContent));
                    printers = serializer.ReadObject(ms) as CloudPrinters;
                    return printers;
                }
                catch (Exception)
                {
                    return printers;
                }
            }
        }
        private bool Authorize(out string authCode)
        {
            var result = false;
            authCode = "";
            var queryString = String.Format("https://www.google.com/accounts/ClientLogin?accountType=HOSTED_OR_GOOGLE&Email={0}&Passwd={1}&service=cloudprint&source={2}",
            UserName, Password, Source);
            var request = (HttpWebRequest)WebRequest.Create(queryString);
            // Setup the web request
            SetupWebRequest(request);
            var response = (HttpWebResponse)request.GetResponse();
            var responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var split = responseContent.Split('\n');
            foreach (var s in split)
            {
                var nvsplit = s.Split('=');
                if (nvsplit.Length == 2)
                {
                    if (nvsplit[0] == "Auth")
                    {
                        authCode = nvsplit[1];
                        result = true;
                    }
                }
            }
            return result;
        }
        private static void SetupWebRequest(HttpWebRequest webRequest)
        {
            // Get the details
            var appSettings = ConfigurationManager.AppSettings;
            // Create some credentials
            if (!String.IsNullOrWhiteSpace(appSettings["ProxyUsername"]))
            {
                var cred = new NetworkCredential(appSettings["ProxyUsername"], appSettings["ProxyPassword"],
                appSettings["ProxyDomain"]);
                // Set the credentials
                webRequest.Credentials = cred;
                webRequest.Proxy = WebRequest.DefaultWebProxy;
                webRequest.Proxy.Credentials = cred;
            }
            // Set the timeout
            webRequest.Timeout = ServiceTimeout;
            webRequest.ServicePoint.ConnectionLeaseTimeout = ServiceTimeout;
            webRequest.ServicePoint.MaxIdleTime = ServiceTimeout;
            // Turn off the 100's
            webRequest.ServicePoint.Expect100Continue = false;
        }
    }
}