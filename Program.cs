using System;
using System.Collections.Generic;

namespace JokeGenerator{
    class Program {

        private const String StartingMessage = "Press the '?' to get instructions or 'X' to end the program. Then hit the enter key.";
        private const String GetCategoryMessage = "Press 'C' to get the categories";
        private const String GetRandomJokeMessage = "Press 'R' to get a random joke(s).";
        private const String SpecifyACategoryMessage = "\nWant to specify a category? (Y/N)";
        private const String PickACategoryMessage = "\nEnter a category and then hit enter.";
        private const String PickARadomNameMessage = "\nDo you want to use a random name? (Y/N)";
        private const String NumberOfJokesMessage = "\nHow many jokes do you want? (1-{0}), then hit enter.";
        private const String InvalidInputMessage = "\nInvalid input.";

        private const String ChuckNorrisRandomJokesResource = "jokes/random";
        private const String ChuckNorrisCategoryResource = "jokes/categories";

        private const String ChuckNorisJokesURL = "https://api.chucknorris.io";
        private const String RandomNamesSiteURL = "https://namey.muffinlabs.com/name.json?with_surname=true";

        static private char key;    // contains the key grabbed from the console
        static private String[] results = new String[50]; // receive the results to out put to the screen
        static private String name; // will contain the random name produced

        private static readonly ConsolePrinter printer = new ConsolePrinter(); // instance of the output class
        private static IJsonFeed _jsonFeed; // handle to JsonFeed class via IJsonFeed interface

        static void Main() {

            ChuckJokerMenu();
        }

        /// <summary>
		/// Print the formatted results of known categories or Chuck Norris jokes.
		/// </summary>
		/// 
        private static void PrintResults() {
            int count = 1;

            Console.WriteLine("\n");
            foreach(String item in results) {
                
                printer.InsertString(count.ToString() + ". " + string.Join(",", item)).Print();
                count++;
            }

        }

        /// <summary>
		/// Grab user key board input and assign a value to key from the consoleKeyInfo.
		/// </summary>
		///
		/// <returns>Returns  a string version of the user input if it is valid for the application.</returns>
        private static String GetEnteredKey() {

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

            switch(consoleKeyInfo.Key) {
                case ConsoleKey.C:
                    key = 'c';
                    break;
                case ConsoleKey.D0:
                    key = '0';
                    break;
                case ConsoleKey.D1:
                    key = '1';
                    break;
                case ConsoleKey.D3:
                    key = '3';
                    break;
                case ConsoleKey.D4:
                    key = '4';
                    break;
                case ConsoleKey.D5:
                    key = '5';
                    break;
                case ConsoleKey.D6:
                    key = '6';
                    break;
                case ConsoleKey.D7:
                    key = '7';
                    break;
                case ConsoleKey.D8:
                    key = '8';
                    break;
                case ConsoleKey.D9:
                    key = '9';
                    break;
                case ConsoleKey.R:
                    key = 'r';
                    break;
                case ConsoleKey.Y:
                    key = 'y';
                    break;
                case ConsoleKey.N:
                    key = 'n';
                    break;
                case ConsoleKey.X:
                    key = 'x';
                    break;
                default:
                    key = '#';
                    break;
            }

            return key.ToString();
        }

        /// <summary>
		/// Get a random joke(s) wrapper method, retrieves chuck norris jokes.
		/// </summary>
		/// <param name="category">The category of Chuck Norris jokes we want.</param>
        /// <param name="number">The number of Chuck Norris jokes we want.</param>		
        private static void GetRandomJokes(string category, int numberOfResults) {

            var args = new Dictionary<String, String>();

            if(category != null)
                if( GetCategories().Contains(category))
                    args.Add("category", category);

            if(name != null)
                args.Add("name", name);

            if(category == null && name == null)
                args = null;

            _jsonFeed = new JsonFeed(ChuckNorisJokesURL, numberOfResults, "value");
            results = _jsonFeed.GetJSONValuesList(ChuckNorrisRandomJokesResource, args).ToArray();
        }

        /// <summary>
		/// Get the categories wrapper method, retrieves the categories of Chuck Norris jokes.
		/// </summary>
        /// <returns>Returns a string list of categories for Chuck Norris jokes.</returns>
        private static List<String> GetCategories() {           

            _jsonFeed = new JsonFeed(ChuckNorisJokesURL, 0);
            List<String> categories = _jsonFeed.GetJSONValuesList(ChuckNorrisCategoryResource);
            results = categories.ToArray();

            return categories;
        }

        /// <summary>
		/// Get the name wrapper method, retrieve a random name to replace Chuck Norris with.
		/// </summary>		
        private static void GetNames() {
            
            _jsonFeed = new JsonFeed(RandomNamesSiteURL, 0);            
            
            name = _jsonFeed.GetJSONValuesList()[0];
        }

        /// <summary>s
        /// The menu for the Chuck Norris application.
        /// </summary>
        private static void ChuckJokerMenu() {
            printer.InsertString(StartingMessage).Print();

            if(Console.ReadLine() == "?") {

                while(key != 'x') {

                    printer.InsertString(GetCategoryMessage).Print();
                    printer.InsertString(GetRandomJokeMessage).Print();
                    getValidUserInput(new List<char>{ 'c', 'r', 'x'});

                    if(key == 'c') {

                        GetCategories();
                        PrintResults();
                    }
                    if(key == 'r') {

                        printer.InsertString(PickARadomNameMessage).Print();
                        getValidUserInput(new List<char> { 'y', 'n'});

                        if(key == 'y') {

                            GetNames();
                        }                        

                        printer.InsertString(SpecifyACategoryMessage).Print();
                        getValidUserInput(new List<char> { 'y', 'n'});

                        if(key == 'y') {

                            printer.InsertString(PickACategoryMessage).Print();
                            String category = Console.ReadLine();

                            printer.InsertString(String.Format(NumberOfJokesMessage, JsonFeed.GetResultLimit())).Print();
                            int n = getValidRange(1, JsonFeed.GetResultLimit());

                            GetRandomJokes(category, n);

                            PrintResults();
                        } else {

                            printer.InsertString(String.Format(NumberOfJokesMessage, JsonFeed.GetResultLimit())).Print();

                            int n = getValidRange(1, JsonFeed.GetResultLimit());
                            GetRandomJokes(null, n);

                            PrintResults();
                        }
                    }

                    name = null;
                }
            }
        }

        /// <summary>
        /// Test for situation valid situation character input.
        /// </summary>
        /// <param name="validValues">List of characters containing the valid input.</param>        	
        private static void getValidUserInput(List<char> validValues) {

            bool isValid = false;
            
            while(!isValid) {
                
                GetEnteredKey();
                if(validValues.Contains(key)) {
                    isValid = true;
                }else {
                    Console.WriteLine(InvalidInputMessage);                    
                }
            }
        }

        /// <summary>
        /// Test for situation valid situation character input for numbers.
        /// </summary>
        /// <param name="validValues">List of characters containing the valid input.</param> 
        /// <param name="min">Minimum value of range, it has a default of one.</param>
        /// <param name="max">Maximum value of range, it has a default of nine.</param>
        /// <return>Returns the valid integer.</return>
        private static int getValidRange(int min=1, int max=9) {

             while(true) {

                GetEnteredKey();
               
                if (int.TryParse( key.ToString(), out int val)) {
                    if(val >= min && val <= max) {
                        return val;
                    }
                }
                Console.WriteLine(InvalidInputMessage);              
            }            
        }

    }

}
