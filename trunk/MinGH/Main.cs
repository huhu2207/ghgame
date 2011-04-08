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
                game.IsFixedTimeStep = false;
                //game.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 20);
                game.Run();
            }
        }
    }
}

