/*
 *      PaddleObject Class
 * 
 * Description:
 *      Used to create paddles
 *      
 * Authors:
 *      Sam Thompson
 */

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

        public Vector2 force;
        public Vector2 prevPos, prevPos2;

        /// <summary>
        /// Constructor - used to create a new PaddleObject object
        /// </summary>
        public PaddleObject() {
            this.force = new Vector2(0, 0);
        }

        /// <summary>
        /// Updates the PaddleObject
        /// </summary>
        public virtual void Update() {

            //Record previous positions
            this.prevPos2 = this.prevPos;
            this.prevPos = this.body.Position;

            //Apply force if we are within bounds
            if(this.body.Force.X < 100 && this.body.Force.Y < 100) {
                this.body.ApplyForce(this.force);
            }
            //Update the object's position
            this.UpdatePosition(this.body.Position);

            //Reset applied force
            this.force.X = 0;
            this.force.Y = 0;
        }

        public override void CreateBody(PhysicsSimulator pS) {

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