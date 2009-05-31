
namespace Chart_View
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Main_Game game = new Main_Game())
            {
                game.Run();
            }
        }
    }
}

