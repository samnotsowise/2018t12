/*
 *      GameState Class
 * 
 * Description:
 *      This class is for use as a global variable
 *      for storing only the variables that are
 *      required by specific classes to operate.
 *      Try to keep it's size to a minimum for the
 *      sake of efficiency
 *      
 * Author(s):
 *      Sam Thompson
 *      David Valente
 */

using GameScreenManager.ScreenSystem;//for Settings class
using Microsoft.Xna.Framework;

namespace AirHockeyGame {

    public static class GameState {

        public static int playerScore, opponentScore;
        public static Settings gameSettings;
        public static Profile playerProfile, opponentProfile;
        public static Vector2 puckPos;
        public static bool gotOpponent, paused = false;

        public static void Initialise() {

            playerScore = 0;
            opponentScore = 0;

        }

    }
}
