using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace MealApp
{
    public static class InfoDataBase
    {
        public const string ApiUserName = "ParadoxApiUser";
        public const string ApiPassword = "Set@User#123";
        /// <summary>
        /// Ends With /
        /// </summary>
        public static readonly string WebBaseUrl = "https://208.78.220.245/meal/";
        //public static readonly string WebBaseUrl = "https://tickers.abmalik.com/meal/";

        static RestClient RestClient { get; set; }
        static WebClient WebClient { get; set; }

        public static DataSet GetDS(string Query, Hashtable parameters = null, Page page = null)
        {

            if (parameters == null) parameters = new Hashtable();
            //
            //string EncQuery = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(Query));
            string Params = JsonConvert.SerializeObject(parameters);
            //string EncParams = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(Params)); ;
            Uri fullUri = new Uri(WebBaseUrl + "APIs/AppMain/GetDataSet").AttachParameters(new Hashtable
            {
                { "Query", Query },
                { "Params", Params },
                { "UserName", RuntimeCache.UserName}
            });
            if (RestClient == null)
                RestClient = new RestClient(WebBaseUrl);
            RestClient.ThrowOnAnyError = true;
            RestClient.ThrowOnDeserializationError = true;
            RestClient.Authenticator = new HttpBasicAuthenticator(ApiUserName, ApiPassword);
            RestClient.AddHandler("application/json", () => NewtonsoftJsonSerializer.Default);
            RestClient.AddHandler("text/json", () => NewtonsoftJsonSerializer.Default);
            RestClient.AddHandler("text/x-json", () => NewtonsoftJsonSerializer.Default);
            RestClient.AddHandler("text/javascript", () => NewtonsoftJsonSerializer.Default);
            RestClient.AddHandler("*+json", () => NewtonsoftJsonSerializer.Default);

            //RestClient.Timeout = 15000;
            var request = new RestRequest(fullUri, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            //request.JsonSerializer = new NewtonsoftJsonSerializer();
            var restResponse = RestClient.Get<DataSet>(request);
            if (restResponse.ErrorException != null)
                throw restResponse.ErrorException;
            return restResponse.Data;
        }
        public static DataSet GetDS(string Query, List<Pair> pairs)
            => GetDS(Query, pairs.ToHashtable());
        public async static Task<DataSet> GetDSAsync(string Query, Hashtable hT)
            => await Task.Run(() => { return GetDS(Query, hT); });
        public async static Task<DataSet> GetDSAsync(string Query, List<Pair> pairs)
            => await GetDSAsync(Query, pairs.ToHashtable());




        public static Hashtable GetDetailsByLocationCodes(string UserName, string Latitude, string Longitude)
        {

            Uri fullUri = new Uri(WebBaseUrl + "APIs/AppMain/GetCurrentAddress").AttachParameters(new Hashtable
            {
                { "UserName", UserName },
                { "Latitude", Latitude },
                { "Longitude", Longitude }
            });
            if (RestClient == null) RestClient = new RestClient();
            RestClient.Authenticator = new HttpBasicAuthenticator(ApiUserName, ApiPassword);
            //RestClient.Timeout = 15000;
            var request = new RestRequest(fullUri, Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = RestClient.Execute(request);
            if (queryResult.ErrorException != null)
                throw queryResult.ErrorException;
            var r = queryResult.Content;
            return JsonConvert.DeserializeObject<Hashtable>(r);
        }

        public async static Task<Hashtable> GetDetailsByLocationCodesAsync(string UserName, string Latitude, string Longitude)
        => await Task.Run(() => { return GetDetailsByLocationCodes(UserName, Latitude, Longitude); });

    }

    public class APIError
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
    }
}
