using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;
using MinGH.EngineExtensions;

namespace MinGH.Fretboard
{
    /// <summary>
    /// Initializes a 2D array of drawable notes acording to the difficulty specified.
    /// </summary>
    public static class NoteInitializer
    {
        /// <summary>
        /// Initialize the notes array according to the drum sprite specification.
        /// Due to the differences of how the drum notes are laid out, we must use a
        /// lengthy switch statment as opposed simply iterating through like guitar.
        /// </summary>
        public static Note[,] InitializeNotesDrumSingle(int noteSpriteSheetSize, Note[,] Notes, Texture2D spriteSheetTex,
                                                        ThemeSetting themeSetting, float noteScaleValue, float bassNoteScaleValue,
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
                            Notes[i, j] = new Note(spriteSheetTex, new Rectangle(0, noteSpriteSheetSize * 5, noteSpriteSheetSize * 4, noteSpriteSheetSize),
                                                   effect, device);
                            //Notes[i, j].initalizeTextureCoords(5, 0, 400, 100);
                            Notes[i, j].spriteSheetStepY = 4;

                            newY = -noteSpriteSheetSize / 2 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(0, newY, 0);
                            Notes[i, j].originalSpriteStepX = 0;
                            Notes[i, j].originalSpriteStepY = 4;
                            Notes[i, j].scale3D = new Vector3(bassNoteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 1, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);

                            newY = -noteSpriteSheetSize / 2 * noteScaleValue;
                            Notes[i, j].position3D = new Vector3(0, newY, 0);
                            Notes[i, j].originalSpriteStepX = 1;
                            Notes[i, j].originalSpriteStepY = 0;
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 2, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            newY = -noteSpriteSheetSize / 2 * noteScaleValue;
                            Notes[i, j].originalSpriteStepX = 2;
                            Notes[i, j].originalSpriteStepY = 0;
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 3, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            newY = -noteSpriteSheetSize / 2 * noteScaleValue;
                            Notes[i, j].originalSpriteStepX = 3;
                            Notes[i, j].originalSpriteStepY = 0;
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                        case 4:  // Green Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(0, 0, noteSpriteSheetSize, noteSpriteSheetSize), effect, device);
                            newY = -noteSpriteSheetSize / 2 * noteScaleValue;
                            Notes[i, j].originalSpriteStepX = 4;
                            Notes[i, j].originalSpriteStepY = 0;
                            Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                            break;
                    }
                }
            }
            return Notes;
        }

        /// <summary>
        /// Initialize the notes array according to the guitar sprite specification.
        /// </summary>
        public static Note[,] InitializeNotesGuitarSingle(int noteSpriteSheetSize, Note[,] Notes, Texture2D spriteSheetTex,
                                                          ThemeSetting themeSetting, float noteScaleValue, Effect effect,
                                                          GraphicsDevice device)
        {
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    Notes[i, j] = new Note(spriteSheetTex, new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize),
                                           effect, device);
                    //Notes[i, j].initalizeTextureCoords(i, 0, noteSpriteSheetSize, noteSpriteSheetSize);

                    float newY = -noteSpriteSheetSize / 2 * noteScaleValue;
                    Notes[i, j].position3D = new Vector3((i * 100 * noteScaleValue) + (themeSetting.laneSeparatorSize * i), newY, 0);
                    Notes[i, j].originalSpriteStepX = i;
                    Notes[i, j].originalSpriteStepY = 0;
                    Notes[i, j].scale3D = new Vector3(noteScaleValue, noteScaleValue, 1.0f);
                }
            }
            return Notes;
        }
    }
}
