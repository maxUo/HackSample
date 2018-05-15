using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnblockHackNET.BL.DB
{
    public static class DataBaseService
    {
        private static string _apiUri = "http://rosum-rigovon.westeurope.cloudapp.azure.com:3000/api/";

        public static async Task<List<string>> CreateSeed(string pass)
        {
            var result = new List<string>();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + "createSeed"),
                    Method = HttpMethod.Post
                };

                //request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                List<KeyValuePair<string, string>> tmp = new List<KeyValuePair<string, string>>();
                tmp.Add(new KeyValuePair<string, string>("userID", pass));

                request.Content = new FormUrlEncodedContent(tmp);
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = json.Split();
                    if (!Equals(myObj, null))
                        result.AddRange(myObj);
                }
            }


            return result;
        }

        public static async Task<List<string>> DecryptSeed(List<string> log, string pass)
        {
            //decryptSeed
            var result = new List<string>();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + "decryptSeed"),
                    Method = HttpMethod.Post
                };

                //request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                List<KeyValuePair<string, string>> tmp = new List<KeyValuePair<string, string>>();
                StringBuilder builder = new StringBuilder();

                foreach (var item in log)
                {
                    builder.Append(item + " ");
                }

                tmp.Add(new KeyValuePair<string, string>("userID", pass));
                tmp.Add(new KeyValuePair<string, string>("encryptedSeed", builder.ToString()));

                request.Content = new FormUrlEncodedContent(tmp);
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = json.Split();
                    if (!Equals(myObj, null))
                        result.AddRange(myObj);
                }
            }


            return result;        
        }

        public static async Task<ObservableCollection<Vote>> GetAllVotes()
        {
            var result = new ObservableCollection<Vote>();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + $"findAllVotes"),
                    Method = HttpMethod.Post
                };

                request.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = JsonConvert.DeserializeObject<ObservableCollection<Vote>>(json);
                    if (!Equals(myObj, null))
                        result = myObj;
                }
            }

            return result;
        }

        public static async Task<ObservableCollection<VotationProposal>> GetAllVotesCompose()
        {
            var result = new ObservableCollection<VotationProposal>();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + $"Proposal"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = JsonConvert.DeserializeObject<ObservableCollection<VotationProposal>>(json);
                    if (!Equals(myObj, null))
                        result = myObj;
                }
            }

            return result;
        }

        public static async Task<ObservableCollection<Organisation>> GetAllOrganisationCompose()
        {
            var result = new ObservableCollection<Organisation>();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + $"Organisation"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = JsonConvert.DeserializeObject<ObservableCollection<Organisation>>(json);
                    if (!Equals(myObj, null))
                        result = myObj;
                }
            }

            return result;
        }

        public static async Task<Investor> GetInvestor(string id)
        {
            var result = new Investor();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + $"Investor/{id}"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = JsonConvert.DeserializeObject<Investor>(json);
                    if (!Equals(myObj, null))
                        result = myObj;
                }
            }

            return result;
        }

        public static async Task<bool> UserAuthCheck(string log)
        {
            var result = false;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + $"users/{log}"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = JsonConvert.DeserializeObject<bool>(json);
                    if (!Equals(myObj, null))
                        result = myObj;
                }
            }

            return result;
        }

        public static async Task<User> GetUser(string log)
        {
            var result = new User();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_apiUri + $"users/{log}/getuser"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("Accept", "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var myObj = JsonConvert.DeserializeObject<User>(json);
                    if (!Equals(myObj, null))
                        result = myObj;
                }
            }

            return result;
        }
    }
}
