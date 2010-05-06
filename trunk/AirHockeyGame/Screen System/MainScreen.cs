using System.Text;
using FarseerGames.AirHockeyGame;
using FarseerGames.FarseerPhysics;
using GameScreenManager;
using GameScreenManager.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using System.Threading;
using System.Net;
using Microsoft.Xna.Framework.Input;

namespace AirHockeyGame.Screens {
    /// <summary>
    /// The main game screen
    /// </summary>
    public class MainScreen: GameScreen {
        // GT - Network Variables
        private NetworkPaddle[] players = new NetworkPaddle[2];
        private int UserId = 0;
        NetClient client;
        NetBuffer buffer;
        NetMessageType type;

        private PlayerPaddle playerPaddle;
        private OpponentPaddle opponentPaddle;
        private Puck puck;
        private Board board;
        private ScoreBox scoreBox;
        private SpriteFont font;
        private float elapsedTime;
        private float gameLength = 180000;
        bool gameActive;

        //Player details
        Vector2 playerIconPos, opponentIconPos, playerNamePos, opponentNamePos;

        /// <summary>
        /// Initialises MainScreen
        /// </summary>
        public override void Initialize() {

            elapsedTime = 0;
            gameActive = true;

            //Player detail displays
            this.playerIconPos = new Vector2(325, 10);
            this.playerNamePos = new Vector2(315 - 12 * GameState.playerProfile.Name.Length, 40);
            this.opponentIconPos = new Vector2(615, 10);
            this.opponentNamePos = new Vector2(705, 40);

            //Clear GameState
            GameState.Initialise();

            //Physics Engine
            this.PhysicsSimulator = new PhysicsSimulator(new Vector2(0, 0));
            this.PhysicsSimulatorView = new PhysicsSimulatorView(this.PhysicsSimulator);

            //GT - initialise network
            #region start network client
            if (ScreenManager.gameType != ScreenManager.GameType.SinglePlayer)
            {
            
                // create a client with a default configuration
                try
                {
                    NetConfiguration config = new NetConfiguration("T12Hockey");
                    IPHostEntry ipEntry = Dns.GetHostByName(GameState.gameSettings.serverAddress);
                    IPAddress[] addr = ipEntry.AddressList;
                    IPAddress localAdd = addr[0];
                    config.Address = localAdd;

                    client = new NetClient(config);
                    client.SetMessageTypeEnabled(NetMessageType.ConnectionRejected, true);
                    client.SetMessageTypeEnabled(NetMessageType.DebugMessage, true);
                    client.Start();

                    // Wait half a second to allow server to start up if run via Visual Studio
                    Thread.Sleep(500);

                    // Emit discovery signal
                    client.DiscoverLocalServers(int.Parse(GameState.gameSettings.serverPort));

                    // create a buffer to read data into
                    buffer = client.CreateBuffer();
                }
                catch (System.Exception e)
                {
                    endGame(e.ToString());
                }

                if (ScreenManager.gameType == ScreenManager.GameType.StartMultiplayer)
                    UserId = 0;
                else if(ScreenManager.gameType == ScreenManager.GameType.FindGame)
                    UserId = 1;

            }
            #endregion

            base.Initialize();

        }

        /// <summary>
        /// Loads MainScreen content
        /// </summary>
        public override void LoadContent() {

            //Font
            this.font = this.ScreenManager.ContentManager.Load<SpriteFont>(@"Content\Fonts\gamefont");

            //Load Board
            this.board = new Board(this.PhysicsSimulator);

            //Load Puck
            this.puck = new Puck(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\puck"), 48, new Vector2(512, 384), this.PhysicsSimulator);

            //Load Paddles
            if (ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
            {
                //SINGLE PLAYER LOAD
                this.playerPaddle = new PlayerPaddle(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\playerPaddle"), 82, new Vector2(256, 384), this.PhysicsSimulator);
                this.opponentPaddle = new OpponentPaddle(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\opponentPaddle"), 82, new Vector2(768, 384), this.PhysicsSimulator);

            }
            else
            {
                //NETWORK PLAYER LOAD
                this.players[0] = new NetworkPaddle(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\playerPaddle"), 82, new Vector2(256, 384), PhysicsSimulator);
                this.players[1] = new NetworkPaddle(ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\opponentPaddle"), 82, new Vector2(768, 384), PhysicsSimulator);
            }
            
            //Load ScoreBox
            this.scoreBox = new ScoreBox();
            this.scoreBox.LoadContent(this.ScreenManager.ContentManager);

            base.LoadContent();
        }

        /// <summary>
        /// Handes input to MainScreen
        /// </summary>
        public override void HandleInput(InputState input) {

            #region Screen Manager

            if(this.firstRun) {
                this.ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
                this.firstRun = false;
            }
            if(input.PauseGame) {
                this.ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
            }

            #endregion

            base.HandleInput(input);
        }

        #region send to network
        public void sendCurrentPosition()
        {
            if (client != null)
            {
                if (client.Status == NetConnectionStatus.Connected)
                {
                    int x = Mouse.GetState().X;
                    int y = Mouse.GetState().Y;

                    // Send your current position
                    NetBuffer sendBuffer = new NetBuffer();
                    sendBuffer.Write(UserId);
                    sendBuffer.Write(x);
                    sendBuffer.Write(y);

                    client.SendMessage(sendBuffer, NetChannel.ReliableInOrder1);
                }
            }
        }
        #endregion

        #region receive from network
        public void getPositionsFromNetwork()
        {
            // check if any messages has been received
            while (client.ReadMessage(buffer, out type))
            {
                switch (type)
                {
                    case NetMessageType.ServerDiscovered:
                        // just connect to any server found!

                        // make hail
                        NetBuffer buf = client.CreateBuffer();
                        buf.Write("Hail from " + System.Environment.MachineName);
                        client.Connect(buffer.ReadIPEndPoint(), buf.ToArray());
                        break;
                    case NetMessageType.ConnectionRejected:
                        //Console.WriteLine("Rejected: " + buffer.ReadString());
                        break;
                    case NetMessageType.DebugMessage:
                    case NetMessageType.VerboseDebugMessage:
                        //Console.WriteLine(buffer.ReadString());
                        break;
                    case NetMessageType.StatusChanged:
                        string statusMessage = buffer.ReadString();
                        NetConnectionStatus newStatus = (NetConnectionStatus)buffer.ReadByte();
                        //Console.WriteLine("New status: " + newStatus + " (" + statusMessage + ")");
                        break;
                    case NetMessageType.Data:
                        // The server sent this data!
                        try
                        {
                            int id = buffer.ReadInt32();
                            int x = buffer.ReadInt32();
                            int y = buffer.ReadInt32();

                            players[id].MouseX = x; // Mouse.GetState().X;
                            players[id].MouseY = y; // Mouse.GetState().Y
                        }
                        catch (System.Exception e)
                        {
                            endGame(e.ToString());
                        }

                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// Update MainScreen
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {

            #region Screen Manager

            //Prevent menu mouse movements passing on to game
            if(coveredByOtherScreen || otherScreenHasFocus) {
                
                if(ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
                    this.playerPaddle.ResetMouse();
                else
                    this.players[UserId].ResetMouse();

            } else {

                //Update time
                if(elapsedTime < gameLength)
                    elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                else
                    gameActive = false;
            }

            #endregion

            #region Update Board Objects

            //Make sure none of the board objects are getting stuck in each other - thanks to FARSEER's engine bug
            
            
            if (ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
            {
                //SINGLE PLAYER UPDATE

                //update puck
                if (this.playerPaddle.rect.Contains(this.puck.rect) || this.opponentPaddle.rect.Contains(this.puck.rect))
                {
                    this.puck.UpdatePosition(this.puck.prevPos2);
                }
                this.puck.Update();

                //update player paddle
                if (this.playerPaddle.rect.Intersects(this.board.boundaries[4].rect))
                {
                    this.playerPaddle.UpdatePosition(this.playerPaddle.prevPos2);
                }
                this.playerPaddle.Update();

                //update opponent paddle
                if (this.opponentPaddle.rect.Intersects(this.board.boundaries[4].rect))
                {
                    this.opponentPaddle.UpdatePosition(this.opponentPaddle.prevPos2);
                }
                this.opponentPaddle.Update();
            }
            else
            {
                //NETWORK UPATE
                sendCurrentPosition();
                getPositionsFromNetwork();

                players[0].Update();
                players[1].Update();

                this.puck.Update();

                if (this.players[UserId].rect.Contains(this.puck.rect))
                {
                    this.puck.UpdatePosition(this.puck.initialPosition);
                }
            }

            #endregion

            #region Check Goals

            //Player scored
            if(this.board.detectors[0].rect.Intersects(this.puck.rect)) {
                this.scoreBox.Scored();
                this.puck.reset();

                if (ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
                {
                    this.playerPaddle.reset();
                    this.opponentPaddle.reset();
                }
                else
                {
                    this.players[0].reset();
                    this.players[1].reset();
                }
            }

            //Opponent scored
            if(this.board.detectors[1].rect.Intersects(this.puck.rect)) {
                this.scoreBox.OpponentScored();
                this.puck.reset();
                if (ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
                {
                    this.playerPaddle.reset();
                    this.opponentPaddle.reset();
                }
                else
                {
                    this.players[0].reset();
                    this.players[1].reset();
                }
            }

            //Check for Game Over
            this.CheckGameOver();

            #endregion

            //Update scorebox
            this.scoreBox.Update(gameTime);


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Draws the MainScreen
        /// </summary>
        public override void Draw(GameTime gameTime) {

            this.ScreenManager.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            /* The order of the following segment is crucial.
             * Draw in order to avoid having to use layers */

            //Draw Board background
            this.ScreenManager.SpriteBatch.Draw(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\boardSurface"), this.board.rect, Color.White);

            //Draw Puck
            this.puck.Draw(this.ScreenManager.SpriteBatch);

            //Draw Paddles
            if (ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
            {
                //SINGLE PLAYER DRAW
                this.playerPaddle.Draw(this.ScreenManager.SpriteBatch);
                this.opponentPaddle.Draw(this.ScreenManager.SpriteBatch);
            }
            else
            {
                //NETWORK DRAW
                foreach (NetworkPaddle p in this.players)
                {
                    ScreenManager.SpriteBatch.Draw(p.texture, p.rect, Color.White);
                }
            }

            //Draw Board Foreground
            this.ScreenManager.SpriteBatch.Draw(this.ScreenManager.ContentManager.Load<Texture2D>("Content\\Core Game\\boardEdges"), this.board.rect, Color.White);

            if (ScreenManager.gameType == ScreenManager.GameType.SinglePlayer)
            {
                //Draw player avatars
                this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.playerProfile.PictureIndex], this.playerIconPos, Color.White);
                this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.opponentProfile.PictureIndex], this.opponentIconPos, Color.White);

                //Draw player names
                this.ScreenManager.SpriteBatch.DrawString(font, GameState.playerProfile.Name, this.playerNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                this.ScreenManager.SpriteBatch.DrawString(font, GameState.opponentProfile.Name, this.opponentNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            }
            else
            {
                if (UserId == 0)
                {
                    //Draw player avatars
                    this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.playerProfile.PictureIndex], this.playerIconPos, Color.White);
                    this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.opponentProfile.PictureIndex], this.opponentIconPos, Color.White);

                    //Draw player names
                    this.ScreenManager.SpriteBatch.DrawString(font, GameState.playerProfile.Name, this.playerNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                    this.ScreenManager.SpriteBatch.DrawString(font, GameState.opponentProfile.Name, this.opponentNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                }
                else
                {
                    //Draw player avatars
                    this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.playerProfile.PictureIndex], this.opponentIconPos, Color.White);
                    this.ScreenManager.SpriteBatch.Draw(GameState.profilePictures[GameState.opponentProfile.PictureIndex], this.playerIconPos, Color.White);

                    //Draw player names
                    this.ScreenManager.SpriteBatch.DrawString(font, GameState.playerProfile.Name, this.opponentNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                    this.ScreenManager.SpriteBatch.DrawString(font, GameState.opponentProfile.Name, this.playerNamePos, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                }
            }

            //Draw Scorebox
            this.scoreBox.Draw(this.ScreenManager.SpriteBatch);

            //Draw time
            this.ScreenManager.SpriteBatch.DrawString(font, "Time Remaining: " + (int)((gameLength - (elapsedTime)) / 1000), new Vector2(10, 20), Color.Yellow, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            this.ScreenManager.SpriteBatch.End();

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

        private void CheckGameOver() {
            if (UserId == 0)
            {
                // PLAYER IS ON THE LEFT HAND SIDE USING BLUE PADDLE

                if (GameState.playerScore > 2 && gameActive == true)
                {
                    //You Win
                    gameActive = false;
                    GameState.playerProfile.GameWon();
                    ScreenManager.AddScreen(new GameEndOverlay("Game Won", "Congratulations. You won."));
                }
                else if (GameState.opponentScore > 2 && gameActive == true)
                {
                    //You Lose
                    gameActive = false;
                    GameState.playerProfile.GameLost();
                    ScreenManager.AddScreen(new GameEndOverlay("Game Lost", "Sadly, you lost."));
                }
                else if (this.elapsedTime >= this.gameLength && gameActive == true)
                {
                    //You Draw
                    gameActive = false;
                    GameState.playerProfile.GameDrawn();
                    ScreenManager.AddScreen(new GameEndOverlay("Game Drawn", "Time up. You have drawn."));
                }
            }
            else
            {
                //PLAYER IS ON THE RIGHT HAND SIDE USING RED PADDLE

                if (GameState.playerScore > 2 && gameActive == true)
                {
                    //You Lose
                    gameActive = false;
                    GameState.playerProfile.GameLost();
                    ScreenManager.AddScreen(new GameEndOverlay("Game Lost", "Sadly, you lost."));
                }
                else if (GameState.opponentScore > 2 && gameActive == true)
                {
                    //You Lose
                    gameActive = false;
                    GameState.playerProfile.GameWon();
                    ScreenManager.AddScreen(new GameEndOverlay("Game Won", "Congratulations. You won."));
                }
                else if (this.elapsedTime >= this.gameLength && gameActive == true)
                {
                    //You Draw
                    gameActive = false;
                    GameState.playerProfile.GameDrawn();
                    ScreenManager.AddScreen(new GameEndOverlay("Game Drawn", "Time up. You have drawn."));
                }
            }
        }

        private void endGame(string strReason)
        {
            gameActive = false;
            ScreenManager.AddScreen(new GameEndOverlay("Exiting Game", strReason));
        }
    }

}