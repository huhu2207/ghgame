using Microsoft.Xna.Framework;
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
        public static Note[,] InitializeNotesDrumSingle(int noteSpriteSheetSize, Note[,] Notes, Texture2D spriteSheetTex,
                                                        GameConfiguration gameConfiguration)
        {
            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Bass Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(0, noteSpriteSheetSize * 5, noteSpriteSheetSize * 4, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            Notes[i, j].originalSpritePosition = new Rectangle(0, noteSpriteSheetSize * 5, noteSpriteSheetSize * 4, noteSpriteSheetSize);
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 1, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 1, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 2, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * 1) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 2, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 3, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * 2) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 3, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            break;
                        case 4:  // Green Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 0, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * 3) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * 0, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                            break;
                    }
                    Notes[i, j].velocity = new Vector2(0.0f, (float)gameConfiguration.speedModValue.noteVelocityMultiplier);
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
        public static Note[,] InitializeNotesGuitarSingle(int noteSpriteSheetSize, Note[,] Notes, Texture2D spriteSheetTex,
                                                          GameConfiguration gameConfiguration)
        {
            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    Notes[i, j] = new Note(spriteSheetTex,
                                  new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                    Notes[i, j].velocity = new Vector2(0.0f, (float)gameConfiguration.speedModValue.noteVelocityMultiplier);
                    Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneGuitarSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * i) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                    Notes[i, j].originalSpritePosition = new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize);
                }
            }
            return Notes;
        }
    }
}
