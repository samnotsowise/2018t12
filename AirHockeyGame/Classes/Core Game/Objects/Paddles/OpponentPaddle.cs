/*
 *      OpponentPaddle Class
 * 
 * Description:
 *      This class is for the opponent's
 *      paddle control. If there is a
 *      network connection, that is used
 *      to control this paddle. If not, 
 *      this becomes an AI paddle.
 *      
 * Author(s):
 *      Sam Thompson
 */

using AirHockeyGame;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// An OpponentPaddle object
    /// Extends PaddleObject
    /// </summary>
    class OpponentPaddle: PaddleObject {
        
        private int mouseX, mouseY;
        public int MouseX
        {
            get { return mouseX; }
            set { mouseX = value; }
        }

        public int MouseY
        {
            get { return mouseY; }
            set { mouseY = value; }
        }

        /// <summary>
        /// Constructor - used to create a new OpponentPaddle object
        /// </summary>
        /// <param name="t">Texture</param>
        /// <param name="c">Circumference</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>
        public OpponentPaddle(Texture2D t, int c, Vector2 iP, PhysicsSimulator pS) {

            //GameObject properties
            this.texture = t;
            this.width = c;
            this.height = c;
            this.midWidth = c / 2;
            this.midHeight = this.midWidth;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            this.initialPosition = iP;
            this.pS = pS;

            //Create a physics body
            this.CreateBody(pS);

            //Set the object to it's initial position
            this.UpdatePosition(this.initialPosition);
        }

        /// <summary>
        /// Updates the OpponentPaddle
        /// </summary>
        public override void Update() {

            if(GameState.gotOpponent == true) {

                //NetUpdate code goes in here

                #region Network Control

                #endregion

            } else {

                //AI takes over

                #region AI Control

                Vector2 diff = GameState.puckPos - this.body.Position;

                //If the puck is in AI's half, hit it
                if(GameState.puckPos.X > 512) {
                    if(diff.X > 0) {
                        this.force = new Vector2(2000, diff.Y) * (30 * ((int)GameState.gameSettings.difficulty + 1));
                    } else {
                        this.force = new Vector2(diff.X + 48, diff.Y) * (50 * ((int)GameState.gameSettings.difficulty + 1));
                    }

                    //Otherwise, just match it's Y position
                } else {
                    this.force = new Vector2(2000, (diff.Y) * 40);
                }

                #endregion

            }
            base.Update();
        }
    }
}