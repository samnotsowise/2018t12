using System;
using System.Collections.Generic;
using System.Text;

using GameScreenManager.ScreenSystem;//for Settings class

namespace AirHockeyGame
{
    public static class GameState
    {

        public static int playerScore;
        public static int opponentScore;
        public static Settings gameSettings;
        public static Profile playerProfile;

        public static void Initialise()
        {
            playerScore = 0;
            opponentScore = 0;
        }

    }
}
