using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JokeGenerator {

	// the JsonFeed class is the data fetching class
	class JsonFeed : IJsonFeed {

		private const int ResultsLimit = 9;

		private static HttpClient _Client;
		static private string _Url = "";
		static private int _NumberOfresults;
		static private String _JsonKey; // key to retrieve specific json values

		/// <summary>
		/// JsonFeed constructor.
		/// </summary>
		/// <param name="endPoint">The url for the website.</param>
		/// <param name="results">The number of times the rest call is to be executed.</param>
		/// <returns></returns>
		public JsonFeed(string endPoint, int numberOfresults, String jsonKey = null) {

			_JsonKey = (jsonKey != null ) ? jsonKey : null;
			_NumberOfresults = (numberOfresults > 1 && numberOfresults <= ResultsLimit) ? numberOfresults : 1;
			_Url = endPoint;

			try {

				_Client = new HttpClient {

					BaseAddress = new Uri(_Url)
				};
			} catch(HttpRequestException e) {

				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// returns the a list of json values in a list.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>A list of json encoded strings</returns>
		public List<String> GetJSONValuesList(String resource = "", Dictionary<String, String> args = null) {

			List<String> jsonList;
			String url;

			if(args != null && args.Count > 0)
				url = AssembleURL(resource, args);
			else
				url = resource;

			if(_JsonKey == null)
				jsonList = ProcessNoJsonKey(url);
			else
				jsonList = ProcessWithJsonkKey(url);

			return jsonList;
		}

		/// <summary>
		/// returns the assembled URL as a string.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>The url as a string.</returns>
		private String AssembleURL(String resource, Dictionary<String, String> args) {

			String url = resource;
			int parameterCount = 0;

			if(resource.Contains('?'))
				url += "&";
			else
				url += "?";

			foreach(var arg in args) {
				if(parameterCount > 0 && arg.Value != "" & arg.Value != null) {
					url += "&";
				}

				if(arg.Value != null && arg.Value != null) {
					url += arg.Key + "=";
					url += arg.Value;
				}

				parameterCount++;
			}

			return url;
		}

		/// <summary>
		/// Process a single json record
		/// </summary>
		/// <param name="url">The rest api url to retrieve the json string.</param>
		/// <returns>A list of strings containing a single json record.</returns>
		private List<String> ProcessNoJsonKey(String url) {

			String json = Task.FromResult(_Client.GetStringAsync(url).Result).Result;
			return JsonConvert.DeserializeObject<List<String>>(json);
		}

		/// <summary>
		/// Processes specific keys from json records and returns the results as a list of strings
		/// </summary>
		/// <param name="url">The rest api rul to retreive the json string.</param>
		/// <returns>A list of strings containing one or more json records</returns>
		private List<String> ProcessWithJsonkKey(String url) {

			List<String> results = null;
			String result;			

			// retrieve results from the specified json key
			for(int i = 0; i < _NumberOfresults; i++) {

				String json = Task.FromResult(_Client.GetStringAsync(url).Result).Result;

				if(results == null) {

					results = new List<String>();
					result = JsonConvert.DeserializeObject<dynamic>(json)[_JsonKey];
					results.Add(result);
				} else {

					result = JsonConvert.DeserializeObject<dynamic>(json)[_JsonKey];
					results.Add(result);
				}
			}			

			return results;
		}

		/// <summary>
		/// Returns the maximum number of results that can be returned
		/// </summary>		
		/// <returns>Maximum allowable number of results produced by JSonFeed.</returns>
		public static int GetResultLimit() {

			return ResultsLimit;
		}

		List<string> IJsonFeed.GetJSONValuesList(string resource, Dictionary<string, string> args) {
			throw new NotImplementedException();
		}

		string IJsonFeed.AssembleURL(string resource, Dictionary<string, string> args) {
			throw new NotImplementedException();
		}

		List<string> IJsonFeed.ProcessNoJsonKey(string url) {
			throw new NotImplementedException();
		}

		List<string> IJsonFeed.ProcessWithJsonkKey(string url) {
			throw new NotImplementedException();
		}
	}
}
