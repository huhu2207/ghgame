using System;
using GameEngine.FMOD;
using GameEngine.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Config;
using MinGH.Enum;
using MinGH.Fretboard;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Contains all functionality and data to display a single player session of MinGH.
    /// </summary>
    class SinglePlayerScreen : DrawableGameComponent
    {

        const int maxNotesOnscreen = 500;  // Maximum amount of a single note (i.e. how many reds per frame)
        const int noteSpriteSheetSize = 100;  // How large each sprite is in the spritesheet (including the offset padding)

        IFretboard fretboard;
        MinGHMain gameReference;  // A reference to the game itself, allows for game state changing.
        GraphicsDeviceManager graphics;
        ChartSelection chartSelection;
        GameConfiguration gameConfiguration;
        SpriteBatch spriteBatch;  // Draws the shapes
        Rectangle viewportRectangle;  // The window itself
        Texture2D backgroundTex;// spriteSheetTex, fretboardTex;
        SpriteFont gameFont;  // The font the game will use
        IKeyboardInputManager keyboardInputManager;
        GameStringManager strManager;  // Stores each string and its position on the screen
        Vector3 cameraPostion, cameraLookAt;
        Matrix viewMatrix, projectionMatrix;
        VertexDeclaration texturedVertexDeclaration;
        Effect effect;

        // Variables related to the audio playing and note syncing
        private GameEngine.FMOD.System system;
        private GameEngine.FMOD.Channel musicChannel;
        private GameEngine.FMOD.Channel guitarChannel;
        private GameEngine.FMOD.Channel bassChannel;
        private GameEngine.FMOD.Channel drumChannel;
        private GameEngine.FMOD.Sound musicStream;
        private GameEngine.FMOD.Sound guitarStream;
        private GameEngine.FMOD.Sound bassStream;
        private GameEngine.FMOD.Sound drumStream;
        RESULT result;
        uint currentMsec;  // The current place of the audio stream the game will follow
        bool audioIsPlaying;  // So we don't play the song again every single update

        public SinglePlayerScreen(MinGHMain game, GraphicsDeviceManager graph, ChartSelection inputLocation)
            : base(game)
        {
            gameReference = game;
            graphics = graph;
            chartSelection = inputLocation;
        }

        public override void Initialize()
        {
            strManager = new GameStringManager();
            keyboardInputManager = new KeyboardInputManager();
            currentMsec = 0;
            effect = gameReference.Content.Load<Effect>("effects");
            texturedVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionTexture.VertexElements);
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            gameConfiguration = new GameConfiguration("./config.xml");
            strManager = SinglePlayerStringInitializer.initializeStrings(graphics.GraphicsDevice.Viewport.Width,
                                                graphics.GraphicsDevice.Viewport.Height);
            
            // Initialize FMOD variables
            system = new GameEngine.FMOD.System();
            musicChannel = new GameEngine.FMOD.Channel();
            guitarChannel = new GameEngine.FMOD.Channel();
            bassChannel = new GameEngine.FMOD.Channel();
            drumChannel = new GameEngine.FMOD.Channel();
            musicStream = new GameEngine.FMOD.Sound();
            guitarStream = new GameEngine.FMOD.Sound();
            bassStream = new GameEngine.FMOD.Sound();
            drumStream = new GameEngine.FMOD.Sound();
            result = new RESULT();
            audioIsPlaying = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>("Arial");  // Load the font

            string backgroundFilename = "";
            Texture2D laneSeparatorTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.laneSeparatorTexture);
            Texture2D hitMarkerTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.hitMarkerTexture);

            if (chartSelection.instrument == "Drums")
            {
                fretboard = new DrumFretboard(maxNotesOnscreen, gameConfiguration, chartSelection);

                float cameraX = (gameConfiguration.themeSetting.laneSizeDrums * 2) +
                                (gameConfiguration.themeSetting.laneSeparatorSize * 1) +
                                (gameConfiguration.themeSetting.laneSeparatorSize / 2);
                cameraPostion = new Vector3(cameraX, 170.0f, 0.0f);
                cameraLookAt = new Vector3(cameraX, 50.0f, -300.0f);
                viewMatrix = Matrix.CreateLookAt(cameraPostion, cameraLookAt, new Vector3(0, 1, 0));
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 0.2f, gameConfiguration.themeSetting.fretboardDepth);

                // Set up the particle explosions
                backgroundFilename = "DrumsSingle.png";
            }
            else if (chartSelection.instrument == "Single" ||
                     chartSelection.instrument == "DoubleGuitar"||
                     chartSelection.instrument == "DoubleBass")
            {
                fretboard = new GuitarFretboard(maxNotesOnscreen, gameConfiguration, chartSelection);

                float cameraX = (gameConfiguration.themeSetting.laneSizeGuitar * 2) +
                                (gameConfiguration.themeSetting.laneSeparatorSize * 3) +
                                (gameConfiguration.themeSetting.laneSizeGuitar / 2);
                cameraPostion = new Vector3(cameraX, 170.0f, 0.0f);
                cameraLookAt = new Vector3(cameraX, 50.0f, -300.0f);
                viewMatrix = Matrix.CreateLookAt(cameraPostion, cameraLookAt, new Vector3(0, 1, 0));
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 0.2f, gameConfiguration.themeSetting.fretboardDepth);

                // Set up the particle explosions
                backgroundFilename = "GuitarSingle.png";

            }
            else  // Quit out if an invalid chart type is used
            {
                // WHY DOESNT THIS WORK?!?
                gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
            }


            backgroundTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.backgroundDirectory + "\\" + backgroundFilename);

            // Add the "Song Title" and "Artist Name" to the string manager
            string songInformation = "Song Title:\n" + fretboard.getChartInfo().songName + "\n\n" +
                                     "Artist Name:\n" + fretboard.getChartInfo().artistName + "\n\n" +
                                     "Instrument:\n" + chartSelection.instrument + "\n\n" +
                                     "Difficulty:\n" + chartSelection.difficulty;
            strManager.SetString(2, songInformation);
            
            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            fretboard.loadContent(gameConfiguration, laneSeparatorTexture, hitMarkerTexture, effect,
                                        viewMatrix, projectionMatrix, noteSpriteSheetSize, graphics, Game);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            system.close();

            fretboard.unloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Start the song immediately
            if (audioIsPlaying == false)
            {
                GameEngine.FMOD.Factory.System_Create(ref system);
                system.init(32, INITFLAGS.NORMAL, (IntPtr)null);

                AudioInitializer.InitaliazeAudio(system, chartSelection, fretboard.getChartInfo(), result,
                                                 musicChannel, bassChannel, guitarChannel, drumChannel,
                                                 musicStream, bassStream, guitarStream, drumStream);
                audioIsPlaying = true;
            }

            // Update the FMOD system
            system.update();

            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                UnloadContent();
                gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
            }

            // Since some charts use the gutar stream as the main music stream,
            // I update from the guitar channel if no music stream is present.
            if (fretboard.getChartInfo().musicStream != null)
            {
                musicChannel.getPosition(ref currentMsec, TIMEUNIT.MS);
            }
            else
            {
                guitarChannel.getPosition(ref currentMsec, TIMEUNIT.MS);
            }

            // Get the current keyboard state
            keyboardInputManager.processKeyboardState(Keyboard.GetState());

            // Update varous strings
            uint guitarPosition = 0, bassPosition = 0, drumPosition = 0, musicPosition = 0;
            musicChannel.getPosition(ref musicPosition, TIMEUNIT.MS);
            guitarChannel.getPosition(ref guitarPosition, TIMEUNIT.MS);
            bassChannel.getPosition(ref bassPosition, TIMEUNIT.MS);
            drumChannel.getPosition(ref drumPosition, TIMEUNIT.MS);

            string times = "Music: " + currentMsec + "\n" +
                           "Guitar: " + guitarPosition + "\n" +
                           "Bass: " + bassPosition + "\n" +
                           "Drum: " + drumPosition;
            strManager.SetString(0, "TEST");
            strManager.SetString(1, times);
            strManager.SetString(3, "Score: " + fretboard.getPlayerInfo().currentScore.ToString() + "\n\n" +
                                     "Multiplier : " + fretboard.getPlayerInfo().currentMultiplier.ToString() + "\n\n" +
                                     "Combo :" + fretboard.getPlayerInfo().currentCombo.ToString());
            strManager.SetString(4, "Health: " + fretboard.getPlayerInfo().currentHealth.ToString());

            
            //if (currentGrade.X == 1)  // Will not be 1 unless a note was hit and inside the hitbox
            //{
            //    string gradeString = "Early <----> Late\n";
                
            //    // Top Good
            //    if (currentGrade.Y < -hitBox.greatThreshold)
            //    {
            //        gradeString += "<----";
            //    }
                
            //    // Top Great
            //    else if (currentGrade.Y < -hitBox.perfectThreshold)
            //    {
            //        gradeString += "<--";
            //    }

            //    // Perfect
            //    else if (currentGrade.Y < hitBox.perfectThreshold)
            //    {
            //        gradeString += "(^_^)";
            //    }

            //    // Bottom Great
            //    else if (currentGrade.Y < hitBox.greatThreshold)
            //    {
            //        gradeString += "-->";
            //    }

            //    // Bottom Good
            //    else
            //    {
            //        gradeString += "---->";
            //    }
            //    strManager.SetString(5, gradeString);
            //}
            
            // Ensure the audio tracks are within 50 MS accuracy
            if ((((int)guitarPosition - (int)currentMsec > 50) ||
                ((int)guitarPosition - (int)currentMsec < -50)) &&
                (fretboard.getChartInfo().guitarStream != null))
            {
                guitarChannel.setPosition(musicPosition, TIMEUNIT.MS);
            }
            if ((((int)bassPosition - (int)currentMsec > 50) ||
                ((int)bassPosition - (int)currentMsec < -50)) &&
                (fretboard.getChartInfo().bassStream != null))
            {
                bassChannel.setPosition(musicPosition, TIMEUNIT.MS);
            }
            if ((((int)drumPosition - (int)currentMsec > 50) ||
                ((int)drumPosition - (int)currentMsec < -50)) &&
                (fretboard.getChartInfo().drumStream != null))
            {
                drumChannel.setPosition(musicPosition, TIMEUNIT.MS);
            }

            // Stop playing music when chart is over or when the screen goes inactive
            if (currentMsec > fretboard.getChartInfo().chartLengthMiliseconds)
            {
                if (fretboard.getChartInfo().musicStream != null)
                {
                    musicChannel.stop();
                }
                if (fretboard.getChartInfo().bassStream != null)
                {
                    bassChannel.stop();
                }
                if (fretboard.getChartInfo().guitarStream != null)
                {
                    guitarChannel.stop();
                }
                if (fretboard.getChartInfo().drumStream != null)
                {
                    drumChannel.stop();
                }
            }

            fretboard.update(keyboardInputManager, viewportRectangle, gameConfiguration, effect,
                             currentMsec, graphics, noteSpriteSheetSize, gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //Draw the background
            spriteBatch.Draw(backgroundTex, viewportRectangle, Color.White);

            // Draw every string in str_manager
            strManager.draw(spriteBatch, gameFont, viewportRectangle);

            spriteBatch.End();

            fretboard.draw(graphics, viewMatrix, projectionMatrix);

            base.Draw(gameTime);
        }
    }
}
