using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HttpsCallSampleApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = await ConnectAndGetToken();
            Console.WriteLine($"[*] Succefully got the access token: {token.access_token.Substring(0, 30)}...");

            AddClientModel addClientModel = new AddClientModel();
            await AddNewClient(addClientModel, token.access_token);

            Console.WriteLine("[*] Press any key to exit the utility.");
            Console.ReadKey();
        }

        private static async Task<TokenModel> ConnectAndGetToken()
        {
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            HttpWebRequest request = HttpWebRequest.CreateHttp(CombineUri(ConfigurationManager.AppSettings["ISURL"], "connect/token"));
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
            return JsonConvert.DeserializeObject<TokenModel>(responseString);
        }

        private static async Task AddNewClient(AddClientModel addClientModel, string token)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var httpResponse = await client.PostAsync(CombineUri(ConfigurationManager.AppSettings["ISURL"], "api/Clients"), ToHttpJsonContent(addClientModel));
            var jsonResult = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.IsSuccessStatusCode)
                Console.WriteLine($"[*] Successfully added the client: Message from API: {jsonResult}");
            else
                Console.WriteLine($"[x] Error in adding client: {httpResponse.StatusCode}");
        }

        private static HttpContent ToHttpJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static Uri CombineUri(string uri, string uri2)
        {
            if (uri.EndsWith("/") && uri2.StartsWith("/"))
                return new Uri($"{uri.TrimEnd('/')}{uri2}");
            else if (uri.EndsWith("/") || uri2.StartsWith("/"))
                return new Uri($"{uri}{uri2}");
            return new Uri($"{uri}/{uri2}");
        }
    }
}