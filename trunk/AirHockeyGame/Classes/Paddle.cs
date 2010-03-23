using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A paddle object
    /// Extends GameObject
    /// </summary>

    class Paddle: GameObject {

        /// <summary>
        /// Constructor - used to create a new Paddle object
        /// </summary>
        /// <param name="t">Texture</param>
        /// <param name="c">Circumference</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>

        public Paddle(Texture2D t, int c, Vector2 iP, PhysicsSimulator pS) {
            this.texture = t;
            this.width = c;
            this.height = c;
            this.midPoint = c / 2;
            this.rect.Width = this.width;
            this.rect.Height = this.height;
            this.body = BodyFactory.Instance.CreateCircleBody(pS, this.midPoint, (float)0.5);
            this.initialPosition = iP;
            this.UpdatePosition(initialPosition);
            this.ResetMouse();
        }

        /// <summary>
        /// Updates the paddle
        /// </summary>

        public override void Update() {
            this.UpdatePosition(this.body.Position);
        }

        /// <summary>
        /// Resets the mouse to the center of the paddle
        /// </summary>

        public void ResetMouse() {
            Mouse.SetPosition((int)this.body.Position.X, (int)this.body.Position.Y);
        }

        /// <summary>
        /// Applies a force to the object body
        /// </summary>
        /// <param name="f">Force to be applied</param>

        public void ApplyForce(Vector2 f) {
            this.body.ApplyForce(f);
        }
    }
}