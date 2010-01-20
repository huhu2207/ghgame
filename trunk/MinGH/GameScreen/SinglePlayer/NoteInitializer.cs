using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.EngineExtensions;
using MinGH.Config;

namespace MinGH.GameScreen.SinglePlayer
{
    public static class NoteInitializer
    {
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
                                          new Rectangle(noteSpriteSheetSize * 5, 0, noteSpriteSheetSize * 4, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 1, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 2, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * 1) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 3, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * 2) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            break;
                        case 4:  // Green Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * 0, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneDrumSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * 3) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                            break;
                    }
                    Notes[i, j].velocity = new Vector2(0.0f, (float)gameConfiguration.speedModValue.noteVelocityMultiplier);
                }
            }
            return Notes;
        }

        public static Note[,] InitializeNotesGuitarSingle(int noteSpriteSheetSize, Note[,] Notes, Texture2D spriteSheetTex,
                                                          GameConfiguration gameConfiguration)
        {
            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Green Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            break;
                        case 4:  // Orange Notes
                            Notes[i, j] = new Note(spriteSheetTex,
                                          new Rectangle(noteSpriteSheetSize * i, 0, noteSpriteSheetSize, noteSpriteSheetSize), 0);
                            break;
                    }
                    Notes[i, j].velocity = new Vector2(0.0f, (float)gameConfiguration.speedModValue.noteVelocityMultiplier);
                    Notes[i, j].position.X = Notes[i, j].spriteSheetOffset +
                                             gameConfiguration.themeSetting.distanceUntilLeftMostLaneGuitarSingle +
                                             ((gameConfiguration.themeSetting.laneSize + gameConfiguration.themeSetting.laneBorderSize) * i) +
                                             gameConfiguration.themeSetting.laneBorderSize;
                }
            }
            return Notes;
        }
    }
}
