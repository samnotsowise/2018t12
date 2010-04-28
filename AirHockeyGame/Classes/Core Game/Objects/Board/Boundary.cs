using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Boundary object
    /// Extends PhysicsObject
    /// </summary>
    class Boundary: PhysicsObject {

        /// <summary>
        /// Constructor - used to create a new Boundary object
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>
        public Boundary(int w, int h, Vector2 iP, PhysicsSimulator pS) {

            //GameObject properties
            this.width = w;
            this.height = h;
            this.midWidth = w / 2;
            this.midHeight = h / 2;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            this.initialPosition = iP;

            //-------------------
            //Physics Properties

            //Create a physics body
            this.body = BodyFactory.Instance.CreateRectangleBody(pS, this.width, this.height, (float)1);

            //Body can't move
            this.body.IsStatic = true;

            //Create a Geom
            Geom rectGeom = GeomFactory.Instance.CreateRectangleGeom(this.body, this.width, this.height);

            //Bounciness
            rectGeom.RestitutionCoefficient = (float)0.6;

            //Friction with objects
            rectGeom.FrictionCoefficient = .5f;

            //Collision handling group
            //rectGeom.CollisionGroup = 100;

            //Add Geom to PhysicsSimulator
            pS.Add(rectGeom);

            //-------------------

            this.UpdatePosition(initialPosition);
        }
    }
}