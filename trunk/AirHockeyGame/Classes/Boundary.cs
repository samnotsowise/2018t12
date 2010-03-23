using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Boundary object
    /// Extends GameObject
    /// </summary>

    class Boundary: GameObject {

        /// <summary>
        /// Constructor - used to create a new Boundary object
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>

        public Boundary(int w, int h, Vector2 iP, PhysicsSimulator pS) {
            this.width = w;
            this.height = h;
            this.rect.Width = this.width;
            this.rect.Height = this.height;
            this.body = BodyFactory.Instance.CreateCircleBody(pS, this.midPoint, (float)0.5);
            this.initialPosition = iP;
            this.UpdatePosition(initialPosition);
        }

        /// <summary>
        /// Updates the Boundary
        /// </summary>

        public override void Update() {
            base.Update();
        }
    }
}