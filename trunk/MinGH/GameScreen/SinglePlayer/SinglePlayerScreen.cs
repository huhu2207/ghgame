using System;
using System.Collections.Generic;
using System.Threading;
using GameEngine.FMOD;
using GameEngine.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.EngineExtensions;
using MinGH.Enum;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Contains all functionality and data to display a single player session of MinGH.
    /// </summary>
    class SinglePlayerScreen : DrawableGameComponent
    {
        const int maxNotesOnscreen = 50;  // Maximum amount of a single note (i.e. how many reds per frame)
        const int noteSpriteSheetSize = 100;  // How large each sprite is in the spritesheet (including the offset padding)

        MinGHMain gameReference;  // A reference to the game itself, allows for game state changing.
        GraphicsDeviceManager graphics;
        ChartSelection chartSelection;
        GameConfiguration gameConfiguration;
        SpriteBatch spriteBatch;  // Draws the shapes
        Rectangle viewportRectangle;  // The window itself
        Texture2D backgroundTex, spriteSheetTex, fretboardTex;
        SpriteFont gameFont;  // The font the game will use
        Note3D[,] notes;  // Will hold every note currently on the screen
        List<Fretboard3D> fretboards;  // A set of fretboards aligned next to each other giving a continous effect
        LaneSeparators laneSeparators;
        FretboardBorders fretboardBorders;
        int noteIterator;  // This iterator is used to keep track of which note to draw next
        float noteScaleValue, bassNoteScaleValue;
        NoteUpdater noteUpdater;
        IKeyboardInputManager keyboardInputManager;
        IInputManager inputManager;
        HorizontalHitBox hitBox;
        PlayerInformation playerInformation;
        Chart mainChart;  // Create the chart file
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
        uint currentMsec;
        bool audioIsPlaying;  // So we don't play the song again every single update

        // Project Mercury Particle Engine related variables
        NoteParticleEmitters noteParticleEmitters;
        PointSpriteRenderer renderer;

        public SinglePlayerScreen(MinGHMain game, GraphicsDeviceManager graph, ChartSelection inputLocation)
            : base(game)
        {
            gameReference = game;
            graphics = graph;
            chartSelection = inputLocation;
        }

        public override void Initialize()
        {
            
            // Initialize some variables
            renderer = new PointSpriteRenderer();
            noteParticleEmitters = new NoteParticleEmitters();
            playerInformation = new PlayerInformation();
            strManager = new GameStringManager();
            keyboardInputManager = new KeyboardInputManager();
            noteUpdater = new NoteUpdater();
            noteIterator = 0;
            currentMsec = 0;
            noteScaleValue = 0.0f;
            bassNoteScaleValue = 0.0f;
            notes = new Note3D[5, maxNotesOnscreen];
            effect = gameReference.Content.Load<Effect>("effects");
            texturedVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionTexture.VertexElements);
            fretboards = new List<Fretboard3D>();

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

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            gameConfiguration = new GameConfiguration("./config.xml");
            noteScaleValue = gameConfiguration.themeSetting.laneSize / (float)noteSpriteSheetSize;
            bassNoteScaleValue = (gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) / 
                                 ((float)noteSpriteSheetSize);

            hitBox = new HorizontalHitBox(new Rectangle(0, 0,
                                          graphics.GraphicsDevice.Viewport.Width,
                                          graphics.GraphicsDevice.Viewport.Height),
                                          gameConfiguration.speedModValue);

            strManager = SinglePlayerStringInitializer.initializeStrings(graphics.GraphicsDevice.Viewport.Width,
                                                graphics.GraphicsDevice.Viewport.Height);
            float cameraX = (gameConfiguration.themeSetting.laneSize * 2) +
                            (gameConfiguration.themeSetting.laneBorderSize * 3) +
                            (gameConfiguration.themeSetting.laneSize / 2);

            cameraPostion = new Vector3(cameraX, 170.0f, 0.0f);
            cameraLookAt = new Vector3(cameraX, 50.0f, -300.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPostion, cameraLookAt, new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 0.2f, 1000.0f);

            Texture2D laneSeparatorTexture = Texture2D.FromFile(graphics.GraphicsDevice, ".\\Content\\Fretboards\\LaneSeparatorDefault.png");
            laneSeparators = new LaneSeparators(gameConfiguration.themeSetting.laneSize, gameConfiguration.themeSetting.laneBorderSize, effect,
                                                laneSeparatorTexture, graphics.GraphicsDevice);
            fretboardBorders = new FretboardBorders(gameConfiguration.themeSetting.laneSize, gameConfiguration.themeSetting.laneBorderSize, effect,
                                                    laneSeparatorTexture, graphics.GraphicsDevice, 6);
                
            base.Initialize();
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>("Arial");  // Load the font
            mainChart = new Chart(chartSelection);

            string backgroundFilename = "";
            spriteSheetTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.noteSkinFile);

            if (mainChart.noteCharts[0].instrument == "Drums")
            {
                // Set up the particle explosions
                noteParticleEmitters.initalizeEmittersDrumsSingle(gameConfiguration.themeSetting);
                noteParticleEmitters.initializeLocationsDrumsSingle(gameConfiguration.themeSetting, hitBox.centerLocation);
                backgroundFilename = "DrumsSingle.png";
                //Notes = NoteInitializer.InitializeNotesDrumSingle(noteSpriteSheetSize, Notes, spriteSheetTex, gameConfiguration, noteScaleValue, bassNoteScaleValue);
                //inputManager = new DrumInputManager();
            }
            else  // A guitar background and emitter setting will be the "default"
            {
                // Set up the particle explosions
                noteParticleEmitters.initalizeEmittersGuitarSingle();
                noteParticleEmitters.initializeLocationsGuitarSingle(gameConfiguration.themeSetting, hitBox.centerLocation);
                backgroundFilename = "GuitarSingle.png";
                notes = NoteInitializer.InitializeNotesGuitarSingle(noteSpriteSheetSize, notes, spriteSheetTex, gameConfiguration, noteScaleValue, effect, graphics.GraphicsDevice);
                if (gameConfiguration.useDrumStyleInputForGuitarMode)
                {
                    //inputManager = new DrumInputManager();
                }
                else
                {
                    inputManager = new GuitarInputManager();
                }
            }

            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                emitter.Initialize();
            }

            backgroundTex = Texture2D.FromFile(graphics.GraphicsDevice,
                                gameConfiguration.themeSetting.backgroundDirectory + "\\" + backgroundFilename);
            fretboardTex = Texture2D.FromFile(graphics.GraphicsDevice, ".\\Content\\Fretboards\\FretboardDefault.png");

            // Add the "Song Title" and "Artist Name" to the string manager
            string songInformation = "Song Title:\n" + mainChart.chartInfo.songName + "\n\n" +
                                     "Artist Name:\n" + mainChart.chartInfo.artistName + "\n\n" +
                                     "Instrument:\n" + mainChart.noteCharts[0].instrument + "\n\n" +
                                     "Difficulty:\n" + mainChart.noteCharts[0].difficulty;
            strManager.SetString(2, songInformation);
            
            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            
            renderer.GraphicsDeviceService = graphics;
            renderer.BlendMode = SpriteBlendMode.Additive;
            renderer.LoadContent(Game.Content);

            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                emitter.LoadContent(Game.Content);
            }

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            system.close();
        }

        public override void Update(GameTime gameTime)
        {
            // Start the song immediately
            if (audioIsPlaying == false)
            {
                GameEngine.FMOD.Factory.System_Create(ref system);
                system.init(32, INITFLAGS.NORMAL, (IntPtr)null);

                AudioInitializer.InitaliazeAudio(system, chartSelection, mainChart, result, musicChannel,
                                                 bassChannel, guitarChannel, drumChannel, musicStream,
                                                 bassStream, guitarStream, drumStream);
                audioIsPlaying = true;
            }

            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                UnloadContent();
                gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
            }

            // Since some charts use the gutar stream as the main music stream,
            // I update from the guitar channel if no music stream is present.
            if (mainChart.chartInfo.musicStream != null)
            {
                musicChannel.getPosition(ref currentMsec, TIMEUNIT.MS);
            }
            else
            {
                guitarChannel.getPosition(ref currentMsec, TIMEUNIT.MS);
            }

            // Update the FMOD system
            system.update();

            // Get the current keyboard state
            keyboardInputManager.processKeyboardState(Keyboard.GetState());

            // The distance each note must step to be in sync with this current update
            float currStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * gameConfiguration.speedModValue.noteVelocityMultiplier);
            
            inputManager.processPlayerInput(notes, noteParticleEmitters, hitBox,
                                                  playerInformation, keyboardInputManager,
                                                  mainChart.noteCharts[0]);

            NoteUpdater.updateNotes(mainChart.noteCharts[0], ref noteIterator, notes, viewportRectangle,
                                    currStep, gameConfiguration,
                                    currentMsec + gameConfiguration.speedModValue.milisecondOffset,
                                    noteSpriteSheetSize, playerInformation, hitBox);

            FretboardUpdater.UpdateFretboards(fretboards, fretboardTex, effect, graphics.GraphicsDevice,
                                              gameConfiguration, currStep);

            // Update varous strings
            strManager.SetString(0, "Hitbox Y: " + hitBox.physicalHitbox.Y + "\nHitbox Height: " + hitBox.physicalHitbox.Height);
            strManager.SetString(1, "End MSEC:\n" + mainChart.chartInfo.chartLengthMiliseconds.ToString());
            strManager.SetString(3, "Score: " + playerInformation.currentScore.ToString() + "\n\n" +
                                     "Multiplier : " + playerInformation.currentMultiplier.ToString() + "\n\n" +
                                     "Combo :" + playerInformation.currentCombo.ToString());
            strManager.SetString(4, "Health: " + playerInformation.currentHealth.ToString());

            // Stop playing music when chart is over or when the screen goes inactive
            if (currentMsec > mainChart.chartInfo.chartLengthMiliseconds)
            {
                if (mainChart.chartInfo.musicStream != null)
                {
                    musicChannel.stop();
                }
                if (mainChart.chartInfo.bassStream != null)
                {
                    bassChannel.stop();
                }
                if (mainChart.chartInfo.guitarStream != null)
                {
                    guitarChannel.stop();
                }
                if (mainChart.chartInfo.drumStream != null)
                {
                    drumChannel.stop();
                }
            }

            // Update every particle explosion
            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                emitter.Update((float)gameTime.TotalGameTime.TotalSeconds, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //Draw the background
            spriteBatch.Draw(backgroundTex, viewportRectangle, Color.White);

            // Draw every string in str_manager
            strManager.DrawStrings(spriteBatch, gameFont);

            spriteBatch.End();

            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                renderer.RenderEmitter(emitter);
            }

            foreach (Fretboard3D fretboard in fretboards)
            {
                if (fretboard.alive)
                {
                    fretboard.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
                }
            }

            laneSeparators.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            fretboardBorders.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);

            //Draw the notes
            for (int i = 0; i < notes.GetLength(0); i++)
            {
                for (int j = 0; j < notes.GetLength(1); j++)
                {
                    if (notes[i, j].alive)
                    {
                        notes[i, j].draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
                    }
                }
            }

            base.Draw(gameTime);
        }
    }
}
