using System;

namespace AirHockey {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            using(AirHockey game = new AirHockey()) {
                game.Run();
            }
        }
    }
}

