using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            this.width = w;
            this.height = h;
            this.rect.Width = this.width;
            this.rect.Height = this.height;
            this.initialPosition = iP;
            this.UpdatePosition(initialPosition);
        }

        /// <summary>
        /// Updates the Restrictor
        /// </summary>

    }
}