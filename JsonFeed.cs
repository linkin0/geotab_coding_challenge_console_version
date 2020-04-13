using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JokeGenerator {

	// the JsonFeed class is the data fetching class
	class JsonFeed : IJsonFeed {

		private const int ResultsLimit = 9;

		private static HttpClient Client;
		static private string Url = "";
		static private int NumberOfresults;
		static private String JsonKey; // key to retrieve specific json values

		/// <summary>
		/// JsonFeed constructor.
		/// </summary>
		/// <param name="endPoint">The url for the website.</param>
		/// <param name="results">The number of times the rest call is to be executed.</param>
		/// <returns></returns>
		public JsonFeed(string endPoint, int numberOfresults, String jsonKey = null) {

			JsonKey = (jsonKey != null) ? jsonKey : null;
			NumberOfresults = (numberOfresults > 1 && numberOfresults <= ResultsLimit) ? numberOfresults : 1;
			Url = endPoint;

			try {

				Client = new HttpClient {

					BaseAddress = new Uri(Url)
				};
			} catch(HttpRequestException e) {

				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// returns the assembled URL as a string.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>The url as a string.</returns>
		public string AssembleURL(string resource, Dictionary<string, string> args) {

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
		/// returns the a list of json values in a list.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>A list of json encoded strings</returns>
		public List<string> GetJSONValuesList(string resource = "", Dictionary<string, string> args = null) {

			List<String> jsonList;
			String url;

			if(args != null && args.Count > 0)
				url = AssembleURL(resource, args);
			else
				url = resource;

			if(JsonKey == null)
				jsonList = ProcessNoJsonKey(url);
			else
				jsonList = ProcessWithJsonkKey(url);

			return jsonList;
		}

		/// <summary>
		/// Processes a json string with no keys
		/// </summary>
		/// <param name="url">The rest api url to retrieve the json string.</param>
		/// <returns>A list of strings containing a single json record.</returns>
		public List<string> ProcessNoJsonKey(string url) {

			List<String> returnList = new List<string>();

			try {

				String json = Task.FromResult(Client.GetStringAsync(url).Result).Result;

				returnList = JsonConvert.DeserializeObject<List<String>>(json);
			} catch(Exception e) {

				Console.WriteLine(e);
			}

			return returnList;			
		}

		/// <summary>
		/// Processes a json string containing keys.
		/// </summary>
		/// <param name="url">The rest api rul to retreive the json string.</param>
		/// <returns>A list of strings containing one or more json records</returns>
		public List<string> ProcessWithJsonkKey(string url) {

			List<String> results = null;
			String result;

			// retrieve results from the specified json key
			for(int i = 0; i < NumberOfresults; i++) {
				
				try {

					String json = Task.FromResult(Client.GetStringAsync(url).Result).Result;

					if(results == null) {

						results = new List<String>();
						result = JsonConvert.DeserializeObject<dynamic>(json)[JsonKey];
						results.Add(result);
					} else {

						result = JsonConvert.DeserializeObject<dynamic>(json)[JsonKey];
						results.Add(result);
					}
				} catch(Exception e) {

					Console.WriteLine(e);
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
	}
}