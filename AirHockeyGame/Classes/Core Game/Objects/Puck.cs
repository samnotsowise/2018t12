/*
 *      Puck Class
 * 
 * Description:
 *      Used to create pucks.
 *      Most of this object is
 *      handled by the physics
 *      engine.
 *      
 * Author(s):
 *      Sam Thompson
 */

using AirHockeyGame;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Puck object
    /// Extends PhysicsObject
    /// </summary>
    class Puck: PhysicsObject {

        public Vector2 prevPos, prevPos2;
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
            this.midWidth = c / 2;
            this.midHeight = this.midWidth;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            this.initialPosition = iP;
            this.pS = pS;
            this.CreateBody(this.pS);

            //Set the object to it's initial position
            this.UpdatePosition(this.initialPosition);
        }

        /// <summary>
        /// Updates the Puck
        /// </summary>
        public void Update() {
            if(this.prevPos != this.body.Position) {
                this.prevPos2 = this.prevPos;
            }
            this.prevPos = this.position;
            this.UpdatePosition(this.body.Position);
        }

        /// <summary>
        /// Updates the pucks position and the global record
        /// </summary>
        /// <param name="p">Position</param>
        public override void UpdatePosition(Vector2 p) {
            GameState.puckPos = p;
            base.UpdatePosition(p);
        }

        /// <summary>
        /// Creates a physics body for this paddle
        /// </summary>
        /// <param name="pS">Physics Simulator</param>
        public override void CreateBody(PhysicsSimulator pS) {

            //Create a physics body
            this.body = BodyFactory.Instance.CreateCircleBody(pS, this.midWidth, (float)0.1);

            //Create a Geom
            Geom circleGeom = GeomFactory.Instance.CreateCircleGeom(this.body, (int)this.midWidth, (int)this.width);

            //Collision Group
            circleGeom.CollisionGroup = 101;

            //Bounciness
            circleGeom.RestitutionCoefficient = (float)0.5;

            //Add Geom to PhysicsSimulator
            pS.Add(circleGeom);
        }
    }
}