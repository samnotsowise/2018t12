/*
 *      Board Class
 * 
 * Description:
 *      A collection of boundaries and 
 *      detectors
 *      
 * Author(s):
 *      Sam Thompson
 */

using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// The Board object
    /// Extends GameObject
    /// </summary>
    class Board: GameObject {

        //Boundaries go clockwise from the top leftmost bound (boundaries[0] = top left boundary)
        public Boundary[] boundaries;
        public Detector[] detectors;


        /// <summary>
        /// Constructor - Creates a new Board object
        /// </summary>
        /// <param name="pS"></param>
        public Board(PhysicsSimulator pS) {

            this.width = 1024;
            this.height = 672;
            this.midWidth = this.width / 2;
            this.midHeight = this.height / 2;
            this.initialPosition = new Vector2(0, 54);
            this.position = this.initialPosition;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);

            #region Bounadaries

            //Boundaries
            this.boundaries = new Boundary[5];

            //Top
            this.boundaries[0] = new Boundary(this.width, 92, new Vector2(0, 0), pS);

            //Bottom
            this.boundaries[1] = new Boundary(this.width, 84, new Vector2(0, 684), pS);

            //Left
            this.boundaries[2] = new Boundary(45, 592, new Vector2(0, 92), pS);

            //Right
            this.boundaries[3] = new Boundary(45, 592, new Vector2(979, 92), pS);

            //MidLine
            this.boundaries[4] = new Boundary(6, 768, new Vector2(509, 0), pS, 101);

            #endregion

            #region Detectors

            //Detectors
            this.detectors = new Detector[2];

            //Player Goal
            this.detectors[0] = new Detector(1, 231, new Vector2(978, 280));

            //Opponent Goal
            this.detectors[1] = new Detector(1, 231, new Vector2(46, 280));

            #endregion

        }
    }
}
