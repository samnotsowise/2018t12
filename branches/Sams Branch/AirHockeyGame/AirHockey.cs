using System;
using FarseerGames.AirHockeyGame.Screens;
using GameScreenManager.Components;
using GameScreenManager.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AirHockeyGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AirHockey: Game {

        private GraphicsDeviceManager _graphics;
        private ExplosionParticleSystem explosion;
        private ExplosionSmokeParticleSystem smoke;

        // a random number generator for the particle system
        private static Random random = new Random();
        public static Random Random {
            get { return random; }
        }

        public AirHockey() {
            Window.Title = "AirHockey";
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 10);
            IsFixedTimeStep = true;

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.IsFullScreen = false;
            //IsMouseVisible = true;

            //Set window defaults. Parent game can override in constructor
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            //new-up components and add to Game.Components
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);
            ScreenManager.GoToMainMenu();

            #region Particles
            //Particles
            explosion = new ExplosionParticleSystem(this, 1);
            Components.Add(explosion);

            smoke = new ExplosionSmokeParticleSystem(this, 2);
            Components.Add(smoke);

            #endregion

            ScreenManager.GoToMainMenu();
        }

        public void PlaySinglePlayer() {
            ScreenManager.AddScreen(new MainGameScreen());
        }

        public ScreenManager ScreenManager { get; set; }

        protected override void Update(GameTime gameTime) {
            #region Menu Options
            //The ScreenManager can't access the MainGameScreen class
            //It also can't send requests to the AirHockey class
            //Therefore, the airhockey class detects when it wants to start a game
            if(ScreenManager.StartTheGame) {
                switch(ScreenManager.gameType) {
                    case ScreenManager.GameType.FindGame:
                        ScreenManager.AddScreen(new MainGameScreen());
                        break;
                    case ScreenManager.GameType.SinglePlayer:
                        ScreenManager.AddScreen(new MainGameScreen());
                        break;
                    case ScreenManager.GameType.StartMultiplayer:
                        ScreenManager.AddScreen(new MainGameScreen());
                        break;
                }

                ScreenManager.StartTheGame = false;
            }
            #endregion

            #region Particles

            //Checks the ScreenManager to see if particles should be added, which type and where.
            if(ScreenManager.AddParticles) {
                switch(ScreenManager.ParticleTypeToAdd) {
                    case ScreenManager.ParticleType.Explosion:
                        explosion.AddParticles(ScreenManager.ParticleSourcePosition);
                        break;
                    case ScreenManager.ParticleType.Smoke:
                        smoke.AddParticles(ScreenManager.ParticleSourcePosition);
                        break;
                }

                ScreenManager.AddParticles = false;
            }

            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            _graphics.ApplyChanges();
            base.Initialize();
        }

        //  Picks a random number
        public static float RandomBetween(float min, float max) {
            return min + (float)random.NextDouble() * (max - min);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e) {
            if(Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0) {
                _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            }
        }
    }
}