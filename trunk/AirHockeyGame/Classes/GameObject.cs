using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A paddle object
    /// </summary>

    public abstract class GameObject {

        public Body body;
        public Texture2D texture;
        public Rectangle rect;
        public int width, height;
        public Vector2 position, initialPosition;
        public float midPoint;

        /// <summary>
        /// Constructor - used to extend GameObject
        /// </summary>

        public GameObject() {
            this.texture = null;
            this.width = 0;
            this.height = 0;
            this.midPoint = 0;
            this.rect.Width = 0;
            this.rect.Height = 0;
            this.body = null;
            this.initialPosition = new Vector2(0, 0);
        }

        /// <summary>
        /// Updates the GameObject
        /// </summary>

        public virtual void Update() { }

        /// <summary>
        /// Updates the GameObject's postion
        /// </summary>
        /// <param name="p">New Position</param>

        public void UpdatePosition(Vector2 p) {
            this.position.X = (int)p.X - this.midPoint;
            this.position.Y = (int)p.Y - this.midPoint;
            this.body.Position = p;
            this.rect.X = (int)this.position.X;
            this.rect.Y = (int)this.position.Y;
        }

    }
}