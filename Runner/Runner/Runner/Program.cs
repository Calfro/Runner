using System;

namespace Runner
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RunnerGame game = new RunnerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

