/* Holds various variables that all other classes can access.
 * Doesn't require you to copy or pass it as a parameter.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameScreenManager.ScreenSystem;//for Settings class

namespace AirHockeyGame
{
    public static class GameState
    {
        public static int playerScore;
        public static int opponentScore;
        public static Settings gameSettings;
        public static Profile playerProfile;
        public static Texture2D[] profilePictures;

        public static void Initialise()
        {
            playerScore = 0;
            opponentScore = 0;
        }

        public static void LoadContent(ContentManager content)
        {
            profilePictures = new Texture2D[9];

            for (int i = 0; i < 9; i++)
            {
                profilePictures[i] = content.Load<Texture2D>(@"Content\ProfilePics\profilepic" + i);    
            }
        }



    }
}
