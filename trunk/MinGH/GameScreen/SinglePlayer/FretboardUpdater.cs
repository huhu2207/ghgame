using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;
using MinGH.EngineExtensions;

namespace MinGH.GameScreen.SinglePlayer
{
    public static class FretboardUpdater
    {
        public static void UpdateFretboards(List<Fretboard3D> fretboards, Texture2D fretboardTex,
                                            Effect effect, GraphicsDevice graphics,
                                            GameConfiguration gameConfiguration, float currStep, string instrument)
        {
            foreach (Fretboard3D fretboard in fretboards)
            {
                fretboard.position3D += new Vector3(0f, 0f, currStep);
            }


            int laneSize = 0;
            float fretboardDepth = fretboardTex.Height * ((laneSize * 5) / (float)fretboardTex.Width);
            float newScale = 0;
            if (instrument == "Drums")
            {
                laneSize = gameConfiguration.themeSetting.laneSizeDrums;
                newScale = ((laneSize * 4) + (gameConfiguration.themeSetting.laneSeparatorSize * 3))
                           / (float)fretboardTex.Width;
                fretboardDepth = fretboardTex.Height * ((laneSize * 4) / (float)fretboardTex.Width);
            }
            else
            {
                laneSize = gameConfiguration.themeSetting.laneSizeGuitar;
                newScale = ((laneSize * 5) + (gameConfiguration.themeSetting.laneSeparatorSize * 4))
                           / (float)fretboardTex.Width;
                fretboardDepth = fretboardTex.Height * ((laneSize * 5) / (float)fretboardTex.Width);
            }

            if (fretboards.Count == 0)
            {
                Fretboard3D fretboardToAdd = new Fretboard3D(fretboardTex, effect, graphics);
                fretboardToAdd.scale3D = new Vector3(newScale, 1f, newScale);
                fretboardToAdd.position3D = new Vector3(0f, 0f, -1000f);
                fretboardToAdd.alive = true;
                fretboards.Add(fretboardToAdd);
            }
            if (fretboards[fretboards.Count - 1].position3D.Z > -1000)
            {
                Fretboard3D fretboardToAdd = new Fretboard3D(fretboardTex, effect, graphics);
                fretboardToAdd.scale3D = new Vector3(newScale, 1f, newScale);
                fretboardToAdd.position3D = new Vector3(0f, 0f, fretboards[fretboards.Count - 1].position3D.Z - fretboardDepth);
                fretboardToAdd.alive = true;
                fretboards.Add(fretboardToAdd);
            }
            if (fretboards[0].position3D.Z > 0)
            {
                fretboards.RemoveAt(0);
            }
        }
    }
}
