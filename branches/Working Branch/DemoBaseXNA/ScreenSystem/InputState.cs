#region File Description

//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DemoBaseXNA.ScreenSystem {
    /// <summary>
    /// Helper for reading input from keyboard and gamepad. This class tracks both
    /// the current and previous state of both input devices, and implements query
    /// properties for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState {

        public KeyboardState CurrentKeyboardState;
        public MouseState CurrentMouseState;
        public KeyboardState LastKeyboardState;
        public MouseState LastMouseState;

        /// <summary>
        /// Checks for a "menu up" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuUp {
            get {
                return IsNewKeyPress(Keys.Up);
            }
        }

        /// <summary>
        /// Checks for a "menu down" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuDown {
            get {
                return IsNewKeyPress(Keys.Down);
            }
        }

        /// <summary>
        /// Checks for a "menu select" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuSelect {
            get {
                return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);
            }
        }

        /// <summary>
        /// Checks for a "menu cancel" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuCancel {
            get {
                return IsNewKeyPress(Keys.Escape);
            }
        }

        /// <summary>
        /// Checks for a "pause the game" input action (on either keyboard or gamepad).
        /// </summary>
        public bool PauseGame {
            get {
                return IsNewKeyPress(Keys.Escape);
            }
        }

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update() {
            LastKeyboardState = CurrentKeyboardState;
            LastMouseState = CurrentMouseState;
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public bool IsNewKeyPress(Keys key) {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

        public bool OneOfKeysPressed(params Keys[] keys) {
            foreach(Keys key in keys) {
                if(IsNewKeyPress(key)) {
                    return true;
                }
            }
            return false;
        }
    }
}