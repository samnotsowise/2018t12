using AirHockeyGame;

namespace FarseerGames.AirHockeyGame {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main() {
            using(AirHockey game = new AirHockey()) {
                game.Run();
            }
        }
    }
}