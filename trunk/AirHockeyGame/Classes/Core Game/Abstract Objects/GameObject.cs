using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// An abstract GameObject object
    /// </summary>
    public abstract class GameObject {

        public Texture2D texture;

        //Rectangle
        public Rectangle rect {
            set {
                this.rect = value;
            }
            get {
                return this.rect;
            }
        }

        //Width & Height
        /// <summary>
        /// Readonly int
        /// </summary>
        public int width {
            set {
                this.width = value;
            }
            get {
                return this.width;
            }
        }
        /// <summary>
        /// Readonly int
        /// </summary>
        public int height {
            set {
                this.height = value;
            }
            get {
                return this.height;
            }
        }

        /// <summary>
        /// Readonly float
        /// </summary>
        public float midWidth {
            set {
                this.midWidth = value;
            }
            get {
                return this.midWidth;
            }
        }
        /// <summary>
        /// Readonly float
        /// </summary>
        public float midHeight {
            set {
                this.midHeight = value;
            }
            get {
                return this.midHeight;
            }
        }

        //Position
        /// <summary>
        /// Readonly Vector2
        /// </summary>
        public Vector2 initialPosition {
            set {
                this.initialPosition = value;
            }
            get {
                return this.initialPosition;
            }
        }
        public Vector2 position;

        /// <summary>
        /// Constructor - used to extend GameObject
        /// </summary>

        public GameObject() {
            this.width = 0;
            this.height = 0;
            this.midWidth = 0;
            this.midHeight = 0;
            this.rect = new Rectangle(0, 0, 0, 0);
            this.initialPosition = new Vector2(0, 0);
            this.position = this.initialPosition;
            this.texture = null;
        }

        /// <summary>
        /// Updates the GameObject's postion
        /// </summary>
        /// <param name="p">New Position</param>
        public virtual void UpdatePosition(Vector2 p) {
            this.position.X = p.X;
            this.position.Y = p.Y;
            //this.rect.X = (int)this.position.X;
            //this.rect.Y = (int)this.position.Y;
        }
    }
}