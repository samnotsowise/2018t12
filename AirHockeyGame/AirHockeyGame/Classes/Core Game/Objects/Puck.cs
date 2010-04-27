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

        private Restrictor restrictor;

        /// <summary>
        /// Constructor - used to create a new Puck object
        /// </summary>
        /// <param name="t">Texture</param>
        /// <param name="c">Circumference</param>
        /// <param name="iP">Initial Position</param>
        /// <param name="pS">Physics Simulator</param>
        public Puck(Texture2D t, int c, Vector2 iP, PhysicsSimulator pS, Restrictor r) {
            this.texture = t;
            this.SetWidth(c);
            this.SetHeight(c);
            this.SetMidWidth(c / 2);
            this.SetMidHeight(this.midWidth);
            this.SetRect(new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height));
            this.SetInitialPosition(iP);
            this.restrictor = r;

            //-------------------
            //Physics Properties

            //Create a physics body
            this.body = BodyFactory.Instance.CreateCircleBody(pS, this.midWidth, (float)0.1);

            //Create a Geom
            Geom circleGeom = GeomFactory.Instance.CreateCircleGeom(this.body, (int)this.midWidth, (int)this.width);

            //Bounciness
            circleGeom.RestitutionCoefficient = (float)0.5;

            //Add Geom to PhysicsSimulator
            pS.Add(circleGeom);

            //-------------------

            //Set the object to it's initial position
            this.UpdatePosition(this.initialPosition);
        }

        /// <summary>
        /// Updates the Puck
        /// </summary>
        public void Update() {
            this.UpdatePosition(this.body.Position);
        }
    }
}