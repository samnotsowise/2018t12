using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Restrictor object
    /// Extends GameObject
    /// </summary>

    class Restrictor: GameObject {

        /// <summary>
        /// Constructor - used to create a new Restrictor object
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="iP">Initial Position</param>

        public Restrictor(int w, int h, Vector2 iP) {
            this.SetWidth(w);
            this.SetHeight(h);
            this.SetMidWidth((int)(w / 2));
            this.SetMidHeight((int)(h / 2));
            this.SetRect(new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height));
            this.SetInitialPosition(iP);
            this.UpdatePosition(initialPosition);
        }
    }
}