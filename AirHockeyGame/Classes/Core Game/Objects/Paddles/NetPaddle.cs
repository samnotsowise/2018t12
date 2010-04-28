using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A NetPaddle object
    /// Extends PaddleObject
    /// </summary>
    class NetPaddle: PaddleObject {

        /// <summary>
        /// Constructor - used to create a new NetPaddle object
        /// </summary>
        /// <param name="t">Texture</param>
        /// <param name="c">Circumference</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>
        public NetPaddle(Texture2D t, int c, Vector2 iP, PhysicsSimulator pS) {

            //GameObject properties
            this.texture = t;
            this.width = c;
            this.height = c;
            this.midWidth = c / 2;
            this.midHeight = this.midWidth;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            this.initialPosition = iP;

            //Create a physics body
            this.CreateBody(pS);

            //Set the object to it's initial position
            this.UpdatePosition(this.initialPosition);
        }

        /// <summary>
        /// Updates the NetPaddle
        /// </summary>
        public override void Update() {

            //NetUpdate code goes in here

            base.Update();
        }
    }
}