using System;
using System.Collections.Generic;

namespace JokeGenerator {
	interface IJsonFeed {

		/// <summary>
		/// Abstract method for getting a list of json values.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>A list of json encoded strings</returns>
		abstract List<String> GetJSONValuesList(String resource = "", Dictionary<String, String> args = null);



		/// <summary>
		/// Abstract mehtod for assembling a URL.
		/// </summary>
		/// <param name="resource">The specific restapi resource we're using, defaulted to an empty.</param>
		/// <param name="args">Key values pairs of strings for any arguments that need to be appended to url.</param>
		/// <returns>The url as a string.</returns>
		abstract String AssembleURL(String resource, Dictionary<String, String> args);



		/// <summary>
		/// Abbstract method for processing a json values with no keys.
		/// </summary>
		/// <param name="url">The rest api url to retrieve the json string.</param>
		/// <returns>A list of strings containing a single json record.</returns>
		abstract List<String> ProcessNoJsonKey(String url);


		/// <summary>
		/// Abstract method for processing a json method with keys.
		/// </summary>
		/// <param name="url">The rest api rul to retreive the json string.</param>
		/// <returns>A list of strings containing one or more json records</returns>
		abstract List<String> ProcessWithJsonkKey(String url);


	}
}
