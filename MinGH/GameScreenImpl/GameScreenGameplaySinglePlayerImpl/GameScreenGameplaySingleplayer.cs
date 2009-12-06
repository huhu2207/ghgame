using System;
using FMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl;
using MinGH.GameStringImpl;
using MinGH.Misc_Classes;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace MinGH.GameScreenImpl
{
    /// <summary>
    /// Contains all functionality and data to display a single player session of MinGH
    /// </summary>
    class GameScreenGameplaySingleplayer : GameScreen
    {
        // User variables
        int hyperSpeedSetting = 0;  // Selects the speed in which the notes come
        int milisecondOffsetFineTune = 0;  // Use to adjust the timing of the notes

        SpriteBatch spriteBatch;  // Draws the shapes
        Rectangle viewportRectangle;  // The window itself
        Texture2D backgroundTex;  // The background texture
        SpriteFont game_font;  // The font the game will use
        Note[,] Notes;  // Will hold every note currently on the screen
        const int maxNotesOnscreen = 20;  // Maximum amount of a single note (i.e. how many reds per frame)
        HyperSpeedList hyperSpeedList = new HyperSpeedList();
        int noteIterator;  // These iterators are used to keep track of which note to observe next
        const int noteLeftPadding = 196;  // How far from the left the green note is placed in pixels
        const int noteWidth = 86;  // How far each lane is on the background
        double currentNoteVelocityMultiplier = 0.0;  // Used to calculate the speed of a note
        int currentMilisecondOffset = 0;  // The number of miliseconds a note must come early

        // Sprite Sheet Variables
        const int noteSpriteSheetOffset = 6;  // How many pixels pad the left side of a note on the sprite sheet
        const int noteSpriteSheetSize = 99;  // How large each sprite is in the spritesheet (including the offset padding)
        
        // Variables unique to this game screen
        NoteUpdater noteUpdater = new NoteUpdater();
        IKeyboardInputManager keyboardInputManager = new SinglePlayerKeyboardManager();
        HorizontalHitBox hitBox;
        PlayerInformation playerInformation = new PlayerInformation();
        KeyboardConfiguration keyboardConfig = new KeyboardConfiguration();

        Chart mainChart;  // Create the chart file
        GameStringManager strManager = new GameStringManager();  // Stores each string and its position on the screen

        // Variables related to the audio playing and note syncing
        private FMOD.System system = new FMOD.System();
        private FMOD.Channel musicChannel = new FMOD.Channel();
        private FMOD.Channel tickChannel = new FMOD.Channel();
        private FMOD.Sound musicSound = new FMOD.Sound();
        private FMOD.Sound tickSound = new FMOD.Sound();
        RESULT result = new RESULT();
        uint currentMsec = 0;
        bool audioIsPlaying = false;  // So we don't play the song again every single update
        bool useAudioTicker = true;

        // Project Mercury Particle Engine related variables
        NoteParticleExplosionEmitters noteParticleExplosionEmitters = new NoteParticleExplosionEmitters();
        PointSpriteRenderer renderer = new PointSpriteRenderer();
        ColorModifier modifier = new ColorModifier();

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            gameScreenType = GameScreenEnum.GameplaySinglePlayer;
            isActive = true;

            // Setup the strings
            SinglePlayerStringInitializer.initializeStrings(ref strManager, graphics.GraphicsDevice.Viewport.Width,
                               graphics.GraphicsDevice.Viewport.Height);
            // Initialize some variables
            noteIterator = 0;
            Notes = new Note[5, maxNotesOnscreen];

            // Create the sprite bacth
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            // Setup the note velocity constant (HYPERSPEEDS!!)
            if ((hyperSpeedSetting < 0) || (hyperSpeedSetting > hyperSpeedList.theList.Count))
            {
                hyperSpeedSetting = 0;
            }
            currentMilisecondOffset = hyperSpeedList.theList[hyperSpeedSetting].milisecondOffset +
                                      milisecondOffsetFineTune;
            currentNoteVelocityMultiplier = hyperSpeedList.theList[hyperSpeedSetting].noteVelocityMultiplier;

            // Create the hitbox
            hitBox = new HorizontalHitBox(new Rectangle(0, 0,
                                          graphics.GraphicsDevice.Viewport.Width,
                                          graphics.GraphicsDevice.Viewport.Height));

            // Set up the particle explosions
            noteParticleExplosionEmitters.initalizeEmitters();
            noteParticleExplosionEmitters.initializeLocations(noteLeftPadding, noteWidth, hitBox.centerLocation);
            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
            {
                emitter.Initialize();
            }
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            game_font = content.Load<SpriteFont>("Arial");  // Load the font
            mainChart = new Chart("chart.chart");  // Setup the chart
            backgroundTex = content.Load<Texture2D>("Backgrounds\\GH_Background");

            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Green Notes
                            Notes[i, j] = new Note(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                        case 4:  // Orange Notes
                            Notes[i, j] = new Note(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), -noteSpriteSheetOffset);
                            break;
                    }
                    Notes[i,j].velocity = new Vector2(0.0f, (float)currentNoteVelocityMultiplier);
                }
            }

            // Add the "Song Title" and "Artist Name" to the string manager
            strManager.Set_String(2, "Song Title:\n" + mainChart.chartInfo.songName);
            strManager.Set_String(3, "Artist Name:\n" + mainChart.chartInfo.artistName);
            
            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            
            renderer.GraphicsDeviceService = graphics;
            renderer.BlendMode = SpriteBlendMode.Additive;
            renderer.LoadContent(content);

            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
            {
                emitter.LoadContent(content);
            }
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Start the song immediately
            if (audioIsPlaying == false)
            {
                //audio_engine.Play2D("guitar.ogg", true);
                FMOD.Factory.System_Create(ref system);
                system.init(32, INITFLAGS.NORMAL, (IntPtr)null);
                system.createSound("./guitar.ogg", MODE.HARDWARE, ref musicSound);
                system.playSound(CHANNELINDEX.FREE, musicSound, false, ref musicChannel);
                audioIsPlaying = true;
                system.createSound("./tick.wav", MODE.HARDWARE, ref tickSound);
            }

            musicChannel.getPosition(ref currentMsec, TIMEUNIT.MS);

            string bla = "...";

            // Update audio ticker
            if (useAudioTicker)
            {
                for (int i = 0; i < Notes.GetLength(0); i++)
                {
                    for (int j = 0; j < Notes.GetLength(1); j++)
                    {
                        if ((!Notes[i, j].wasTicked) && (Notes[i, j].getCenterPosition().Y >= hitBox.centerLocation))
                        {
                            result = system.playSound(CHANNELINDEX.FREE, tickSound, false, ref tickChannel);
                            Notes[i, j].wasTicked = true;
                            bla = "YEA!!";
                        }
                    }
                }
            }

            // Update the FMOD system
            system.update();

            // Get the current keyboard state
            keyboardInputManager.processKeyboardState(Keyboard.GetState());
            
            PlayerInputManager.processPlayerInput(Notes, noteParticleExplosionEmitters, hitBox,
                                                  playerInformation, keyboardInputManager,
                                                  mainChart.noteCharts[0]);

            NoteUpdater.updateNotes(mainChart.noteCharts[0], ref noteIterator, Notes, viewportRectangle,
                                    gameTime, currentNoteVelocityMultiplier, noteWidth, currentMsec + currentMilisecondOffset,
                                    noteSpriteSheetSize, playerInformation, hitBox);

            // Update varous strings
            strManager.Set_String(0, "Current MSEC:\n" + bla);
            strManager.Set_String(1, "HOPO?:\n" + Convert.ToString(playerInformation.HOPOState));
            strManager.Set_String(4, "Score: " + playerInformation.currentScore.ToString() + "\n\n" +
                                     "Multiplier : " + playerInformation.currentMultiplier.ToString() + "\n\n" +
                                     "Combo :" + playerInformation.currentCombo.ToString());
            strManager.Set_String(5, "Health: " + playerInformation.currentHealth.ToString());

            // Stop playing music when chart is over or when the screen goes inactive
            if ((currentMsec > mainChart.chartInfo.chartLengthMiliseconds) ||
                (!isActive))
            {
                musicChannel.stop();
            }

            // Update every particle explosion
            foreach (Emitter emitter in noteParticleExplosionEmitters.emitterList)
            {
                emitter.Update((float)gameTime.TotalGameTime.TotalSeconds, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //Draw the background
            spriteBatch.Draw(backgroundTex, viewportRectangle, Color.White);

            // Draw every string in str_manager
            strManager.DrawStrings(spriteBatch, game_font);

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
        }
    }
}
