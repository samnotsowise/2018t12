using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// An abstract GameObject object
    /// </summary>

    public abstract class GameObject {

        public Texture2D texture = null;

        //Rectangle
        private Rectangle pRect = new Rectangle(0, 0, 0, 0);
        public Rectangle rect {
            get {
                return this.pRect;
            }
        }

        //Width & Height
        private int pWidth, pHeight = 0;
        private float pMidWidth, pMidHeight = 0;
        /// <summary>
        /// Readonly int
        /// </summary>
        public int width {
            get {
                return this.pWidth;
            }
        }
        /// <summary>
        /// Readonly int
        /// </summary>
        public int height {
            get {
                return this.pHeight;
            }
        }
        /// <summary>
        /// Readonly float
        /// </summary>
        public float midWidth {
            get {
                return this.pMidWidth;
            }
        }
        /// <summary>
        /// Readonly float
        /// </summary>
        public float midHeight {
            get {
                return this.pMidHeight;
            }
        }

        //Position
        private Vector2 pInitialPosition = new Vector2(0, 0);
        /// <summary>
        /// Readonly Vector2
        /// </summary>
        public Vector2 initialPosition {
            get {
                return this.pInitialPosition;
            }
        }
        public Vector2 position = new Vector2(0, 0);

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
            this.position.X = (int)p.X - this.width / 2;
            this.position.Y = (int)p.Y - this.height / 2;
            this.pRect.X = (int)this.position.X;
            this.pRect.Y = (int)this.position.Y;
        }

        /// <summary>
        /// Set rect property
        /// </summary>
        /// <param name="r">Rectangle</param>
        public void SetRect(Rectangle r) {
            this.pRect = r;
        }

        /// <summary>
        /// Set width property
        /// </summary>
        /// <param name="w">Width</param>
        public void SetWidth(int w) {
            this.pWidth = w;
        }

        /// <summary>
        /// Set height property
        /// </summary>
        /// <param name="h">Height</param>
        public void SetHeight(int h) {
            this.pHeight = h;
        }

        /// <summary>
        /// Set midWidth property
        /// </summary>
        /// <param name="mW">midWidth</param>
        public void SetMidWidth(int mW) {
            this.pMidWidth = mW;
        }

        /// <summary>
        /// Set midHeight property
        /// </summary>
        /// <param name="mH">midHeight</param>
        public void SetMidHeight(int mH) {
            this.pMidHeight = mH;
        }

        /// <summary>
        /// Set initialPosition property
        /// </summary>
        /// <param name="iP">initialPosition</param>
        public void SetInitialPosition(Vector2 iP) {
            this.pInitialPosition = iP;
        }

    }
}