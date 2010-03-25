#region File Description

//-----------------------------------------------------------------------------
// ScreenManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework.Input;

#endregion

namespace GameScreenManager.ScreenSystem {
    /// <summary>
    /// The screen manager is a component which manages one or more <see cref="GameScreen"/>
    /// instances. It maintains a stack of _screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes _input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager: DrawableGameComponent {

        #region network variables

        NetServer server;
        NetClient client;
        string strMsg = "";
        private bool blnNetworkConnected;

        public bool blnConnected
        {
            get { return blnNetworkConnected; }
            set { blnNetworkConnected = value; }
        }

        public string strNetworkMsg
        {
            get { return strMsg; }
            set { strMsg = value; }
        }

        private float _networkMouseX;
        private float _networkMouseY;

        public float NetworkMouseX
        {
            get { return _networkMouseX; }
            set { _networkMouseX = value; }
        }

        public float NetworkMouseY
        {
            get { return _networkMouseY; }
            set { _networkMouseY = value; }
        }

        #endregion

        private Texture2D _blankTexture;
        private IGraphicsDeviceService _graphicsDeviceService;
        private InputState _input = new InputState();
        private List<GameScreen> _screens = new List<GameScreen>();
        private List<GameScreen> _screensToUpdate = new List<GameScreen>();
        private SpriteFonts _spriteFonts;
        private bool gameStart;
        private bool addParticles;
        

        public enum GameType {
            SinglePlayer,
            FindGame,
            StartMultiplayer
        }
        public GameType gameType;

        /// <summary>
        /// Specifies the range of particle effects available.
        /// </summary>
        public enum ParticleType {
            Smoke,
            Explosion,
            None
        }

        /// <summary>
        /// Tells AirHockey class which particles to add.
        /// </summary>
        public ParticleType ParticleTypeToAdd { get; set; }

        public SpriteFonts SpriteFonts {
            get { return _spriteFonts; }
        }

        public Vector2 ParticleSourcePosition { get; set; }

        /// <summary>
        /// Returns true if particles are to be added.
        /// </summary>
        public bool AddParticles {
            get { return addParticles; }
            set { addParticles = value; }
        }

        /// <summary>
        /// Returns true if the game should start.
        /// </summary>
        public bool StartTheGame {
            get { return gameStart; }
            set { gameStart = value; }
        }

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        /// <exception cref="InvalidOperationException">No graphics device service.</exception>
        public ScreenManager(Game game)
            : base(game) {
            gameStart = false;

            //Load settings file
            OptionsMenuScreen.currentSettings = Settings.ReadSettingsFile();

            //Creates a settings instance with default variables
            //This is then saved to file
            if(OptionsMenuScreen.currentSettings == null) {
                OptionsMenuScreen.currentSettings = new Settings();
                OptionsMenuScreen.currentSettings.WriteSettingsFile();
            }


            ContentManager = new ContentManager(game.Services);
            _graphicsDeviceService = (IGraphicsDeviceService)game.Services.GetService(
                                                                  typeof(IGraphicsDeviceService));
            game.Exiting += Game_Exiting;

            if(_graphicsDeviceService == null)
                throw new InvalidOperationException("No graphics device service.");
        }

        /// <summary>
        /// ScreenManager can't access AirHockey (Game) methods
        /// So instead, the AirHockey class must poll the ScreenManager
        /// to see if the game is supposed to start.
        /// </summary>
        /// <param name="gameType">'s': single-player, 'm': start multiplayer, 'f': find game</param>
        public void StartGame(char gameType) {
            
            blnConnected = false;
            gameStart = true;

            switch(gameType) {
                case 's': this.gameType = GameType.SinglePlayer;
                    break;
                case 'f': 
                    this.gameType = GameType.FindGame;
                    //SET UP CLIENT
                    NetConfiguration config = new NetConfiguration("T12AirHockey");
                    client = new NetClient(config);
                    client.SetMessageTypeEnabled(NetMessageType.ConnectionRejected, true);
                    client.SetMessageTypeEnabled(NetMessageType.DebugMessage, true);
                    //client.SetMessageTypeEnabled(NetMessageType.VerboseDebugMessage, true);
                    client.Start();

                    // Wait half a second to allow server to start up if run via Visual Studio
                    System.Threading.Thread.Sleep(500);
                    
                    //FIND NETWORK GAME
                    findNetworkGame();
                    break;
                case 'm': 
                    this.gameType = GameType.StartMultiplayer;
                    startNetworkGame();
                    break;
            }
        }

        private void findNetworkGame()
        {
            // Emit discovery signal
            //client.DiscoverLocalServers(14242);
            client.DiscoverKnownServer(Dns.GetHostName(), 14242);

            // create a buffer to read data into
            NetBuffer buffer = client.CreateBuffer();

            // keep running until the user presses a key
            NetMessageType type;

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
                        blnConnected = true;
                        break;
                    case NetMessageType.ConnectionRejected:
                        // strMsg += System.Environment.NewLine + "Rejected: " + buffer.ReadString().ToString();
                        break;
                    case NetMessageType.DebugMessage:
                    case NetMessageType.VerboseDebugMessage:
                        //strMsg += System.Environment.NewLine + "Rejected: " + buffer.ReadString();
                        break;
                    case NetMessageType.StatusChanged:
                        string statusMessage = buffer.ReadString();
                        NetConnectionStatus newStatus = (NetConnectionStatus)buffer.ReadByte();
                        //strMsg += System.Environment.NewLine + "New status: " + newStatus + " (" + statusMessage + ")";
                        break;
                    case NetMessageType.Data:
                        // The server sent this data!
                        string msg = buffer.ReadString();
                        //strMsg += System.Environment.NewLine + msg;
                        break;
                }
            }

        }

        private void startNetworkGame()
        {
            // create a configuration for the server
            NetConfiguration serverConfig = new NetConfiguration("T12AirHockey");
            serverConfig.MaxConnections = 128;
            serverConfig.Port = 14242;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            serverConfig.Address = addr[0];

            // create server and start listening for connections
            server = new NetServer(serverConfig);
            server.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
            server.Start();
            //Allow time for server to start
            System.Threading.Thread.Sleep(1000);
            //Check if the network has started
            if (server.IsListening)
            {
                strMsg = "Server successfully started on port 14242, awaiting users to connect";
            }
            else
            {
                strMsg = "Server not started";
            }
        }
        
        /// <summary>
        /// A content manager used to load data that is shared between multiple
        /// screens. This is never unloaded, so if a screen requires a large amount
        /// of temporary data, it should create a local content manager instead.
        /// </summary>
        public ContentManager ContentManager { get; private set; }

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        public Vector2 ScreenCenter {
            get {
                return new Vector2(_graphicsDeviceService.GraphicsDevice.Viewport.Width / 2f,
                                   _graphicsDeviceService.GraphicsDevice.Viewport.Height / 2f);
            }
        }

        public int ScreenWidth {
            get { return _graphicsDeviceService.GraphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight {
            get { return _graphicsDeviceService.GraphicsDevice.Viewport.Height; }
        }

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled { get; set; }

        /// <summary>
        /// Goes to main menu.
        /// Removes all active screens and add a main menu
        /// </summary>
        public void GoToMainMenu() {
            _screens.Clear();
            _screensToUpdate.Clear();

            AddScreen(new MainMenuScreen());
        }

        /// <summary>
        /// Sets variable to tell ScreenManager to start game.
        /// </summary>
        public void GoToSinglePlayerGame() {
            gameStart = true;
        }

        /// <summary>
        /// ScreenManager needs to let AirHockey to add particles but
        /// cannot access it directly. It therefore sets relevant variables
        /// and the AirHockey class polls it every update.
        /// 
        /// Classes that can access the ScreenManager methods can therefore
        /// indirectly start a particle effect
        /// </summary>
        /// <param name="particleType">'e': explosion, 's': smoke</param>
        public void RequestParticleEffect(char particleType, Vector2 position) {
            addParticles = true;
            switch(particleType) {
                case 'e':
                    ParticleTypeToAdd = ParticleType.Explosion;
                    break;
                case 's':
                    ParticleTypeToAdd = ParticleType.Smoke;
                    break;
            }

            ParticleSourcePosition = position;
        }

        /// <summary>
        /// Sets variable and gametype so that airhockeygame knows which gametype to start
        /// </summary>
        /// <param name="startGame">If true, player creates game. If false player, looks for game.</param>
        public void GoToMultiGameScreen(bool createGame) {
            gameStart = true;

            if(createGame)
                gameType = GameType.StartMultiplayer;
            else
                gameType = GameType.FindGame;
        }

        private void Game_Exiting(object sender, EventArgs e) {
            //Make sure to dispose ALL screens when the game is forcefully closed
            //We do this to ensure that open resources and threads created by screens are closed.
            foreach(GameScreen screen in _screens) {
                screen.Dispose();
            }

            _screens.Clear();
            _screensToUpdate.Clear();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize() {
            _spriteFonts = new SpriteFonts(ContentManager);

            foreach(GameScreen screen in _screens) {
                screen.Initialize();
            }
            base.Initialize();
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent() {
            // Load content belonging to the screen manager.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _blankTexture = ContentManager.Load<Texture2D>("Content/Common/blank");

            // Tell each of the _screens to load their content.
            foreach(GameScreen screen in _screens) {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent() {
            ContentManager.Unload();

            // Tell each of the _screens to unload their content.
            foreach(GameScreen screen in _screens) {
                screen.UnloadContent();
            }
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime) {

            //SERVER UPDATE METHODS
            if (gameType == GameType.StartMultiplayer)
            {
                handleServerUpdate();
            }
            else if (gameType == GameType.FindGame)
            {
                handleClientUpdate();
            }

            // Read the keyboard and gamepad.
            _input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _screensToUpdate.Clear();

            for(int i = 0; i < _screens.Count; i++)
                _screensToUpdate.Add(_screens[i]);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are _screens waiting to be updated.
            while(_screensToUpdate.Count > 0) {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];

                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if(screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active) {
                    // If this is the first active screen we came across,
                    // give it a chance to handle _input.
                    if(!otherScreenHasFocus) {
                        screen.HandleInput(_input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // _screens that they are covered by it.
                    if(!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            // Print debug trace?
            if(TraceEnabled)
                TraceScreens();
        }

        private void handleServerUpdate()
        {
            //Check if connected if not keep listening for incoming connection
            if (!blnConnected)
            {
                //HANDLE NETWORK MESSAGES
                NetBuffer buffer = server.CreateBuffer();
                NetMessageType type;
                NetConnection sender;

                // check if any messages has been received
                while (server.ReadMessage(buffer, out type, out sender))
                {
                    switch (type)
                    {
                        case NetMessageType.ConnectionApproval:
                            sender.Approve();
                            blnConnected = true;
                            break;
                    }
                }
            }
            else
            {
                strMsg = "Successfully connected awaiting commands!";

                //HANDLE NETWORK MESSAGES
                NetBuffer buffer = server.CreateBuffer();
                NetMessageType type;
                NetConnection sender;

                // check if any messages has been received
                while (server.ReadMessage(buffer, out type, out sender))
                {
                    switch (type)
                    {
                        case NetMessageType.Data:
                            // A client sent this data!
                            try
                            {
                                Vector2 v2 = buffer.ReadVector2();
                                NetworkMouseX = v2.X;
                                NetworkMouseY = v2.Y;
                            }
                            catch(Exception ex)
                            {
                            }

                            // send your co-orders to everyone
                            NetBuffer sendBuffer = server.CreateBuffer();
                            sendBuffer.Write(new Vector2(Mouse.GetState().X,Mouse.GetState().Y));
                            server.SendToAll(sendBuffer, NetChannel.ReliableInOrder1);

                            break;
                    }
                }
            }
        }

        private void handleClientUpdate()
        {
            //IF NOT CONNECTED KEEP TRYING TO CONNECT TO SERVER
            if (!blnConnected)
            {
                strMsg = "Could not connect to server, trying again...";
                findNetworkGame();
            }
            else
            {
                //SEND DATA TO THE NETWORK
                strMsg = "connected to server, starting game";
                NetBuffer buffer = client.CreateBuffer();

                buffer.Write(new Vector2(Mouse.GetState().X,Mouse.GetState().Y));
                client.SendMessage(buffer, NetChannel.ReliableUnordered);

                // create a buffer to read data into
                NetBuffer readBuffer = client.CreateBuffer();

                // keep running until the user presses a key
                NetMessageType type;

                // check if any messages has been received
                while (client.ReadMessage(readBuffer, out type))
                {
                    switch (type)
                    {
                        case NetMessageType.Data:
                            // The server sent this data!
                            try
                            {
                                Vector2 v2 = readBuffer.ReadVector2();
                                NetworkMouseX = v2.X;
                                NetworkMouseY = v2.Y;
                            }
                            catch (Exception ex)
                            {
                            } 
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        private void TraceScreens() {
            List<string> screenNames = new List<string>();

            foreach(GameScreen screen in _screens)
                screenNames.Add(screen.GetType().Name);

            Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime) {
            for(int i = 0; i < _screens.Count; i++) {
                if(_screens[i].ScreenState == ScreenState.Hidden)
                    continue;

                _screens[i].Draw(gameTime);
            }

            //DRAW OUT NETWORK MSG FOR TESTING PURPOSES
            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            SpriteBatch.DrawString(this.SpriteFonts.DetailsFont, strMsg, new Vector2(50, 10), Color.White);

            SpriteBatch.End();

        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen) {
            screen.ScreenManager = this;
            screen.Initialize();
            // If we have a graphics device, tell the screen to load content.
            if((_graphicsDeviceService != null) &&
                (_graphicsDeviceService.GraphicsDevice != null)) {
                screen.LoadContent();
            }

            _screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use <see cref="GameScreen"/>.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen) {
            // If we have a graphics device, tell the screen to unload content.
            if((_graphicsDeviceService != null) &&
                (_graphicsDeviceService.GraphicsDevice != null)) {
                screen.UnloadContent();
            }

            _screens.Remove(screen);
            _screensToUpdate.Remove(screen);

            screen.Dispose();
        }

        /// <summary>
        /// Helper draws a translucent black full screen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeBackBufferToBlack(int alpha) {
            Viewport viewport = GraphicsDevice.Viewport;

            SpriteBatch.Begin();

            SpriteBatch.Draw(_blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             new Color(0, 0, 0, (byte)alpha));

            SpriteBatch.End();
        }
    }
}