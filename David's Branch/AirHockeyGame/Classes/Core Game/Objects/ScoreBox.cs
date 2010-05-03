/*
 * Displays the score and an effect when a goal is scored.
 * 
 * Author:
 *  David Valente
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AirHockeyGame;

namespace FarseerGames.AirHockeyGame
{
    class ScoreBox
    {

        private const float posX = 410.0f;//horizontal position of scoreBox
        private const float posY = 10.0f;//vertical position of scorebox
        private Vector2 scoreBoxPos;
        private Vector2 thisScoreTextPos;
        private Vector2 opponentScoreTextPos;

        //Effect variables
        bool effect;
        float scale;
        string effectText;
        Vector2 effectPos;
        float effectTime;
        const float effectLength = 500.0f;
        Color effectColor;
        
        public Vector2 OpponentScorePosition
        {
            get { return opponentScoreTextPos; }
        }

        public Vector2 ScoreTextPosition
        {
            get { return thisScoreTextPos; }
        }

        private Texture2D scoreOverlay;
        private SpriteFont font;

        /// <summary>
        /// Creates a scoreboard to show the current points of both players.
        /// </summary>
        public ScoreBox()
        {
            //Set positions of the overlay and text
            scoreBoxPos             = new Vector2(posX, posY);
            thisScoreTextPos        = new Vector2(scoreBoxPos.X + 20, scoreBoxPos.Y+10);
            opponentScoreTextPos    = new Vector2(scoreBoxPos.X + 120, scoreBoxPos.Y+10);

            ResetEffect();//initialise effect variables
        }

        /// <summary>
        /// Loads the overlay and the font.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            scoreOverlay = content.Load<Texture2D>(@"Content\Core Game\score_overlay");
            font = content.Load<SpriteFont>(@"Content\Fonts\gamefont");
        }

        /// <summary>
        /// When the player scores a point, the stored score increases.
        /// </summary>
        public void Scored()
        {
            GameState.playerScore++;
            ResetEffect();//cancels current effect if one is playing
            effect = true;
            effectText = "" + GameState.playerScore;
            effectPos = thisScoreTextPos;
        }

        /// <summary>
        /// When the opponent scores a point, the stored score increases.
        /// </summary>
        public void OpponentScored()
        {
            GameState.opponentScore++;
            ResetEffect();//cancels current effect if one is playing
            effect = true;
            effectText = "" + GameState.opponentScore;
            effectPos = opponentScoreTextPos;
        }

        public void ResetEffect()
        {
            effect = false;
            scale = 1.0f;
            effectText = "";
            effectPos = Vector2.Zero;
            effectTime = 0.0f;
            effectColor = Color.Red;
        }
        
        /// <summary>
        /// Draws the scorebox with the points overlaid.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(scoreOverlay, scoreBoxPos, Color.White);

            spriteBatch.DrawString(font, ""+ GameState.playerScore, thisScoreTextPos, Color.Black);
            spriteBatch.DrawString(font, ""+ GameState.opponentScore, opponentScoreTextPos, Color.Black);

            //Draw effect upon scoreup
            if (effect)
                spriteBatch.DrawString(font, effectText, effectPos, effectColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            if (effect)
            {
                //Calculates the effect text's transparency based on the time elapsed
                float translucency = 255.0f - ((effectTime /effectLength) * 255.0f);
                effectColor.A = (byte) translucency;

                //Increments timer and scales the text up
                effectTime += gameTime.ElapsedGameTime.Milliseconds;
                scale += 0.008f * gameTime.ElapsedGameTime.Milliseconds;

                //Resets effect after certain time has elapsed
                if (effectTime >= effectLength)
                    ResetEffect();
            }
        }
    }
}
