using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TestCallWowAPI.Models;

namespace TestCallWowAPI
{
    class Program
    {
        public static string token = "";
        private static readonly HttpClient httpClient = new HttpClient();
        public const string baseTokenURL = "https://us.battle.net/oauth/token";
        public const string baseURL = "https://us.api.blizzard.com/";

        static async Task Main(string[] args)
        {
            // Get the access token
            token = GetAccessToken("b6b4ab532cb245c28315b1b2c606166b", "6Qw6ncBG8cQJBiPiuD2HihmrIbYUEzqE");

            await SearchCreature("static-us", "en_US", "Dog");

            // Récupèration des types de créature
            var res = await GetCreatureIndex("static-us", "en_US");

            if (res != null)
            {
                foreach (CreatureType creatureType in res.creature_types)
                {
                    Console.WriteLine("Name: " + creatureType.name + " /Type: " + creatureType.key);
                }
            }

        }

        public static string GetAccessToken(string clientId, string clientSecret)
        {
            Console.WriteLine("Début de la récupération du token");
            var client = new RestClient(baseTokenURL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

            Console.WriteLine("Fin de la récupération du token");
            return tokenResponse.access_token;
        }

        /// <summary>
        /// Call the creature index directly
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="requiredNamespace">The required namespace.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>The creature types</returns>
        public static async Task<RootCreatureType> GetCreatureIndex(string requiredNamespace, string locale)
        {
            Console.WriteLine("Début de la récupération des types de créatures");
            UriBuilder uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Path = $"data/wow/creature-type/index";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["namespace"] = requiredNamespace;
            query["locale"] = locale;
            uriBuilder.Query = query.ToString();

            var request = CreateHttpRequest(HttpMethod.Get, uriBuilder.Uri);

            var response = httpClient.SendAsync(request).Result;

            var content = await response.Content.ReadAsStringAsync();

            RootCreatureType result = JsonConvert.DeserializeObject<RootCreatureType>(content);
            if (result.creature_types != null && result.creature_types.Count > 0)
            {
                Console.WriteLine("Récupération des types de créatures OK");
                return result;
            }

            Console.WriteLine("Récupération des types de créatures KO");
            return null;
        }

        /// <summary>
        /// Call the creature index directly
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="requiredNamespace">The required namespace.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>The creature types</returns>
        public static async Task<PaginatedResult> SearchCreature(string requiredNamespace, string locale, string name = null, string orderBy = "id", string sortOrder = "desc", int page = 1)
        {
            Console.WriteLine("Début de la recherche sur les créatures");
            UriBuilder uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Path = $"data/wow/search/creature";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["namespace"] = requiredNamespace;
            query["locale"] = locale;
            query["orderby"] = orderBy + ":" + sortOrder;
            query["_page"] = page.ToString();
            query["name.en_US"] = name;
            uriBuilder.Query = query.ToString();

            var request = CreateHttpRequest(HttpMethod.Get, uriBuilder.Uri);

            var response = httpClient.SendAsync(request).Result;

            var content = await response.Content.ReadAsStringAsync();

            PaginatedResult result = JsonConvert.DeserializeObject<PaginatedResult>(content);
            if (result.results != null && result.results.Count > 0)
            {
                Console.WriteLine("Récupération des résultats de la recherche OK");
                return result;
            }

            Console.WriteLine("Récupération des résultats de la recherche KO");
            return null;
        }

        /// <summary>
        /// Creates the HTTP request asynchronous.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// The http request message
        /// </returns>
        private static HttpRequestMessage CreateHttpRequest(HttpMethod method, Uri url)
        {
            var message = new HttpRequestMessage(method, url);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return message;
        }
    }
}
