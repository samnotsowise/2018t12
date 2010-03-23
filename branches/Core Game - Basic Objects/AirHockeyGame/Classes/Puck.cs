using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Puck object
    /// Extends GameObject
    /// </summary>

    class Puck: GameObject {

        /// <summary>
        /// Constructor - used to create a new Puck object
        /// </summary>
        /// <param name="t">Texture</param>
        /// <param name="c">Circumference</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>

        public Puck(Texture2D t, int c, Vector2 iP, PhysicsSimulator pS) {
            this.texture = t;
            this.width = c;
            this.height = c;
            this.midPoint = c / 2;
            this.rect.Width = this.width;
            this.rect.Height = this.height;
            this.body = BodyFactory.Instance.CreateCircleBody(pS, this.midPoint, (float)0.5);
            this.initialPosition = iP;
            this.UpdatePosition(initialPosition);
        }

        /// <summary>
        /// Updates the paddle
        /// </summary>

        public override void Update() {
            this.UpdatePosition(this.body.Position);
            base.Update();
        }
    }
}