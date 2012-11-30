using System;
namespace MinGH
{
    static class Program
    {
CHANGE MADE AFTER RC1 WAS MADE        /// <summary>
        /// The main entry point for the application.
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

