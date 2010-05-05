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

using GameScreenManager.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AirHockeyGame {

    public static class GameState {

        public static int playerScore, opponentScore;
        public static Settings gameSettings;
        public static Profile playerProfile, opponentProfile;
        public static Vector2 puckPos;
        public static bool gotOpponent, paused = false;
        public static Texture2D[] profilePictures;

        public static void Initialise() {

            playerScore = 0;
            opponentScore = 0;

        }

        public static void LoadContent(ContentManager content) {
            profilePictures = new Texture2D[9];

            for(int i = 0; i < 9; i++) {
                profilePictures[i] = content.Load<Texture2D>(@"Content\ProfilePics\profilepic" + i);
            }
        }

    }
}
