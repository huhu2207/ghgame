using System;
namespace MinGH
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Another change in the missed feature branch!!!@
        /// </summary>
        static void Main(string[] args)
        {
            using (MinGH game = new MinGH())
            {
                game.Run();
            }
        }
    }
}

