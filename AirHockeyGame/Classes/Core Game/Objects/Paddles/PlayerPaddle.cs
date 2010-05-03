/*
 *      PlayerPaddle Class
 * 
 * Description:
 *      A player paddle is controlled by
 *      the user's mouse
 *      
 * Author(s):
 *      Sam Thompson
 */

using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A PlayerPaddle object
    /// Extends PaddleObject
    /// </summary>
    class PlayerPaddle: PaddleObject {

        private Vector2 previousMouse, currentMouse;

        /// <summary>
        /// Constructor - used to create a new PlayerPaddle object
        /// </summary>
        /// <param name="t">Texture</param>
        /// <param name="c">Circumference</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>
        public PlayerPaddle(Texture2D t, int c, Vector2 iP, PhysicsSimulator pS) {

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

            //Set the object to it's initial position, along with the mouse pointer
            this.UpdatePosition(this.initialPosition);
            this.ResetMouse();
        }

        /// <summary>
        /// Updates the PlayerPaddle
        /// </summary>
        public override void Update() {

            //Saves multiple calls to GetState later on
            this.currentMouse = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            //Only apply a force if the mouse has moved
            if((this.previousMouse != this.currentMouse) || this.previousMouse == null) {

                //Calculate the mouse's difference in position from the last update
                Vector2 diff = new Vector2(this.currentMouse.X - this.body.Position.X, this.currentMouse.Y - this.body.Position.Y);

                //Set a force accordingly
                this.force.X = diff.X * 360;
                this.force.Y = diff.Y * 360;

                //Move the mouse to the new location
                this.ResetMouse();
            }

            //Remember where the mouse is
            this.previousMouse = this.currentMouse;

            base.Update();
        }

        /// <summary>
        /// Resets the mouse to the center of the PlayerPaddle
        /// </summary>
        public void ResetMouse() {
            Mouse.SetPosition((int)this.body.Position.X, (int)this.body.Position.Y);
        }
    }
}