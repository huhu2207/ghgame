﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.EngineExtensions;
using MinGH.GameScreen;
using ProjectMercury.Emitters;
using ProjectMercury.Renderers;
using GameEngine;

namespace MinGH.Fretboard
{
    class DrumFretboard : IFretboard
    {

        public DrumFretboard(int maxNotesOnscreen, GameConfiguration gameConfiguration, ChartSelection chartSelection)
        {
            mainChart = new Chart(chartSelection);
            renderer = new PointSpriteRenderer();
            noteParticleEmitters = new NoteParticleEmitters();
            playerInformation = new PlayerInformation();
            noteIterator = 0;
            noteScaleValue = 0.0f;
            bassNoteScaleValue = 0.0f;
            notes = new Note[5, maxNotesOnscreen];
            fretboardBackgrounds = new List<GameObject>();
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
                                GraphicsDeviceManager graphics, Game game)
        {
            

            spriteSheetTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.noteSkinTexture);
            laneSeparatorTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.laneSeparatorTexture);
            hitMarkerTexture = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.hitMarkerTexture);
            fretboardTex = Texture2D.FromFile(graphics.GraphicsDevice, gameConfiguration.themeSetting.fretboardTexture);


            // Set up the particle explosions
            noteParticleEmitters.initalizeEmittersDrumsSingle(gameConfiguration.themeSetting, graphics.GraphicsDevice, viewMatrix, projectionMatrix);
            noteParticleEmitters.initializeLocationsDrumsSingle(gameConfiguration.themeSetting, graphics.GraphicsDevice, viewMatrix, projectionMatrix);

            inputManager = new DrumInputManager();

            noteScaleValue = gameConfiguration.themeSetting.laneSizeDrums;
            bassNoteScaleValue = ((gameConfiguration.themeSetting.laneSizeDrums * 4) + (gameConfiguration.themeSetting.laneSeparatorSize * 3));


            laneSeparators = new DrumLaneSeparators(gameConfiguration, effect, laneSeparatorTexture, graphics.GraphicsDevice);

            fretboardBorders = new DrumFretboardBorder(effect, laneSeparatorTexture, graphics.GraphicsDevice, gameConfiguration);

            hitMarker = new GameObject(hitMarkerTexture, effect, graphics.GraphicsDevice);
            hitMarker.pixelsPerSpriteSheetStepX = hitMarkerTexture.Width;
            hitMarker.pixelsPerSpriteSheetStepY = hitMarkerTexture.Height;
            hitMarker.position3D = new Vector3(-gameConfiguration.themeSetting.fretboardBorderSize, 0f, -gameConfiguration.themeSetting.hitMarkerDepth);
            hitMarker.scale3D = new Vector3((4 * gameConfiguration.themeSetting.laneSizeDrums) +
                                            (3 * gameConfiguration.themeSetting.laneSeparatorSize) +
                                            (2 * gameConfiguration.themeSetting.fretboardBorderSize),
                                            gameConfiguration.themeSetting.hitMarkerSize, 1f);
            hitMarker.rotation3D = new Vector3(-MathHelper.PiOver2, 0f, 0f);

            notes = NoteInitializer.InitializeNotesDrumSingle(noteSpriteSheetSize, notes, spriteSheetTex, gameConfiguration.themeSetting,
                                                              noteScaleValue, bassNoteScaleValue, effect, graphics.GraphicsDevice);

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
                           GameConfiguration gameConfiguration, Effect effect, uint currentMsec,
                           GraphicsDeviceManager graphics, int noteSpriteSheetSize, GameTime gameTime,
                           float cameraYRotation)
        {
            float currStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * currStepPerMilisecond);

            inputManager.processPlayerInput(notes, noteParticleEmitters, hitBox,
                                            playerInformation, keyboardInputManager,
                                            mainChart.noteCharts[0]);

            noteUpdater.updateNotes(mainChart.noteCharts[0], ref noteIterator, notes, viewportRectangle,
                                    currStep, currentMsec + gameConfiguration.MSOffset,
                                    noteSpriteSheetSize, playerInformation, hitBox, noteParticleEmitters,
                                    gameConfiguration.themeSetting.fretboardDepth, gameConfiguration.MSTillHit, currStepPerMilisecond,
                                    cameraYRotation);

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
            foreach (GameObject fretboard in fretboardBackgrounds)
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

        public ChartInfo getChartInfo()
        {
            return mainChart.chartInfo;
        }

        public PlayerInformation getPlayerInfo()
        {
            return playerInformation;
        }

        Texture2D spriteSheetTex, fretboardTex;
        Note[,] notes;  // Will hold every note currently on the screen
        List<GameObject> fretboardBackgrounds;  // A set of fretboards aligned next to each other giving a continous effect
        DrumLaneSeparators laneSeparators;
        DrumFretboardBorder fretboardBorders;
        GameObject hitMarker;
        int noteIterator;  // This iterator is used to keep track of which note to draw next
        float noteScaleValue, bassNoteScaleValue;
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