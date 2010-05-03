#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
//  Author(s):
//      David Valente
//
//-----------------------------------------------------------------------------
#endregion

using System;
using AirHockeyGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameScreenManager.ScreenSystem {
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    public class OptionsMenuScreen: MenuScreen {
        #region Fields

        KeyboardState prevKeyboardState;
        Texture2D background;

        //Menu system has a flaw where exiting too quickly may cause errors in the menu system
        //since the items can't transition off quickly enough.
        float exitTimer;
        const float timeBeforeExitAllowed = 100.0f;
        bool exitAllowed;
        bool nameChanging;

        private Settings oldSettings;
        private Profile oldProfile;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base() {

            nameChanging = false;
            exitTimer = 0.0f;
            exitAllowed = false;

            //Copying current settings allows the system to
            //check for changes.
            oldSettings = new Settings();
            oldSettings.Copy(GameState.gameSettings);

            oldProfile = new Profile();
            oldProfile.Copy(GameState.playerProfile);

            //By copying the current settings, we can check if they
            //are changed.
            oldSettings = new Settings();
            oldSettings.Copy(GameState.gameSettings);

            MenuEntries.Add("Ai Difficulty: " + GameState.gameSettings.difficulty);
            MenuEntries.Add("Screen Mode: " + GameState.gameSettings.screenSize);
            MenuEntries.Add("Name: " + GameState.playerProfile.Name);
            MenuEntries.Add("Win/Lose/Draw: " + GameState.playerProfile.WonLostDrawn);
            MenuEntries.Add("Return");
        }

        public override void LoadContent() {
            background = ScreenManager.ContentManager.Load<Texture2D>(@"Content\ScreenSystem\Images\backgroundOrange");
            base.LoadContent();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Saves changes to file if settings differ from when the screen was
        /// finished loading.
        /// </summary>
        public void SaveChanges() {
            //If settings have changed or no previous settings existed,
            //write settings to file
            if(oldSettings == null || !oldSettings.IsEqual(GameState.gameSettings)) {
                GameState.gameSettings.WriteSettingsFile();
                oldSettings = GameState.gameSettings;
            }

            //If profile has been changed, save it to file
            if(oldProfile == null || !oldProfile.Matches(GameState.playerProfile)) {
                //If new name is empty, old name is inserted
                if(GameState.playerProfile.Name.Equals(""))
                    if(!oldProfile.Name.Equals(""))
                        GameState.playerProfile.Name = oldProfile.Name;
                    else
                        GameState.playerProfile.Name = "Player Name";//if old name is also empty, default name is inserted

                GameState.playerProfile.WriteProfile("profile.dat");
                oldProfile = GameState.playerProfile;
            }
        }

        protected override void OnSelectEntry(int entryIndex) {
            switch(entryIndex) {

                //Difficulty
                case 0:
                    GameState.gameSettings.difficulty++;

                    if(GameState.gameSettings.difficulty > Settings.Difficulty.hard)
                        GameState.gameSettings.difficulty = 0;

                    MenuEntries[0] = ("Ai Difficulty: " + GameState.gameSettings.difficulty);
                    break;
                //Window size
                case 1:
                    if(GameState.gameSettings.screenSize == Settings.ScreenSize.fullscreen)
                        GameState.gameSettings.screenSize = Settings.ScreenSize.windowed;
                    else
                        GameState.gameSettings.screenSize = Settings.ScreenSize.fullscreen;

                    MenuEntries[1] = ("Screen Mode: " + GameState.gameSettings.screenSize);
                    break;
                //Name
                case 2:
                    if(!nameChanging) {
                        nameChanging = true;
                        GameState.playerProfile.Name = "";

                        MenuEntries[2] = ("Name: " + GameState.playerProfile.Name + "|");
                    }
                    break;
                //Won/Lost/Drawn
                case 3:
                    break;
                //Exit
                case 4:
                    SaveChanges();//save changes to settings to file
                    ScreenManager.GoToMainMenu();
                    break;
            }
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
            //If user presses escape, they return to previous menu
            KeyboardState keyboard = Keyboard.GetState();

            if(exitTimer < timeBeforeExitAllowed)
                exitTimer += gameTime.ElapsedGameTime.Milliseconds;
            else
                exitAllowed = true;


            //Checks screen is active and not transitioning
            if(keyboard.IsKeyDown(Keys.Escape) && !prevKeyboardState.IsKeyDown(Keys.Escape) && !IsExiting && exitAllowed) {
                if(ScreenState != ScreenState.TransitionOn) {
                    SaveChanges();//save changes to external file                        
                    ScreenManager.GoToMainMenu();
                }
            }

            //If player is changing the name displayed
            if(nameChanging) {
                Keys[] pressedKeys = keyboard.GetPressedKeys();
                for(int i = 0; i < pressedKeys.Length; i++) {
                    if(GameState.playerProfile.Name.Length < Profile.MaxNameLength) {
                        //Checks it's an alphabet character
                        if((int)pressedKeys[i] >= 65 && (int)pressedKeys[i] <= 90) {
                            //Check it was not held down
                            if(!prevKeyboardState.IsKeyDown(pressedKeys[i])) {
                                //Converts character to lowercase
                                String character = pressedKeys[i].ToString().ToLower();

                                //If shift is held, it's changed to upperspace
                                if(keyboard.IsKeyDown(Keys.RightShift) || keyboard.IsKeyDown(Keys.LeftShift))
                                    character = character.ToUpper();

                                //Appends character to name
                                GameState.playerProfile.Name += character;

                                //Displays new name
                                MenuEntries[2] = ("Name: " + GameState.playerProfile.Name + "|");
                            }
                        }
                        //Adds space if player presses spacebar
                        if(pressedKeys[i] == Keys.Space && !prevKeyboardState.IsKeyDown(Keys.Space)) {
                            GameState.playerProfile.Name += " ";
                            //Displays new name
                            MenuEntries[2] = ("Name: " + GameState.playerProfile.Name + "|");
                        }
                    }
                    //User can press backspace to delete last character
                    if(pressedKeys[i] == Keys.Back && !prevKeyboardState.IsKeyDown(Keys.Back) && GameState.playerProfile.Name.Length > 0) {
                        GameState.playerProfile.Name = GameState.playerProfile.Name.Substring(0, GameState.playerProfile.Name.Length - 1);
                        MenuEntries[2] = ("Name: " + GameState.playerProfile.Name + "|");
                    }
                }
            }

            //If player presses up or down, they stop editing the name
            if(keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.Up)) {
                nameChanging = false;
                MenuEntries[2] = ("Name: " + GameState.playerProfile.Name);
            }

            prevKeyboardState = keyboard;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        public override void Draw(GameTime gameTime) {
            ScreenManager.SpriteBatch.Begin();

            Color fade = new Color(255, 255, 255, TransitionAlpha); //anything drawn with this color will fade properly
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.SpriteBatch.Draw(background, fullscreen, fade);

            base.Draw(gameTime);
            ScreenManager.SpriteBatch.End();
        }


        #endregion
    }
}
