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

namespace DemoBaseXNA.ScreenSystem
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    public class OptionsMenuScreen : MenuScreen
    {
        #region Fields
        
        KeyboardState prevKeyboardState;
        Texture2D background;

        //Menu system has a flaw where exiting too quickly may cause errors in the menu system
        //since the items can't transition off quickly enough.
        float exitTimer;                
        const float timeBeforeExitAllowed = 100.0f;
        bool exitAllowed;

        enum Difficulty
        {
            Low,
            Medium,
            High,
        }

        static Difficulty aiDifficulty = Difficulty.Medium;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base()
        {
            // Create our menu entries.
            //AddOptionsSetting("AI difficulty");
            
            //AddOptionsItem("Return", null, true);
            exitTimer = 0.0f;
            exitAllowed = false;

            MenuEntries.Add("Ai Difficulty: " + aiDifficulty);
            MenuEntries.Add("Return");

        }

        public override void LoadContent()
        {
            background = ScreenManager.ContentManager.Load<Texture2D>(@"Content\Images\backgroundOrange");
            base.LoadContent();
        }    

        #endregion

        #region Handle Input

        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0:
                    aiDifficulty++;

                    if (aiDifficulty > Difficulty.High)
                        aiDifficulty = 0;

                    MenuEntries[0] = ("Ai Difficulty: " + aiDifficulty);
                    break;
                case 1:
                    //also remove the screen that called this pausescreen

                    ScreenManager.GoToMainMenu();
                    break;
            }
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //If user presses escape, they return to previous menu
            KeyboardState keyboard = Keyboard.GetState();

            if (exitTimer < timeBeforeExitAllowed)
                exitTimer += gameTime.ElapsedGameTime.Milliseconds;
            else
                exitAllowed = true;
            

            //Checks screen is active and not transitioning
            if (keyboard.IsKeyDown(Keys.Escape) && !prevKeyboardState.IsKeyDown(Keys.Escape) && !IsExiting && exitAllowed)
            {
                if (ScreenState != ScreenState.TransitionOn)
                {
                    ScreenManager.GoToMainMenu();
                }
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
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
