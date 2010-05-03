/*
 *      GameObject Class
 * 
 * Description:
 *      Abstract class for creating game objects
 *      
 * Author(s):
 *      Sam Thompson
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// An abstract GameObject object
    /// </summary>
    public abstract class GameObject {

        public Texture2D texture = null;

        //Rectangle
        public Rectangle rect = new Rectangle(0, 0, 0, 0);

        //Width & Height
        public int width, height = 0;
        public float midWidth, midHeight = 0;

        //Position
        public Vector2 initialPosition, position = new Vector2(0, 0);

        /// <summary>
        /// Constructor - used to extend GameObject
        /// </summary>
        public GameObject() {
        }

        /// <summary>
        /// Updates the GameObject's postion
        /// </summary>
        /// <param name="p">New Position</param>
        public virtual void UpdatePosition(Vector2 p) {
            this.position.X = p.X;
            this.position.Y = p.Y;
            this.rect.X = (int)this.position.X;
            this.rect.Y = (int)this.position.Y;
        }

    }
}