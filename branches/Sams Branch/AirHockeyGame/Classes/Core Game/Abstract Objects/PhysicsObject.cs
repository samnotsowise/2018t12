using FarseerGames.FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// An abstract PhysicsObject object
    /// Extends GameObject
    /// </summary>
    public abstract class PhysicsObject: GameObject {

        public Body body;

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
    }
}