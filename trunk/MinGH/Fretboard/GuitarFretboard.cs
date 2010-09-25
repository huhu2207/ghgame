using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MinGH.ChartImpl;
using MinGH.EngineExtensions;
using MinGH.GameScreen;
using MinGH.GameScreen.SinglePlayer;
using MinGH.Config;
using Microsoft.Xna.Framework;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;
using MinGH.Interfaces;

namespace MinGH.Fretboard
{
    class GuitarFretboard
    {

        public GuitarFretboard(int maxNotesOnscreen, GameConfiguration gameConfiguration)
        {
            renderer = new PointSpriteRenderer();
            noteParticleEmitters = new NoteParticleEmitters();
            playerInformation = new PlayerInformation();
            noteIterator = 0;
            noteScaleValue = 0.0f;
            notes = new Note[5, maxNotesOnscreen];
            fretboardBackgrounds = new List<FretboardBackground>();
            hitBox = new HorizontalHitBox((int)(gameConfiguration.themeSetting.hitMarkerDepth + (gameConfiguration.themeSetting.hitMarkerSize / 2.0f)),
                                          gameConfiguration.MSTillHit);
            distanceFromNoteStartToHitmarker = gameConfiguration.themeSetting.fretboardDepth - hitBox.centerLocation;
            currStepPerMilisecond = distanceFromNoteStartToHitmarker / gameConfiguration.MSTillHit;

            if (gameConfiguration.autoplay)
            {
                noteUpdater = new AutoplayNoteUpdater();
            }
            else
            {
                noteUpdater = new NoteUpdater();
            }

            
        }

        public void loadContent(GameConfiguration gameConfiguration, Texture2D laneSeparatorTexture, Texture2D hitMarkerTexture,
                                Effect effect, Matrix viewMatrix, Matrix projectionMatrix, int noteSpriteSheetSize,
                                GraphicsDeviceManager graphics, Game game, ChartSelection chartSelection)
        {
            mainChart = new Chart(chartSelection);

            spriteSheetTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.noteSkinTexture);
            laneSeparatorTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.laneSeparatorTexture);
            hitMarkerTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.hitMarkerTexture);
            fretboardTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardTexture);

            // Set up the particle explosions
            noteParticleEmitters.initalizeEmittersGuitarSingle();
            noteParticleEmitters.initializeLocationsGuitarSingle(gameConfiguration.themeSetting, graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            noteScaleValue = gameConfiguration.themeSetting.laneSizeGuitar / (float)noteSpriteSheetSize;


            laneSeparators = new GuitarLaneSeparators(gameConfiguration.themeSetting.laneSizeGuitar, gameConfiguration.themeSetting.laneSeparatorSize, effect,
                                                      laneSeparatorTexture, graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardDepth);
            fretboardBorders = new GuitarFretboardBorders(gameConfiguration.themeSetting.laneSizeGuitar, gameConfiguration.themeSetting.laneSeparatorSize, effect,
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

            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                emitter.Initialize();
            }

            renderer.GraphicsDeviceService = graphics;
            renderer.BlendMode = SpriteBlendMode.Additive;
            renderer.LoadContent(game.Content);

            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                emitter.LoadContent(game.Content);
            }
        }

        public void unloadContent()
        {
            foreach (Note currNote in notes)
            {
                currNote.texturedVertexDeclaration.Dispose();
            }
        }

        public void update(IKeyboardInputManager keyboardInputManager, Rectangle viewportRectangle,
                           GameConfiguration gameConfiguration, Effect effect, float currStep, uint currentMsec,
                           GraphicsDeviceManager graphics, int noteSpriteSheetSize, GameTime gameTime)
        {
            inputManager.processPlayerInput(notes, noteParticleEmitters, hitBox,
                                            playerInformation, keyboardInputManager,
                                            mainChart.noteCharts[0]);

            noteUpdater.updateNotes(mainChart.noteCharts[0], ref noteIterator, notes, viewportRectangle,
                                    currStep, currentMsec + gameConfiguration.MSOffset,
                                    noteSpriteSheetSize, playerInformation, hitBox, noteParticleEmitters,
                                    gameConfiguration.themeSetting.fretboardDepth, gameConfiguration.MSTillHit, currStepPerMilisecond);

            FretboardUpdater.UpdateFretboards(fretboardBackgrounds, fretboardTex, effect, graphics.GraphicsDevice,
                                              gameConfiguration.themeSetting, currStep,
                                              mainChart.noteCharts[0].instrument, gameConfiguration.themeSetting.fretboardDepth);

            // Update every particle explosion
            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                emitter.Update((float)gameTime.TotalGameTime.TotalSeconds, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public void draw(GraphicsDeviceManager graphics, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (FretboardBackground fretboard in fretboardBackgrounds)
            {
                if (fretboard.alive)
                {
                    fretboard.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
                }
            }

            laneSeparators.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            fretboardBorders.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            hitMarker.draw(graphics.GraphicsDevice, viewMatrix, projectionMatrix);

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

            foreach (Emitter emitter in noteParticleEmitters.emitterList)
            {
                renderer.RenderEmitter(emitter);
            }

        }

        Texture2D spriteSheetTex, fretboardTex;
        Note[,] notes;  // Will hold every note currently on the screen
        List<FretboardBackground> fretboardBackgrounds;  // A set of fretboards aligned next to each other giving a continous effect
        LaneSeparator laneSeparators;
        FretboardBorders fretboardBorders;
        HitMarker hitMarker;
        int noteIterator;  // This iterator is used to keep track of which note to draw next
        float noteScaleValue;
        IInputManager inputManager;
        INoteUpdater noteUpdater;
        HorizontalHitBox hitBox;
        PlayerInformation playerInformation;
        Chart mainChart;  // Create the chart file
        float distanceFromNoteStartToHitmarker;
        float currStepPerMilisecond; // How many game space units a note must move per milisecond

        // Project Mercury Particle Engine related variables
        NoteParticleEmitters noteParticleEmitters;
        PointSpriteRenderer renderer;
    
    }
}
