/*
 *      Detector Class
 * 
 * Description:
 *      A board object simply used to
 *      check for collisions
 *      
 * Author(s):
 *      Sam Thompson
 */

using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Detector object
    /// Extends GameObject
    /// </summary>
    class Detector: GameObject {

        /// <summary>
        /// Constructor - used to create a new Detector object
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="iP">Initial Position</param>
        public Detector(int w, int h, Vector2 iP) {
            this.width = w;
            this.height = h;
            this.midWidth = w / 2;
            this.midHeight = h / 2;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            this.initialPosition = iP;
            this.UpdatePosition(this.initialPosition);
        }
    }
}