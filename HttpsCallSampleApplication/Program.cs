using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HttpsCallSampleApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string clientId = "UATUser";
            string clientSecret = "secret123";

            HttpWebRequest request = HttpWebRequest.CreateHttp("https://52.25.168.6:10001/api/connect/token");
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add("Authorization", "Basic " + Base64Encode($"{clientId}:{clientSecret}"));

            Dictionary<string, string> bodyParams = new Dictionary<string, string>();
            bodyParams.Add("grant_type", "client_credentials");
            var postData = string.Empty;
            foreach (string key in bodyParams.Keys)
                postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(bodyParams[key]) + "&";
            byte[] data = Encoding.ASCII.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            var response = await request.GetResponseAsync();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseString);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}