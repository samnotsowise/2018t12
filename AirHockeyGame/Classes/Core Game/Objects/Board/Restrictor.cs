using Microsoft.Xna.Framework;

namespace FarseerGames.AirHockeyGame {

    /// <summary>
    /// A Restrictor object
    /// Extends GameObject
    /// </summary>
    class Restrictor: GameObject {

        private object[] minions;
        private string[] minionDesc;

        /// <summary>
        /// Constructor - used to create a new Restrictor object
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="iP">Initial Position</param>
        public Restrictor(int w, int h, Vector2 iP) {
            this.minions = new object[0];
            this.minionDesc = new string[0];
            this.width = w;
            this.height = h;
            this.midWidth = w / 2;
            this.midHeight = h / 2;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
            this.initialPosition = iP;
            this.UpdatePosition(initialPosition);
        }

        public void Update() {
            for(int i = 0; i < this.minions.Length; i++) {

                //PaddleObject tobj = (PaddleObject)this.minions[i];
                Puck tobj = (Puck)this.minions[i];
                //if(this.minionDesc[i] == "puck") {
                    
                //}
                if(this.rect.Contains((int)tobj.body.Position.X, (int)tobj.body.Position.Y) == false) {
                    tobj.UpdatePosition(tobj.prevPos);
                }
            }
        }

        public void AddToRestrictor(object o, string d) {

            int csize = this.minions.Length;
            object[] tminions = new object[csize];
            string[] tminonDesc = new string[csize];

            this.minions = new object[csize + 1];
            this.minionDesc = new string[csize + 1];

            for(int i = 0; i < csize; i++) {
                this.minions[i] = tminions[i];
            }
            this.minionDesc[csize] = d;
            if(d == "puck") {
                this.minions[csize] = (Puck) o;
            }
        }
    }
}