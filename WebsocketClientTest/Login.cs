using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketClientTest
{
    public static class Login
    {
        static string BiostarApiUrl = "https://192.168.200.51:443/api";
        public static async Task<string> GetSessionId()
        {
            string user = "admin";
            string pass = "P@ssw0rd";
                string session_id = string.Empty;
                try
                {
                    Root root = new Root();
                    root.User = new LoginUser();
                    root.User.login_id = user;
                    root.User.password = pass;
                    //json
                    var todoItemJson = new StringContent(
                             System.Text.Json.JsonSerializer.Serialize(root),
                               Encoding.UTF8,
                               "application/json");

                    //send request
                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                        //httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        //send request
                        using (var client = new HttpClient(httpClientHandler))
                        {
                            HttpResponseMessage response = await client.PostAsync(BiostarApiUrl + "/login", todoItemJson);
                            //response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            IEnumerable<string> values;
                            if (response.Headers.TryGetValues("bs-session-id", out values))
                            {
                                session_id = values.First();
                            }
                        Console.WriteLine("bs-session-id" + session_id);
                    }
                    }

                }
                catch (Exception ex)
                {
                Console.WriteLine(ex.Message);
            }

                return session_id;
            
        }

        public static async Task EventStarts(string bs_session_id)
        {
            string responseBody = string.Empty;
            var todoItemJson = new StringContent(
              "application/json");
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
               // httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Add("bs-session-id", bs_session_id);
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        //client.Timeout = TimeSpan.FromMinutes(1);
                        response = await client.PostAsync(BiostarApiUrl + "/events/start", todoItemJson);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Socket is ... Connected");
                    Console.WriteLine(responseBody.ToString());
                }
            }
        }
    }
}
