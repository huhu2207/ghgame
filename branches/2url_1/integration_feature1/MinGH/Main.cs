using System;
namespace MinGH
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MinGH game = new MinGH())
            {
                game.Run();
            }
        }
        private static void doStuff()
        {
            //TODO: Do stuff here
        }
    }
}

