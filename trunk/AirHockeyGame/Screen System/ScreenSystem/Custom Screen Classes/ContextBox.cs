/*
 * Displays icons for whichever menu option is selected.
 * 
 * Author(s)
 *  David Valente
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameScreenManager.ScreenSystem {
    class ContextBox {
        Texture2D box;      //background box that images are overlayed onto
        Texture2D[] images; //holds different context images
        Vector2 originalPosition;
        private const float scale = 0.8f;
        enum MenuOption {
            SinglePlayer,
            Multiplayer,
            FindGame,
            Options,
            Exit
        }
        private MenuOption currentOption;

        public ContextBox(Vector2 position) {
            originalPosition = position;
        }

        public void Initialise() {
            images = new Texture2D[5];
            currentOption = MenuOption.SinglePlayer;
        }

        public void LoadContent(ContentManager content) {
            box = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\Context\box");
            images[0] = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\Context\singleplayer");
            images[1] = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\Context\multiplayer");
            images[2] = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\Context\findgame");
            images[3] = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\Context\options");
            images[4] = content.Load<Texture2D>(@"ScreenSystem\Images\Main Menu\Context\exit");
        }

        /// <summary>
        /// Cycles through the options, accounting for list size.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="listSize"></param>
        public void Cycle(bool forward, int listSize) {
            if(forward)
                currentOption++;
            else
                currentOption--;

            //User can cycle through menu options
            if((int)currentOption >= listSize)//loop back to start
                currentOption = (MenuOption)0;
            else if((int)currentOption < 0)//loop backwards to exit option
                currentOption = (MenuOption)listSize - 1;
        }

        /// <summary>
        /// Updates the content box based on user input.
        /// Does not update if the screen is transitionining or otherwise not active.
        /// </summary>
        /// <param name="input"></param>
        public void UpdateBox(InputState input, int menuSize, PlayerIndex? ControllingPlayer, bool active) {
            if(!active)
                return;

            if(input.MenuUp)
                Cycle(false, menuSize);
            else if(input.MenuDown)
                Cycle(true, menuSize);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, float transitionOffset) {
            Vector2 position = originalPosition;

            position.X += transitionOffset * 100;

            spriteBatch.Draw(box,
                             position,
                             null,
                             color,
                             0.0f,
                             Vector2.Zero,
                             scale,
                             SpriteEffects.None,
                             0);
            spriteBatch.Draw(images[(int)currentOption],
                             position,
                             null,
                             color,
                             0.0f,
                             Vector2.Zero,
                             scale,
                             SpriteEffects.None,
                             0);
        }
    }
}
