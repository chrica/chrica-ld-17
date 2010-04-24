using System;

namespace LevelOne
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LevelOne game = new LevelOne())
            {
                game.Run();
            }
        }
    }
#endif
}

