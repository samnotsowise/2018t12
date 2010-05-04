using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AirHockeyGame;

namespace GameScreenManager.ScreenSystem
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        public int _id;
        private Dictionary<int, MenuItem> _mainMenuItems = new Dictionary<int, MenuItem>();
        private Texture2D background;
        private Texture2D logo;
        private Texture2D profileBoard;
        private Texture2D iconFrame;
        private ContextBox contentBox;
        private SpriteFont font;
        private const float boxPosX = 600.0f;
        private const float boxPosY = 300.0f;
        private const float logoPosX = 150.0f;
        private const float logoPosY = 50.0f;

        ContentManager content;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
        {
            LeftBorder = 100;
            MenuEntries.Add("Play Single Player Game");
            MenuEntries.Add("Start Multiplayer Game");
            MenuEntries.Add("Find Multiplayer Game");
            MenuEntries.Add("Options");
            MenuEntries.Add("Exit");
        }

        /// <summary>
        /// Responds to user menu selections.
        /// </summary>
        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0:
                    ScreenManager.StartGame('s');
                    break;
                case 1:
                    ScreenManager.StartGame('m');
                    break;
                case 2:
                    ScreenManager.StartGame('f');
                    break;
                case 3:
                    //also remove the screen that called this pausescreen
                    ScreenManager.AddScreen(new OptionsMenuScreen());
                    break;
                case 4:
                    ScreenManager.Game.Exit();
                    break;
            }
        }

        /// <summary>
        /// Updates the contentBox
        /// </summary>
        /// <param name="input"></param>
        public override void HandleInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.MenuUp)
            {
                contentBox.Cycle(false, MenuEntries.Count); 
            }

            // Move to the next menu entry?
            if (input.MenuDown)
            {
                contentBox.Cycle(true, MenuEntries.Count);
            }

            base.HandleInput(input);
        }

        public override void Initialize()
        {
            contentBox = new ContextBox(new Vector2(boxPosX, boxPosY));
            contentBox.Initialise();
            base.Initialize();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            background = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\background");
            logo = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\logo");
            profileBoard = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\profileBoard");
            iconFrame = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\iconFrame");
            font = content.Load<SpriteFont>(@"Fonts\gamefont");
            contentBox.LoadContent(content);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Color fade = new Color(255, 255, 255, TransitionAlpha); //anything drawn with this color will fade properly
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            ScreenManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            ScreenManager.SpriteBatch.Draw(background, fullscreen, fade);
            
            //Draw logo
            Vector2 logoPosition = new Vector2(logoPosX, logoPosY);
            logoPosition.Y -= transitionOffset * 150;
            ScreenManager.SpriteBatch.Draw(logo, logoPosition, null, fade, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            //Draw contentbox
            contentBox.Draw(ScreenManager.SpriteBatch, fade, transitionOffset);

            
            
            //Scale board to match name length
            int boardWidth;
            if(GameState.playerProfile.Name.Length <= 7)
                boardWidth = profileBoard.Width;
            else
                boardWidth = profileBoard.Width + (GameState.playerProfile.Name.Length - 7) * 22;

            //Draw profile and profile details
            //Draw board
            ScreenManager.SpriteBatch.Draw(profileBoard,new Rectangle(100,525, boardWidth, profileBoard.Height), fade);
            //Draw icon frame
            ScreenManager.SpriteBatch.Draw(iconFrame, new Vector2(118, 544), fade);
            //Draw icon
            ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.playerProfile.PictureIndex], new Vector2(121, 547), null, fade, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            //Draw name string
            ScreenManager.SpriteBatch.DrawString(font, "Name: " + GameState.playerProfile.Name, new Vector2(225, 550), fade, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
            //Draw stats string
            ScreenManager.SpriteBatch.DrawString(font, "W/L/D: " + GameState.playerProfile.WonLostDrawn, new Vector2(225, 600), fade, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);

            base.Draw(gameTime);
            
            ScreenManager.SpriteBatch.End();
        }
    }
}