
namespace MinGH.ChartImpl
{
    class ProperStringCreator
    {
        // Creates a proper string from a split array (used with the name and artist like cases)
        public string createProperString(string[] input)
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
