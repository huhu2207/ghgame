using System.Text;

namespace MinGH.GameScreen.SongSelection
{
    /// <summary>
    /// Simply makes a presentable string for the song selection menu.
    /// </summary>
    public static class SongSelectionEntryCreator
    {
        /// <summary>
        /// Simply makes a presentable string for the song selection menu.
        /// </summary>
        /// <param name="path">
        /// The path to the file in question (not including the file itself).
        /// </param>
        /// <returns>A nicely formatted string.</returns>
        public static string CreateProperSongSelectionEntry(string path)
        {
            string[] splitPath = path.Split('\\');
            StringBuilder properString = new StringBuilder();
            for (int i = 1; i < splitPath.GetLength(0); i++)
            {
                properString.Append(splitPath[i]);
                if (i < splitPath.GetLength(0) - 1)
                {
                    properString.Append(" - ");
                }
            }

            return properString.ToString();
        }
    }
}
