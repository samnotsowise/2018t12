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
 * Authors:
 *      Sam Thompson
 */ 

using System;
using System.Collections.Generic;
using System.Text;

namespace AirHockeyGame {
    public class GameState {

        public int playerScore = 0;
        public int opponentScore = 0;

        public GameState() {
        }
    }
}
