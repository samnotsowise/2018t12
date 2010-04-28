using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// The Board object
    /// Extends GameObject
    /// </summary>
    class Board: GameObject {

        //Boundaries go clockwise from the top leftmost bound (boundaries[0] = top left boundary)
        private Boundary[] boundaries;
        public Restrictor[] restrictors;


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

            this.boundaries = new Boundary[6];
            this.boundaries[0] = new Boundary(this.width, 39, new Vector2(this.width / 2, (39 / 2) + this.position.Y), pS);
            this.boundaries[1] = new Boundary(45, this.height, new Vector2(1024 - (45 / 2), 768 - (this.height / 2)), pS);
            this.boundaries[2] = new Boundary(this.width, 39, new Vector2(1024 - (this.width / 2), (768 - (39 / 2)) - this.position.Y), pS);
            this.boundaries[3] = new Boundary(45, this.height, new Vector2(45 / 2, this.height / 2), pS);
            
            this.restrictors = new Restrictor[1];
            this.restrictors[0] = new Restrictor(934, 590, new Vector2(46, 92));
        }

        public void Update() {
            for(int i = 0; i < this.restrictors.Length; i++) {
                this.restrictors[i].Update();
            }
        }
    }
}
