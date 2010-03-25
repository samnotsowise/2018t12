using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameScreenManager.Components
{
    class ScoreBox
    {

        private const float posX = 500.0f;//horizontal position of scoreBox
        private const float posY = 100.0f;//vertical position of scorebox
        private Vector2 scoreBoxPos;
        private Vector2 thisScoreTextPos;
        private Vector2 opponentScoreTextPos;

        private Texture2D scoreOverlay;
        private SpriteFont font;

        public int thisScore;       //score of this player
        public int opponentScore;   //score of the opponent

        /// <summary>
        /// Creates a scoreboard to show the current points of both players.
        /// </summary>
        public ScoreBox()
        {
            //Set positions of the overlay and text
            scoreBoxPos             = new Vector2(500, 500);
            thisScoreTextPos        = new Vector2(scoreBoxPos.X + 50, scoreBoxPos.Y);
            opponentScoreTextPos    = new Vector2(scoreBoxPos.X + 200, scoreBoxPos.Y);
            
            //Initialise scores
            thisScore = 0;
            opponentScore = 0;
        }

        /// <summary>
        /// Loads the overlay and the font.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            scoreOverlay = content.Load<Texture2D>(@"Images\GameScreen\score_overlay");
            font = content.Load<SpriteFont>(@"gamefont");
        }

        /// <summary>
        /// When the player scores a point, the stored score increases.
        /// </summary>
        public void Scored()
        {
            thisScore++;
        }

        /// <summary>
        /// When the opponent scores a point, the stored score increases.
        /// </summary>
        public void OpponentScored()
        {
            opponentScore++;
        }
        
        /// <summary>
        /// Draws the scorebox with the points overlaid.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(scoreOverlay, scoreBoxPos, Color.White);

            spriteBatch.DrawString(font, ""+thisScore, thisScoreTextPos, Color.Black);
            spriteBatch.DrawString(font, ""+opponentScore, opponentScoreTextPos, Color.Black);
        }
    }
}
