using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;

namespace FarseerGames.AirHockeyGame {
    class Board: GameObject {

        //Boundaries go clockwise from the top leftmost bound (boundaries[0] = top left boundary)
        private Boundary[] boundaries;
        public Restrictor[] restrictors;

        public Board(PhysicsSimulator pS) {
            this.SetWidth(1024);
            this.SetHeight(672);
            this.SetMidWidth((int)(this.width / 2));
            this.SetMidHeight((int)(this.height / 2));
            this.SetInitialPosition(new Vector2(0, 48));
            this.position = this.initialPosition;
            this.SetRect(new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height));

            this.boundaries = new Boundary[6];
            this.boundaries[0] = new Boundary(this.width, 39, new Vector2(this.width / 2, (39 / 2) + this.position.Y), pS);
            this.boundaries[1] = new Boundary(45, this.height, new Vector2(1024 - (45 / 2), 768 - (this.height / 2)), pS);
            this.boundaries[2] = new Boundary(this.width, 39, new Vector2(1024 - (this.width / 2), (768 - (39 / 2)) - this.position.Y), pS);
            this.boundaries[3] = new Boundary(45, this.height, new Vector2(45 / 2, this.height / 2), pS);
            
            this.restrictors = new Restrictor[2];
            this.restrictors[0] = new Restrictor(this.width, this.height, new Vector2(0, 48));
        }
    }
}
