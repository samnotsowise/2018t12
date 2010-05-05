using System.Text;
using FarseerGames.AirHockeyGame;
using FarseerGames.FarseerPhysics;
using GameScreenManager;
using GameScreenManager.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AirHockeyGame.Screens {
    /// <summary>
    /// The main game screen
    /// </summary>
    public class MainScreen: GameScreen {

        private PlayerPaddle playerPaddle;
        private OpponentPaddle opponentPaddle;
        private Puck puck;
        private Board board;
        private ScoreBox scoreBox;
        private SpriteFont font;
        private float elapsedTime;
        private float gameLength = 180000;
        bool gameActive;

        //Player details
        Vector2 playerIconPos, opponentIconPos, playerNamePos, opponentNamePos;

        /// <summary>
        /// Initialises MainScreen
        /// </summary>
        public override void Initialize() {

            elapsedTime = 0;
            gameActive = true;

            //Player detail displays
            this.playerIconPos = new Vector2(325, 10);
            this.playerNamePos = new Vector2(315 - 12 * GameState.playerProfile.Name.Length, 40);
            this.opponentIconPos = new Vector2(615, 10);
            this.opponentNamePos = new Vector2(705, 40);

            //Clear GameState
            GameState.Initialise();

            //Physics Engine
            this.PhysicsSimulator = new PhysicsSimulator(new Vector2(0, 0));
            this.PhysicsSimulatorView = new PhysicsSimulatorView(this.PhysicsSimulator);

            base.Initialize();

        }

        /// <summary>
        /// Loads MainScreen content
        /// </summary>
        public override void LoadContent() {

            //Font
            this.font = this.ScreenManager.ContentManager.Load<SpriteFont>(@"Content\Fonts\gamefont");

            //Load Board
            this.board = new Board(this.PhysicsSimulator);

            //Load Puck
            this.puck = new Puck(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\puck"), 48, new Vector2(512, 384), this.PhysicsSimulator);

            //Load Paddles
            this.playerPaddle = new PlayerPaddle(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\playerPaddle"), 82, new Vector2(256, 384), this.PhysicsSimulator);
            this.opponentPaddle = new OpponentPaddle(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\opponentPaddle"), 82, new Vector2(768, 384), this.PhysicsSimulator);

            //Load ScoreBox
            this.scoreBox = new ScoreBox();
            this.scoreBox.LoadContent(this.ScreenManager.ContentManager);

            base.LoadContent();
        }

        /// <summary>
        /// Handes input to MainScreen
        /// </summary>
        public override void HandleInput(InputState input) {

            #region Screen Manager

            if(this.firstRun) {
                this.ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
                this.firstRun = false;
            }
            if(input.PauseGame) {
                this.ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
            }

            #endregion

            base.HandleInput(input);
        }

        /// <summary>
        /// Update MainScreen
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {

            #region Screen Manager

            //Prevent menu mouse movements passing on to game
            if(coveredByOtherScreen || otherScreenHasFocus) {
                this.playerPaddle.ResetMouse();
            } else {

                //Update time
                if(elapsedTime < gameLength)
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                else
                    gameActive = false;
            }

            #endregion

            #region Update Board Objects

            //Make sure none of the board objects are getting stuck in each other - thanks to FARSEER's engine bug
            if(this.playerPaddle.rect.Contains(this.puck.rect) || this.opponentPaddle.rect.Contains(this.puck.rect)) {
                this.puck.UpdatePosition(this.puck.prevPos2);
            }
            this.puck.Update();
            if(this.playerPaddle.rect.Intersects(this.board.boundaries[4].rect)) {
                this.playerPaddle.UpdatePosition(this.playerPaddle.prevPos2);
            }
            this.playerPaddle.Update();
            if(this.opponentPaddle.rect.Intersects(this.board.boundaries[4].rect)) {
                this.opponentPaddle.UpdatePosition(this.opponentPaddle.prevPos2);
            }
            this.opponentPaddle.Update();

            #endregion

            #region Check Goals

            //Player scored
            if(this.board.detectors[0].rect.Intersects(this.puck.rect)) {
                this.scoreBox.Scored();
                this.puck.reset();
                this.playerPaddle.reset();
                this.opponentPaddle.reset();
            }

            //Opponent scored
            if(this.board.detectors[1].rect.Intersects(this.puck.rect)) {
                this.scoreBox.OpponentScored();
                this.puck.reset();
                this.playerPaddle.reset();
                this.opponentPaddle.reset();
            }

            //Check for Game Over
            this.CheckGameOver();

            #endregion

            //Update scorebox
            this.scoreBox.Update(gameTime);


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Draws the MainScreen
        /// </summary>
        public override void Draw(GameTime gameTime) {

            this.ScreenManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            /* The order of the following segment is crucial.
             * Draw in order to avoid having to use layers */

            //Draw Board background
            this.ScreenManager.SpriteBatch.Draw(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\boardSurface"), this.board.rect, Color.White);

            //Draw Puck
            this.puck.Draw(this.ScreenManager.SpriteBatch);

            //Draw Paddles
            this.playerPaddle.Draw(this.ScreenManager.SpriteBatch);
            this.opponentPaddle.Draw(this.ScreenManager.SpriteBatch);

            //Draw Board Foreground
            this.ScreenManager.SpriteBatch.Draw(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\boardEdges"), this.board.rect, Color.White);

            //Draw player avatars
            this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.playerProfile.PictureIndex], this.playerIconPos, Color.White);
            this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.opponentProfile.PictureIndex], this.opponentIconPos, Color.White);
            //Draw player names
            this.ScreenManager.SpriteBatch.DrawString(font, GameState.playerProfile.Name, this.playerNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            this.ScreenManager.SpriteBatch.DrawString(font, GameState.opponentProfile.Name, this.opponentNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            //Draw Scorebox
            this.scoreBox.Draw(this.ScreenManager.SpriteBatch);

            //Draw time
            this.ScreenManager.SpriteBatch.DrawString(font, "Time Remaining: " + (int)((gameLength - (elapsedTime)) / 1000), new Vector2(10, 20), Color.Yellow, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            this.ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Get the screen title
        /// </summary>
        /// <returns>Screen title</returns>
        public static string GetTitle() {
            return "Play Game";
        }

        /// <summary>
        /// Get the screen details
        /// </summary>
        /// <returns>Screen details</returns>
        private static string GetDetails() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This is where we wait for all players to be ready");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Use the mouse to control your paddle.");
            sb.AppendLine("Defend your goal while attacking your opponent's.");
            return sb.ToString();
        }

        private void CheckGameOver() {
            if(GameState.playerScore > 2) {
                //You Win
                GameState.playerProfile.GameWon();
                ScreenManager.AddScreen(new GameEndOverlay("Game Won", "Congratulations. You won."));
            } else if(GameState.opponentScore > 2) {
                //You Lose
                GameState.playerProfile.GameLost();
                ScreenManager.AddScreen(new GameEndOverlay("Game Lost", "Sadly, you lost."));
            } else if(this.elapsedTime >= this.gameLength && gameActive == true) {
                //You Draw
                gameActive = false;
                GameState.playerProfile.GameDrawn();
                ScreenManager.AddScreen(new GameEndOverlay("Game Drawn", "Time up. You have drawn."));
            }


        }
    }
}