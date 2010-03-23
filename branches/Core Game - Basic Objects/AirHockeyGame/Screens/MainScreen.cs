using System.Text;
using DemoBaseXNA;
using DemoBaseXNA.DrawingSystem;
using DemoBaseXNA.ScreenSystem;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.AirHockeyGame.Screens {
    public class MainScreen : GameScreen {

        private Paddle playerPaddle;
        private Puck puck;

        public override void Initialize() {
            PhysicsSimulator = new PhysicsSimulator(new Vector2(0, 0));
            PhysicsSimulatorView = new PhysicsSimulatorView(PhysicsSimulator);
            base.Initialize();
        }

        public override void LoadContent() {

            //Load puck
            this.puck = new Puck(ScreenManager.ContentManager.Load<Texture2D>("Content\\Game\\puck"), 48, new Vector2(725, 384), PhysicsSimulator);
            GeomFactory.Instance.CreateCircleGeom(PhysicsSimulator, this.puck.body, (int)this.puck.width / 2, (int)this.puck.width);

            //Load playerPaddle
            this.playerPaddle = new Paddle(ScreenManager.ContentManager.Load<Texture2D>("Content\\Game\\playerPaddle"), 82, new Vector2(256, 384), PhysicsSimulator);
            GeomFactory.Instance.CreateCircleGeom(PhysicsSimulator, this.playerPaddle.body, (int)this.playerPaddle.width / 2, (int)this.playerPaddle.width);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime) {
            ScreenManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            ScreenManager.SpriteBatch.Draw(puck.texture, puck.rect, Color.White);
            ScreenManager.SpriteBatch.Draw(playerPaddle.texture, playerPaddle.rect, Color.White);

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input) {
            if(firstRun) {
                ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
                firstRun = false;
            }
            if(input.PauseGame) {
                ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
            }
            base.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
            if(coveredByOtherScreen || otherScreenHasFocus) {
                this.playerPaddle.ResetMouse();
            }

            //Calculate the mouse's difference in position from the last update
            Vector2 diff;
            diff.X = Mouse.GetState().X - this.playerPaddle.body.Position.X;
            diff.Y = Mouse.GetState().Y - this.playerPaddle.body.Position.Y;

            //Apply a force accordingly
            this.playerPaddle.ApplyForce(new Vector2(diff.X * 60, diff.Y * 60));

            //Move the mouse to the new location
            this.playerPaddle.ResetMouse();

            //Update playerPaddle and puck objects
            this.playerPaddle.Update();
            this.puck.Update();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        public static string GetTitle() {
            return "Play Game";
        }

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