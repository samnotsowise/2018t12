#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
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

        private Settings oldSettings;
        public static Settings currentSettings;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base() {
            // Create our menu entries.
            //AddOptionsSetting("AI difficulty");

            //AddOptionsItem("Return", null, true);
            exitTimer = 0.0f;
            exitAllowed = false;

            oldSettings = new Settings();
            oldSettings.Copy(currentSettings);

            //By copying the current settings, we can check if they
            //are changed.
            oldSettings = new Settings();
            oldSettings.Copy(currentSettings);

            MenuEntries.Add("Ai Difficulty: " + currentSettings.difficulty);
            MenuEntries.Add("Screen Mode: " + currentSettings.screenSize);
            MenuEntries.Add("Return");
        }

        public override void LoadContent() {
            background = ScreenManager.ContentManager.Load<Texture2D>(@"Content\Images\backgroundOrange");
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
            if(oldSettings == null || !oldSettings.IsEqual(currentSettings)) {
                currentSettings.WriteSettingsFile();
                oldSettings = currentSettings;
            }
        }

        protected override void OnSelectEntry(int entryIndex) {
            switch(entryIndex) {
                case 0:
                    currentSettings.difficulty++;

                    if(currentSettings.difficulty > Settings.Difficulty.hard)
                        currentSettings.difficulty = 0;

                    MenuEntries[0] = ("Ai Difficulty: " + currentSettings.difficulty);
                    break;
                case 1:
                    if(currentSettings.screenSize == Settings.ScreenSize.fullscreen)
                        currentSettings.screenSize = Settings.ScreenSize.windowed;
                    else
                        currentSettings.screenSize = Settings.ScreenSize.fullscreen;

                    MenuEntries[1] = ("Screen Mode: " + currentSettings.screenSize);
                    break;
                case 2:
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
