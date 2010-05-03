/*
 *      ScoreBox Class
 * 
 * Description:
 *      Manages the scoreboard
 *      
 * Author(s):
 *      David Valente
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {
    class ScoreBox {

        private const float posX = 410.0f;//horizontal position of scoreBox
        private const float posY = 10.0f;//vertical position of scorebox
        private Vector2 scoreBoxPos;
        private Vector2 thisScoreTextPos;
        private Vector2 opponentScoreTextPos;

        public Vector2 OpponentScorePosition {
            get { return opponentScoreTextPos; }
        }

        public Vector2 ScoreTextPosition {
            get { return thisScoreTextPos; }
        }

        private Texture2D scoreOverlay;
        private SpriteFont font;

        public int thisScore;       //score of this player
        public int opponentScore;   //score of the opponent

        /// <summary>
        /// Creates a scoreboard to show the current points of both players.
        /// </summary>
        public ScoreBox() {
            //Set positions of the overlay and text
            scoreBoxPos = new Vector2(posX, posY);
            thisScoreTextPos = new Vector2(scoreBoxPos.X + 20, scoreBoxPos.Y + 10);
            opponentScoreTextPos = new Vector2(scoreBoxPos.X + 120, scoreBoxPos.Y + 10);

            //Initialise scores
            thisScore = 0;
            opponentScore = 0;
        }

        /// <summary>
        /// Loads the overlay and the font.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            scoreOverlay = content.Load<Texture2D>(@"Content\Core Game\score_overlay");
            font = content.Load<SpriteFont>(@"Content\Fonts\gamefont");
        }

        /// <summary>
        /// When the player scores a point, the stored score increases.
        /// </summary>
        public void Scored() {
            thisScore++;
        }

        /// <summary>
        /// When the opponent scores a point, the stored score increases.
        /// </summary>
        public void OpponentScored() {
            opponentScore++;
        }

        /// <summary>
        /// Draws the scorebox with the points overlaid.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(scoreOverlay, scoreBoxPos, Color.White);

            spriteBatch.DrawString(font, "" + thisScore, thisScoreTextPos, Color.Black);
            spriteBatch.DrawString(font, "" + opponentScore, opponentScoreTextPos, Color.Black);
        }
    }
}
