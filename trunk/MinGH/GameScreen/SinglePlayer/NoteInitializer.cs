﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.EngineExtensions;
using MinGH.Config;

namespace MinGH.GameScreen.SinglePlayer
{
    public static class NoteInitializer
    {
        /// <summary>
        /// Initialize the notes array according to the drum sprite specification.
        /// </summary>
        /// <param name="noteSpriteSheetSize">The size of an individual element in the sprite sheet</param>
        /// <param name="Notes">The array containing all the drawable notes to initalize.</param>
        /// <param name="spriteSheetTex">The sprite sheet texture as a whole.</param>
        /// <param name="gameConfiguration">The current game configuration.</param>
        /// <returns>A filled out drawable note 2d array.</returns>
        public static Note3D[,] InitializeNotesDrumSingle(int noteSpriteSheetSize, Note3D[,] Notes, Texture2D spriteSheetTex,
                                                        GameConfiguration gameConfiguration, float noteScaleValue, float bassNoteScaleValue,
                                                        Effect effect, GraphicsDevice device)
        {
            float newY = 0.0f;
            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Bass Notes
                            Notes[i, j] = new Note3D(spriteSheetTex,
                                          new Rectangle(0, noteSpriteSheetSize * 5, noteSpriteSheetSize * 4, noteSpriteSheetSize), effect, device);
                            Notes[i, j].initalizeTextureCoords(0, 0, 500);

                            newY = -50 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(0, newY, 0);
                            Notes[i, j].originalSpritePosition = new Rectangle(0, noteSpriteSheetSize * 5, noteSpriteSheetSize * 4, noteSpriteSheetSize);
                            Notes[i, j].scale3D = new Vector3(bassNoteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note3D(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 1, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            
                            newY = -50 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(0, newY, 0);
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 1, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note3D(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 2, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            newY = -50 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(100 * noteScaleValue + (gameConfiguration.themeSetting.laneSeparatorSize * 1), newY, 0);
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 2, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note3D(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 3, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            newY = -50 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(2 * 100 * noteScaleValue + (gameConfiguration.themeSetting.laneSeparatorSize * 2), newY, 0);
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 3, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 4:  // Green Notes
                            Notes[i, j] = new Note3D(spriteSheetTex,
                                          new Rectangle(0, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            newY = -50 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(3 * 100 * noteScaleValue + (gameConfiguration.themeSetting.laneSeparatorSize * 3), newY, 0);
                            Notes[i, j].originalSpritePosition = new Rectangle(0, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                    }
                    //Notes[i, j].velocity = new Vector2(0.0f, (float)gameConfiguration.speedModValue.noteVelocityMultiplier);
                }
            }
            return Notes;
        }

        /// <summary>
        /// Initialize the notes array according to the guitar sprite specification.
        /// </summary>
        /// <param name="noteSpriteSheetSize">The size of an individual element in the sprite sheet</param>
        /// <param name="Notes">The array containing all the drawable notes to initalize.</param>
        /// <param name="spriteSheetTex">The sprite sheet texture as a whole.</param>
        /// <param name="gameConfiguration">The current game configuration.</param>
        /// <returns>A filled out drawable note 2d array.</returns>
        public static Note3D[,] InitializeNotesGuitarSingle(int noteSpriteSheetSize, Note3D[,] Notes, Texture2D spriteSheetTex,
                                                          GameConfiguration gameConfiguration, float noteScaleValue, Effect effect,
                                                          GraphicsDevice device)
        {
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    Notes[i, j] = new Note3D(spriteSheetTex,
                                  new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize),
                                  effect, device);
                    Notes[i, j].initalizeTextureCoords(i * 100, 0, 100);

                    float newY = -50 * noteScaleValue;
                    Notes[i, j].position3D = new Vector3((i * 100 * noteScaleValue) + (gameConfiguration.themeSetting.laneSeparatorSize * i), newY, 0);
                    Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                    Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                }
            }
            return Notes;
        }
    }
}
