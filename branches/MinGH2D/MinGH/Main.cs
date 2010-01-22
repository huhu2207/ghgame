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
            using (MinGHMain game = new MinGHMain())
            {
                game.IsFixedTimeStep = false;
                //game.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 20);
                game.Run();
            }
        }
    }
}

