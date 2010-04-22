using System.Text;
using FarseerGames.FarseerPhysics;
using GameScreenManager;
using GameScreenManager.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame.Screens {

    /// <summary>
    /// The main game screen
    /// </summary>
    public class MainScreen: GameScreen {

        private PlayerPaddle playerPaddle;
        private PlayerPaddle netPaddle;
        private Puck puck;
        private Board board;

        /// <summary>
        /// Initialises MainScreen
        /// </summary>
        public override void Initialize() {
            PhysicsSimulator = new PhysicsSimulator(new Vector2(0, 0));
            PhysicsSimulatorView = new PhysicsSimulatorView(PhysicsSimulator);
            base.Initialize();
        }

        /// <summary>
        /// Loads MainScreen content
        /// </summary>
        public override void LoadContent() {
            
            //Load board
            this.board = new Board(PhysicsSimulator);

            //Load puck
            this.puck = new Puck(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\puck"), 48, new Vector2(512, 384), PhysicsSimulator, this.board.restrictors[0]);

            //Load Paddles
            this.playerPaddle = new PlayerPaddle(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\playerPaddle"), 82, new Vector2(256, 384), PhysicsSimulator);
            this.netPaddle = new PlayerPaddle(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\opponentPaddle"), 82, new Vector2(768, 384), PhysicsSimulator);
            
            base.LoadContent();
        }

        /// <summary>
        /// Handes input to MainScreen
        /// </summary>
        public override void HandleInput(InputState input) {
            if(firstRun) {
                ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
                firstRun = false;
            }
            if(input.PauseGame) {
                ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
            }

            //Particles
            if(input.IsNewKeyPress(Keys.Space)) {
                ScreenManager.RequestParticleEffect('e', this.playerPaddle.position);
            }
            if(input.IsNewKeyPress(Keys.F)) {
                ScreenManager.RequestParticleEffect('s', this.puck.position);
            }

            base.HandleInput(input);
        }

        /// <summary>
        /// Update MainScreen
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {


            //Prevent menu mouse movements passing on to game
            if (coveredByOtherScreen || otherScreenHasFocus)
            {
                //this.playerPaddle.ResetMouse();
                //this.networkPaddle.ResetMouse();
            }
            else
            {
                //Update playerPaddle, networkPaddle and puck objects
                if (ScreenManager.gameType == ScreenManager.GameType.StartMultiplayer)
                {

                    this.playerPaddle.Update();
                    if (ScreenManager.blnConnected)
                    {
                        ScreenManager.handleServerUpdate(Mouse.GetState().X, Mouse.GetState().Y);
                        this.netPaddle.Update(ScreenManager.NetworkMouseX, ScreenManager.NetworkMouseY);
                    }
                }
                else if (ScreenManager.gameType == ScreenManager.GameType.FindGame)
                {
                    this.netPaddle.Update();
                    if (ScreenManager.blnConnected)
                    {
                        ScreenManager.handleClientUpdate(Mouse.GetState().X, Mouse.GetState().Y);
                        this.playerPaddle.Update(ScreenManager.NetworkMouseX, ScreenManager.NetworkMouseY);
                    }
                }

                this.puck.Update();

                if (this.playerPaddle.rect.Contains(this.puck.rect) || this.netPaddle.rect.Contains(this.puck.rect))
                {
                    this.puck.UpdatePosition(this.puck.initialPosition);
                }
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Draws the MainScreen
        /// </summary>
        public override void Draw(GameTime gameTime) {
            ScreenManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            ScreenManager.SpriteBatch.Draw(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\boardSurface"), this.board.rect, Color.White);
            ScreenManager.SpriteBatch.Draw(this.puck.texture, this.puck.rect, Color.White);
            ScreenManager.SpriteBatch.Draw(this.playerPaddle.texture, this.playerPaddle.rect, Color.White);
            ScreenManager.SpriteBatch.Draw(this.netPaddle.texture, this.netPaddle.rect, Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\boardEdges"), this.board.rect, Color.White);
            ScreenManager.SpriteBatch.End();

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
    }
}