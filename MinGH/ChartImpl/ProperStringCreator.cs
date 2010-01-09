
namespace MinGH.ChartImpl
{
	/// <remarks>
	/// Creates a single string from a specificly made string array.
	/// </remarks>
    class ProperStringCreator
    {
        /// <summary>
        /// Takes in a string array and creates a trimmed and concatenated string.
        /// </summary>
        /// <param name="input">
        /// A string array of the form ('|' represents an array element break and quotes on the outside CAN be included):
        /// |"Hello|this|is|my|input"|
        /// </param>
        /// <returns>
        /// A fully trimmed and concatenated string (i.e. Hello this is my input)
        /// </returns>
        public static string createProperString(string[] input)
        {
            string result = "";  // Create the return string

            // Concenate the result string together
            for (int i = 0; i < input.Length; i++)
            {
                result = result + " " + input[i];
            }

            // Remove any leading or following quotes and return
            result = result.Trim(' ').Trim('"');
            return result;
        }
    }
}
