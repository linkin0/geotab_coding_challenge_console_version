using System;

namespace JokeGenerator {
    // The console printer is the output class
    public class ConsolePrinter {
        public static object PrintValue;

        /// <summary>
		/// ConsolePrinter Constructor
		/// </summary>
		/// <param name="value">The value to be printed to the console.</param>
		/// <returns></returns>
        public ConsolePrinter InsertString(String value) {

            PrintValue = value;
            return this;
        }

        /// <summary>
		/// overriden ToString method which prints the given value
		/// </summary>		
        public void Print() {

            Console.WriteLine(PrintValue);            
        }
    }
}
