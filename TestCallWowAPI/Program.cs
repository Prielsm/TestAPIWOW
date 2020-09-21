using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using TestCallWowAPI.Models;

namespace TestCallWowAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the access token
            var token = GetAccessToken("b6b4ab532cb245c28315b1b2c606166b", "6Qw6ncBG8cQJBiPiuD2HihmrIbYUEzqE");

            var res = GetCreatureIndex2(token, "us", "static-us", "en_US");
            // var auctionFileURL = GetAuctionFileUrl(token, "eu", "Blackhand");
            // var auctions = GetAuctions(auctionFileURL);

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

        public static string GetCreatureIndex(string token, string region, string requiredNamespace, string locale)
        {
            var client = new RestClient("https://" + region + ".api.blizzard.com/data/wow/creature-family/index");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", $"Bearer {token}");
            request.AddHeader("namespace", requiredNamespace);
            request.AddHeader("locale", locale);
            IRestResponse response = client.Execute(request);

            return request.ToString();
        }

        /// <summary>
        /// Call the creature index directly
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="region">The region.</param>
        /// <param name="requiredNamespace">The required namespace.</param>
        /// <param name="locale">The locale.</param>
        /// <returns></returns>
        public static string GetCreatureIndex2(string token, string region, string requiredNamespace, string locale)
        {
            string html = "";
            string url = @"https://us.api.blizzard.com/data/wow/creature-family/index?namespace=static-us&locale=en_US&access_token=USut9r6uSXBwJKAe0llB0RPJAEhmaWmEky";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        public static string GetAuctionFileUrl(string token, string region, string realm)
        {
            string fileUrl;
            var client = new RestClient("https://" + region + ".api.blizzard.com/wow/auction/data/" + realm);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", $"Bearer {token}");
            request.AddHeader("namespace", $"dynamic-eu");
            IRestResponse response = client.Execute(request);

            var auctionApiResponse = JsonConvert.DeserializeObject<AuctionApiResponse>(response.Content);
            fileUrl = auctionApiResponse.files.First().url;
            return fileUrl;
        }

        public static List<Auction> GetAuctions(string fileUrl)
        {
            var client = new RestClient(fileUrl);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<AuctionFileContents>(response.Content).auctions;
        }
    }
}
