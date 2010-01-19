using System;
using FMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.Enum;
using MinGH.GameStringImpl;
using MinGH.MiscClasses;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Contains all functionality and data to display a single player session of MinGH.
    /// </summary>
    class SinglePlayerScreen : DrawableGameComponent
    {
        MinGHMain gameReference;  // A reference to the game itself, allows for game state changing.
        GraphicsDeviceManager graphics;
        ChartLocation chartLocation;
        GameConfiguration gameConfiguration;
        SpriteBatch spriteBatch;  // Draws the shapes
        Rectangle viewportRectangle;  // The window itself
        Texture2D backgroundTex;  // The background texture
        SpriteFont gameFont;  // The font the game will use
        Note[,] Notes;  // Will hold every note currently on the screen
        const int maxNotesOnscreen = 50;  // Maximum amount of a single note (i.e. how many reds per frame)
        int noteIterator;  // These iterators are used to keep track of which note to observe next
        const int noteLeftPadding = 196;  // How far from the left the green note is placed in pixels
        const int noteWidth = 86;  // How far each lane is on the background

        // Sprite Sheet Variables
        const int noteSpriteSheetOffset = 6;  // How many pixels pad the left side of a note on the sprite sheet
        const int noteSpriteSheetSize = 100;  // How large each sprite is in the spritesheet (including the offset padding)
        
        // Variables unique to this game screen
        NoteUpdater noteUpdater = new NoteUpdater();
        IKeyboardInputManager keyboardInputManager = new KeyboardInputManager();
        IInputManager inputManager;
        HorizontalHitBox hitBox;
        PlayerInformation playerInformation = new PlayerInformation();

        Chart mainChart;  // Create the chart file
        GameStringManager strManager = new GameStringManager();  // Stores each string and its position on the screen

        // Variables related to the audio playing and note syncing
        private FMOD.System system = new FMOD.System();
        private FMOD.Channel musicChannel = new FMOD.Channel();
        private FMOD.Channel guitarChannel = new FMOD.Channel();
        private FMOD.Channel bassChannel = new FMOD.Channel();
        private FMOD.Channel drumChannel = new FMOD.Channel();
        private FMOD.Sound musicSound = new FMOD.Sound();
        private FMOD.Sound guitarSound = new FMOD.Sound();
        private FMOD.Sound bassSound = new FMOD.Sound();
        private FMOD.Sound drumSound = new FMOD.Sound();
        RESULT result = new RESULT();
        uint currentMsec = 0;
        bool audioIsPlaying = false;  // So we don't play the song again every single update

        // Project Mercury Particle Engine related variables
        NoteParticleExplosionEmitters noteParticleExplosionEmitters = new NoteParticleExplosionEmitters();
        PointSpriteRenderer renderer = new PointSpriteRenderer();
        ColorModifier modifier = new ColorModifier();

        public SinglePlayerScreen(MinGHMain game, GraphicsDeviceManager graph, ChartLocation inputLocation)
            : base(game)
        {
            gameReference = game;
            graphics = graph;
            chartLocation = inputLocation;
        }

        public override void Initialize()
        {
            // Setup the strings
            strManager =  SinglePlayerStringInitializer.initializeStrings(graphics.GraphicsDevice.Viewport.Width,
                                                graphics.GraphicsDevice.Viewport.Height);
            // Initialize some variables
            noteIterator = 0;
            Notes = new Note[5, maxNotesOnscreen];

            // Create the sprite bacth
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            gameConfiguration = new GameConfiguration("./config.xml");

            if (gameConfiguration.useDrumStyleInputForGuitarMode)
            {
                inputManager = new DrumInputManager();
            }
            else
            {
                inputManager = new GuitarInputManager();
            }

            // Create the hitbox
            hitBox = new HorizontalHitBox(new Rectangle(0, 0,
                                          graphics.GraphicsDevice.Viewport.Width,
                                          graphics.GraphicsDevice.Viewport.Height),
                                          gameConfiguration.speedModValue);

            // Set up the particle explosions
            noteParticleExplosionEmitters.initalizeEmitters();
            noteParticleExplosionEmitters.initializeLocations(noteLeftPadding, noteWidth, hitBox.centerLocation);
            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
            {
                emitter.Initialize();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>("Arial");  // Load the font

            switch (chartLocation.chartType)
            {
                case "*.chart":
                    mainChart = new Chart(chartLocation.chartPath, "*.chart");
                    break;
                case "*.mid":
                    mainChart = new Chart(chartLocation.directory, "*.mid");
                    break;
            }

            backgroundTex = Game.Content.Load<Texture2D>("Backgrounds\\GuitarSingle_Background");

            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Green Notes
                            Notes[i, j] = new Note(Game.Content.Load<Texture2D>("Sprites\\My_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note(Game.Content.Load<Texture2D>("Sprites\\My_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note(Game.Content.Load<Texture2D>("Sprites\\My_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note(Game.Content.Load<Texture2D>("Sprites\\My_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 4:  // Orange Notes
                            Notes[i, j] = new Note(Game.Content.Load<Texture2D>("Sprites\\My_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                    }
                    Notes[i,j].velocity = new Vector2(0.0f, (float)gameConfiguration.speedModValue.noteVelocityMultiplier);
                }
            }

            // Add the "Song Title" and "Artist Name" to the string manager
            strManager.SetString(2, "Song Title:\n" + mainChart.chartInfo.songName);
            strManager.SetString(3, "Artist Name:\n" + mainChart.chartInfo.artistName);
            
            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            
            renderer.GraphicsDeviceService = graphics;
            renderer.BlendMode = SpriteBlendMode.Additive;
            renderer.LoadContent(Game.Content);

            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
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
                FMOD.Factory.System_Create(ref system);
                system.init(32, INITFLAGS.NORMAL | INITFLAGS.SOFTWARE_OCCLUSION, (IntPtr)null);
                //uint minDelay = 0, hi = 0, lo = 0;
                //int ass = 0;

                if (mainChart.chartInfo.musicStream != null)
                {
                    string musicLocation = chartLocation.directory + "\\" + mainChart.chartInfo.musicStream;
                    result = system.createSound(musicLocation, MODE.HARDWARE, ref musicSound);
                }
                if (mainChart.chartInfo.guitarStream != null)
                {
                    string guitarLocation = chartLocation.directory + "\\" + mainChart.chartInfo.guitarStream;
                    result = system.createSound(guitarLocation, MODE.HARDWARE, ref guitarSound);
                }
                if (mainChart.chartInfo.bassStream != null)
                {
                    string bassLocation = chartLocation.directory + "\\" + mainChart.chartInfo.bassStream;
                    result = system.createSound(bassLocation, MODE.HARDWARE, ref bassSound);
                }
                if (mainChart.chartInfo.drumStream != null)
                {
                    string drumLocation = chartLocation.directory + "\\" + mainChart.chartInfo.drumStream;
                    result = system.createSound(drumLocation, MODE.HARDWARE, ref drumSound);
                }

                result = system.playSound(CHANNELINDEX.FREE, musicSound, false, ref musicChannel);
                result = system.playSound(CHANNELINDEX.FREE, guitarSound, false, ref guitarChannel);
                result = system.playSound(CHANNELINDEX.FREE, bassSound, false, ref bassChannel);
                result = system.playSound(CHANNELINDEX.FREE, drumSound, false, ref drumChannel);

                // A VERY hackey and uninformed way of syncing the three tracks after playing.
                // Sadly this is the only way that I could get to "work."
                uint bassPositon = 0;
                bassChannel.getPosition(ref bassPositon, TIMEUNIT.MS);
                guitarChannel.setPosition(bassPositon, TIMEUNIT.MS);
                bassChannel.getPosition(ref bassPositon, TIMEUNIT.MS);
                musicChannel.setPosition(bassPositon, TIMEUNIT.MS);
                bassChannel.getPosition(ref bassPositon, TIMEUNIT.MS);
                drumChannel.setPosition(bassPositon, TIMEUNIT.MS);

                audioIsPlaying = true;
            }

            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                UnloadContent();
                gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
            }

            musicChannel.getPosition(ref currentMsec, TIMEUNIT.MS);

            // Update the FMOD system
            system.update();

            // Get the current keyboard state
            keyboardInputManager.processKeyboardState(Keyboard.GetState());

            // The distance each note must step to be in sync with this current update
            float currStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * gameConfiguration.speedModValue.noteVelocityMultiplier);
            
            inputManager.processPlayerInput(Notes, noteParticleExplosionEmitters, hitBox,
                                                  playerInformation, keyboardInputManager,
                                                  mainChart.noteCharts[0]);

            NoteUpdater.updateNotes(mainChart.noteCharts[0], ref noteIterator, Notes, viewportRectangle,
                                    currStep, noteWidth, currentMsec + gameConfiguration.speedModValue.milisecondOffset,
                                    noteSpriteSheetSize, playerInformation, hitBox);

            // Update varous strings
            //strManager.SetString(0, "Current MSEC:\n" + musicLocation);
            strManager.SetString(1, "HOPO?:\n" + mainChart.chartInfo.chartLengthMiliseconds.ToString());
            strManager.SetString(4, "Score: " + playerInformation.currentScore.ToString() + "\n\n" +
                                     "Multiplier : " + playerInformation.currentMultiplier.ToString() + "\n\n" +
                                     "Combo :" + playerInformation.currentCombo.ToString());
            strManager.SetString(5, "Health: " + playerInformation.currentHealth.ToString());

            // Stop playing music when chart is over or when the screen goes inactive
            if (currentMsec > mainChart.chartInfo.chartLengthMiliseconds)
            {
                musicChannel.stop();
                bassChannel.stop();
                guitarChannel.stop();
            }

            // Update every particle explosion
            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
            {
                emitter.Update((float)gameTime.TotalGameTime.TotalSeconds, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //Draw the background
            spriteBatch.Draw(backgroundTex, viewportRectangle, Color.White);

            // Draw every string in str_manager
            strManager.DrawStrings(spriteBatch, gameFont);

            //Draw the notes
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    if (Notes[i, j].alive)
                    {
                        spriteBatch.Draw(Notes[i, j].spriteSheet, Notes[i, j].position, Notes[i, j].spriteSheetRectangle, Color.White);
                    }
                }
            }

            spriteBatch.End();

            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
            {
                renderer.RenderEmitter(emitter);
            }

            base.Draw(gameTime);
        }
    }
}
