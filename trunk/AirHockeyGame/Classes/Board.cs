using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {
    class Board: GameObject {

        //Boundaries go clockwise from the top leftmost bound (boundaries[0] = top left boundary)
        private Boundary[] boundaries;
        private Restrictor[] restrictors;

        public Board() {
            this.boundaries = new Boundary[6];
            this.restrictors = new Restrictor[2];
            this.width = 1024;
            //INCORRECT - CALCULATE
            this.height = 768;
            this.initialPosition = new Vector2(20, 0);
            this.position = this.initialPosition;
        }

    }
}
