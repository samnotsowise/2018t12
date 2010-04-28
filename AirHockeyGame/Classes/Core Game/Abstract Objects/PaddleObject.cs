using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A PaddleObject object
    /// Extends PhysicsObject
    /// </summary>
    public abstract class PaddleObject: PhysicsObject {

        public Vector2 force = new Vector2(0, 0);

        //Previous Location
        public Vector2 prevPos {
            set {
                this.prevPos = value;
            }
            get {
                return this.prevPos;
            }
        }

        /// <summary>
        /// Constructor - used to create a new PaddleObject object
        /// </summary>
        public PaddleObject() {
            this.prevPos = this.initialPosition;
        }

        /// <summary>
        /// Updates the PaddleObject
        /// </summary>
        public virtual void Update() {

            //Keep track of where we've been
            //this.SetPrevPos(this.body.Position);

            //Apply force
            this.body.ApplyForce(this.force);

            //Update the object's position
            this.UpdatePosition(this.body.Position);

            //Reset applied force
            this.force.X = 0;
            this.force.Y = 0;
        }

        public void CreateBody(PhysicsSimulator pS) {

            //Create a physics body
            this.body = BodyFactory.Instance.CreateCircleBody(pS, this.midWidth, (float)3);

            //Create a Geom
            Geom circleGeom = GeomFactory.Instance.CreateCircleGeom(this.body, (int)this.midWidth, (int)this.width);
            
            //Bounciness
            circleGeom.RestitutionCoefficient = 0;

            //Friction in Air
            this.body.LinearDragCoefficient = 10;

            //Add Geom to PhysicsSimulator
            pS.Add(circleGeom);
        }
    }
}