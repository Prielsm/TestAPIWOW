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

        static async Task Main(string[] args)
        {
            // Get the access token
            token = GetAccessToken("b6b4ab532cb245c28315b1b2c606166b", "6Qw6ncBG8cQJBiPiuD2HihmrIbYUEzqE");

            var res = await GetCreatureIndex("us", "static-us", "en_US");

        }

        public static string GetAccessToken(string clientId, string clientSecret)
        {
            var client = new RestClient("https://eu.battle.net/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

            return tokenResponse.access_token;
        }

        /// <summary>
        /// Call the creature index directly
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="region">The region.</param>
        /// <param name="requiredNamespace">The required namespace.</param>
        /// <param name="locale">The locale.</param>
        /// <returns></returns>
        public static async Task<string> GetCreatureIndex( string region, string requiredNamespace, string locale)
        {
            UriBuilder uriBuilder = new UriBuilder("https://us.api.blizzard.com/");
            uriBuilder.Path = $"data/wow/creature-type/index";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["namespace"] = requiredNamespace;
            query["locale"] = locale;
            uriBuilder.Query = query.ToString();

            var request = CreateHttpRequest(HttpMethod.Get, uriBuilder.Uri);

            var response = httpClient.SendAsync(request).Result;

            var content = await response.Content.ReadAsStringAsync();

            return content;
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
