using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// An abstract PhysicsObject object
    /// Extends GameObject
    /// </summary>
    public abstract class PhysicsObject: GameObject {

        public Body body;
        public PhysicsSimulator pS;
        /// <summary>
        /// Constructor - used to extend PhysicsObject
        /// </summary>
        public PhysicsObject() {
            this.body = null;
        }

        /// <summary>
        /// Updates the PhysicsObject's postion
        /// </summary>
        /// <param name="p">New Position</param>
        public override void UpdatePosition(Vector2 p) {
            this.body.Position = p;
            base.UpdatePosition(new Vector2(p.X - this.midWidth, p.Y - this.midHeight));
        }

        public virtual void reset() {
            this.UpdatePosition(this.initialPosition);
            this.body.Dispose();
            this.CreateBody(this.pS);
            this.UpdatePosition(this.initialPosition);
        }

        public virtual void CreateBody(PhysicsSimulator pS) {
        }
    }
}