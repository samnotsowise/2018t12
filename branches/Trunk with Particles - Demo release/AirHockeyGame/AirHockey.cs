using System;
using DemoBaseXNA.Components;
using DemoBaseXNA.ScreenSystem;
using FarseerGames.AirHockeyGame.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarseerGames.AirHockeyGame {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AirHockey : Game {

        private GraphicsDeviceManager _graphics;

        public AirHockey() {
            Window.Title = "AirHockey";
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 10);
            IsFixedTimeStep = true;

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.IsFullScreen = false;
            IsMouseVisible = false;

            //Set window defaults. Parent game can override in constructor
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            //new-up components and add to Game.Components
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);
            ScreenManager.GoToMainMenu();
            
            
            ScreenManager.GoToMainMenu();
        }

        public void PlaySinglePlayer()
        {
            ScreenManager.AddScreen(new MainScreen());
        }

        public ScreenManager ScreenManager { get; set; }

        protected override void Update(GameTime gameTime)
        {
            //The ScreenManager can't access the MainScreen class
            //It also can't send requests to the AirHockey class
            //Therefore, the airhockey class detects when it wants to start a game

            if (ScreenManager.gameStart)
            {
                switch (ScreenManager.gameType)
                {
                    case ScreenManager.GameType.FindGame:
                        ScreenManager.AddScreen(new MainScreen());
                        break;
                    case ScreenManager.GameType.SinglePlayer:
                        ScreenManager.AddScreen(new MainScreen());
                        break;
                    case ScreenManager.GameType.StartMultiplayer:
                        ScreenManager.AddScreen(new MainScreen());
                        break;
                }
                
                ScreenManager.gameStart = false;
            }

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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
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