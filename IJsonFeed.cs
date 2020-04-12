using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace JokeGenerator {
	interface IJsonFeed {

		/// <summary>
		/// returns the a list of json values in a list.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>A list of json encoded strings</returns>
		abstract List<String> GetJSONValuesList(String resource = "", Dictionary<String, String> args = null);



		/// <summary>
		/// returns the assembled URL as a string.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>The url as a string.</returns>
		abstract String AssembleURL(String resource, Dictionary<String, String> args);



		/// <summary>
		/// Process a single json record
		/// </summary>
		/// <param name="url">The rest api url to retrieve the json string.</param>
		/// <returns>A list of strings containing a single json record.</returns>
		abstract List<String> ProcessNoJsonKey(String url);


		/// <summary>
		/// Processes specific keys from json records and returns the results as a list of strings
		/// </summary>
		/// <param name="url">The rest api rul to retreive the json string.</param>
		/// <returns>A list of strings containing one or more json records</returns>
		abstract List<String> ProcessWithJsonkKey(String url);


	}
}
