using System;
using System.Collections.Generic;
using GameEngine.FMOD;
using GameEngine.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.EngineExtensions;
using MinGH.Enum;
using MinGH.Interfaces;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;
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
        Texture2D backgroundTex, spriteSheetTex, fretboardTex;
        SpriteFont gameFont;  // The font the game will use
        Note[,] notes;  // Will hold every note currently on the screen
        List<FretboardBackground> fretboards;  // A set of fretboards aligned next to each other giving a continous effect
        LaneSeparator laneSeparators;
        FretboardBorder fretboardBorders;
        HitMarker hitMarker;
        int noteIterator;  // This iterator is used to keep track of which note to draw next
        float noteScaleValue, bassNoteScaleValue;
        IKeyboardInputManager keyboardInputManager;
        IInputManager inputManager;
        INoteUpdater noteUpdater;
        HorizontalHitBox hitBox;
        PlayerInformation playerInformation;
        Chart mainChart;  // Create the chart file
        GameStringManager strManager;  // Stores each string and its position on the screen
        Vector3 cameraPostion, cameraLookAt;
        Matrix viewMatrix, projectionMatrix;
        VertexDeclaration texturedVertexDeclaration;
        Effect effect;
        float distanceFromNoteStartToHitmarker;
        float currStepPerMilisecond; // How many game space units a note must move per milisecond
        Point currentGrade; // The grade of the last hit note (how close it was to the center of the hitbox)

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
            noteIterator = 0;
            currentMsec = 0;
            noteScaleValue = 0.0f;
            bassNoteScaleValue = 0.0f;
            notes = new Note[5, maxNotesOnscreen];
            effect = gameReference.Content.Load<Effect>("effects");
            texturedVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionTexture.VertexElements);
            fretboards = new List<FretboardBackground>();
            currentGrade = new Point();
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            gameConfiguration = new GameConfiguration("./config.xml");
            hitBox = new HorizontalHitBox((int)(gameConfiguration.themeSetting.hitMarkerDepth + (gameConfiguration.themeSetting.hitMarkerSize / 2.0f)),
                                          gameConfiguration.MSTillHit);
            distanceFromNoteStartToHitmarker = gameConfiguration.themeSetting.fretboardDepth - hitBox.centerLocation;
            currStepPerMilisecond = distanceFromNoteStartToHitmarker / gameConfiguration.MSTillHit;
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

            if (gameConfiguration.autoplay)
            {
                noteUpdater = new AutoplayNoteUpdater();
            }
            else
            {
                noteUpdater = new NoteUpdater();
            }

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>("Arial");  // Load the font
            mainChart = new Chart(chartSelection);

            string backgroundFilename = "";
            spriteSheetTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.noteSkinTexture);
            Texture2D laneSeparatorTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.laneSeparatorTexture);
            Texture2D hitMarkerTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.hitMarkerTexture);

            if (mainChart.noteCharts[0].instrument == "Drums")
            {
                fretboard = new DrumFretboard(maxNotesOnscreen, gameConfiguration);

                float cameraX = (gameConfiguration.themeSetting.laneSizeDrums * 2) +
                                (gameConfiguration.themeSetting.laneSeparatorSize * 1) +
                                (gameConfiguration.themeSetting.laneSeparatorSize / 2);
                cameraPostion = new Vector3(cameraX, 170.0f, 0.0f);
                cameraLookAt = new Vector3(cameraX, 50.0f, -300.0f);
                viewMatrix = Matrix.CreateLookAt(cameraPostion, cameraLookAt, new Vector3(0, 1, 0));
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 0.2f, gameConfiguration.themeSetting.fretboardDepth);

                // Set up the particle explosions
                noteParticleEmitters.initalizeEmittersDrumsSingle(gameConfiguration.themeSetting, graphics.GraphicsDevice, viewMatrix, projectionMatrix);
                noteParticleEmitters.initializeLocationsDrumsSingle(gameConfiguration.themeSetting, graphics.GraphicsDevice, viewMatrix, projectionMatrix);
                backgroundFilename = "DrumsSingle.png";
                inputManager = new DrumInputManager();

                noteScaleValue = gameConfiguration.themeSetting.laneSizeDrums / (float)noteSpriteSheetSize;
                bassNoteScaleValue = ((gameConfiguration.themeSetting.laneSizeDrums * 4) + (gameConfiguration.themeSetting.laneSeparatorSize * 3)) /
                                     ((float)noteSpriteSheetSize);

                
                laneSeparators = new DrumLaneSeparator(gameConfiguration.themeSetting.laneSizeDrums, gameConfiguration.themeSetting.laneSeparatorSize, effect,
                                                        laneSeparatorTexture, graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardDepth);

                fretboardBorders = new DrumFretboardBorder(gameConfiguration.themeSetting.laneSizeDrums, gameConfiguration.themeSetting.laneSeparatorSize, effect,
                                                            laneSeparatorTexture, graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardBorderSize, gameConfiguration.themeSetting.fretboardDepth);

                hitMarker = new DrumHitMarker(gameConfiguration.themeSetting.hitMarkerDepth, gameConfiguration.themeSetting.hitMarkerSize,
                                              gameConfiguration.themeSetting.laneSizeDrums, gameConfiguration.themeSetting.laneSeparatorSize,
                                              gameConfiguration.themeSetting.fretboardBorderSize, effect, hitMarkerTexture, graphics.GraphicsDevice);

                notes = NoteInitializer.InitializeNotesDrumSingle(noteSpriteSheetSize, notes, spriteSheetTex, gameConfiguration.themeSetting,
                                                                  noteScaleValue, bassNoteScaleValue, effect, graphics.GraphicsDevice);
            }
            else  // A guitar background and emitter setting will be the "default"
            {
                fretboard = new GuitarFretboard(maxNotesOnscreen, gameConfiguration);

                float cameraX = (gameConfiguration.themeSetting.laneSizeGuitar * 2) +
                                (gameConfiguration.themeSetting.laneSeparatorSize * 3) +
                                (gameConfiguration.themeSetting.laneSizeGuitar / 2);
                cameraPostion = new Vector3(cameraX, 170.0f, 0.0f);
                cameraLookAt = new Vector3(cameraX, 50.0f, -300.0f);
                viewMatrix = Matrix.CreateLookAt(cameraPostion, cameraLookAt, new Vector3(0, 1, 0));
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 0.2f, gameConfiguration.themeSetting.fretboardDepth);

                // Set up the particle explosions
                noteParticleEmitters.initalizeEmittersGuitarSingle();
                noteParticleEmitters.initializeLocationsGuitarSingle(gameConfiguration.themeSetting, graphics.GraphicsDevice, viewMatrix, projectionMatrix);
                backgroundFilename = "GuitarSingle.png";
                noteScaleValue = gameConfiguration.themeSetting.laneSizeGuitar / (float)noteSpriteSheetSize;
                
                
                laneSeparators = new GuitarLaneSeparator(gameConfiguration.themeSetting.laneSizeGuitar, gameConfiguration.themeSetting.laneSeparatorSize, effect,
                                                          laneSeparatorTexture, graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardDepth);
                fretboardBorders = new GuitarFretboardBorder(gameConfiguration.themeSetting.laneSizeGuitar, gameConfiguration.themeSetting.laneSeparatorSize, effect,
                                                        laneSeparatorTexture, graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardBorderSize, gameConfiguration.themeSetting.fretboardDepth);

                hitMarker = new GuitarHitMarker(gameConfiguration.themeSetting.hitMarkerDepth, gameConfiguration.themeSetting.hitMarkerSize,
                                                gameConfiguration.themeSetting.laneSizeGuitar, gameConfiguration.themeSetting.laneSeparatorSize,
                                                gameConfiguration.themeSetting.fretboardBorderSize, effect, hitMarkerTexture, graphics.GraphicsDevice);

                notes = NoteInitializer.InitializeNotesGuitarSingle(noteSpriteSheetSize, notes, spriteSheetTex, gameConfiguration.themeSetting,
                                                                    noteScaleValue, effect, graphics.GraphicsDevice);

                if (gameConfiguration.useDrumStyleInputForGuitarMode)
                {
                    inputManager = new DrumInputManager();
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
            fretboardTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardTexture);

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

            fretboard.loadContent(gameConfiguration, laneSeparatorTexture, hitMarkerTexture, effect,
                                        viewMatrix, projectionMatrix, noteSpriteSheetSize, graphics, Game,
                                        chartSelection);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (Note currNote in notes)
            {
                currNote.texturedVertexDeclaration.Dispose();
            }
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

                AudioInitializer.InitaliazeAudio(system, chartSelection, mainChart, result, musicChannel,
                                                 bassChannel, guitarChannel, drumChannel, musicStream,
                                                 bassStream, guitarStream, drumStream);
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
            if (mainChart.chartInfo.musicStream != null)
            {
                musicChannel.getPosition(ref currentMsec, TIMEUNIT.MS);
            }
            else
            {
                guitarChannel.getPosition(ref currentMsec, TIMEUNIT.MS);
            }

            // Get the current keyboard state
            keyboardInputManager.processKeyboardState(Keyboard.GetState());

            // The distance each note must step to be in sync with this current update
            //float currStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * gameConfiguration.speedModValue.noteVelocityMultiplier);
            float currStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * currStepPerMilisecond);

            currentGrade = inputManager.processPlayerInput(notes, noteParticleEmitters, hitBox,
                                                           playerInformation, keyboardInputManager,
                                                           mainChart.noteCharts[0]);

            noteUpdater.updateNotes(mainChart.noteCharts[0], ref noteIterator, notes, viewportRectangle,
                                    currStep, currentMsec + gameConfiguration.MSOffset,
                                    noteSpriteSheetSize, playerInformation, hitBox, noteParticleEmitters,
                                    gameConfiguration.themeSetting.fretboardDepth, gameConfiguration.MSTillHit, currStepPerMilisecond);

            FretboardUpdater.UpdateFretboards(fretboards, fretboardTex, effect, graphics.GraphicsDevice,
                                              gameConfiguration.themeSetting, currStep,
                                              mainChart.noteCharts[0].instrument, gameConfiguration.themeSetting.fretboardDepth);

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
            strManager.SetString(0, "Hitbox Y: " + hitBox.centerLocation + "\nHitbox Height: " + hitBox.goodThreshold * 2);
            strManager.SetString(1, times);
            strManager.SetString(3, "Score: " + playerInformation.currentScore.ToString() + "\n\n" +
                                     "Multiplier : " + playerInformation.currentMultiplier.ToString() + "\n\n" +
                                     "Combo :" + playerInformation.currentCombo.ToString());
            strManager.SetString(4, "Health: " + playerInformation.currentHealth.ToString());

            
            if (currentGrade.X == 1)  // Will not be 1 unless a note was hit and inside the hitbox
            {
                string gradeString = "Early <----> Late\n";
                
                // Top Good
                if (currentGrade.Y < -hitBox.greatThreshold)
                {
                    gradeString += "<----";
                }
                
                // Top Great
                else if (currentGrade.Y < -hitBox.perfectThreshold)
                {
                    gradeString += "<--";
                }

                // Perfect
                else if (currentGrade.Y < hitBox.perfectThreshold)
                {
                    gradeString += "(^_^)";
                }

                // Bottom Great
                else if (currentGrade.Y < hitBox.greatThreshold)
                {
                    gradeString += "-->";
                }

                // Bottom Good
                else
                {
                    gradeString += "---->";
                }
                strManager.SetString(5, gradeString);
            }
            
            // Ensure the audio tracks are within 50 MS accuracy
            if ((((int)guitarPosition - (int)currentMsec > 50) ||
                ((int)guitarPosition - (int)currentMsec < -50)) &&
                (mainChart.chartInfo.guitarStream != null))
            {
                guitarChannel.setPosition(musicPosition, TIMEUNIT.MS);
            }
            if ((((int)bassPosition - (int)currentMsec > 50) ||
                ((int)bassPosition - (int)currentMsec < -50)) &&
                (mainChart.chartInfo.bassStream != null))
            {
                bassChannel.setPosition(musicPosition, TIMEUNIT.MS);
            }
            if ((((int)drumPosition - (int)currentMsec > 50) ||
                ((int)drumPosition - (int)currentMsec < -50)) &&
                (mainChart.chartInfo.drumStream != null))
            {
                drumChannel.setPosition(musicPosition, TIMEUNIT.MS);
            }

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

            fretboard.update(keyboardInputManager, viewportRectangle, gameConfiguration, effect,
                                   currStep, currentMsec, graphics, noteSpriteSheetSize, gameTime);

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

            //foreach (FretboardBackground fretboard in fretboards)
            //{
            //    if (fretboard.alive)
            //    {
            //        fretboard.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            //    }
            //}

            //laneSeparators.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            //fretboardBorders.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            //hitMarker.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);

            ////Draw the notes
            //for (int i = 0; i < notes.GetLength(0); i++)
            //{
            //    for (int j = 0; j < notes.GetLength(1); j++)
            //    {
            //        if (notes[i, j].alive)
            //        {
            //            notes[i, j].draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            //        }
            //    }
            //}

            //foreach (Emitter emitter in noteParticleEmitters.emitterList)
            //{
            //    renderer.RenderEmitter(emitter);
            //}

            fretboard.draw(graphics, viewMatrix, projectionMatrix);

            base.Draw(gameTime);
        }
    }
}
