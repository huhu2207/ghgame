using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinGH.EngineExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;

namespace MinGH.GameScreen.SinglePlayer
{
    public static class FretboardUpdater
    {
        public static void UpdateFretboards(List<Fretboard3D> fretboards, Texture2D fretboardTex,
                                            Effect effect, GraphicsDevice graphics,
                                            GameConfiguration gameConfiguration, float currStep)
        {
            foreach (Fretboard3D fretboard in fretboards)
            {
                fretboard.position3D += new Vector3(0f, 0f, currStep);
            }
            float fretboardHeight = fretboardTex.Height * ((gameConfiguration.themeSetting.laneSize * 5) / (float)fretboardTex.Width);

            if (fretboards.Count == 0)
            {
                Fretboard3D fretboardToAdd = new Fretboard3D(fretboardTex, effect, graphics);
                float newScale = ((gameConfiguration.themeSetting.laneSize * 5) + (gameConfiguration.themeSetting.laneBorderSize * 4))
                                 / (float)fretboardTex.Width;
                fretboardToAdd.scale3D = new Vector3(newScale, 1f, newScale);
                fretboardToAdd.position3D = new Vector3(0f, 0f, -1000f);
                fretboardToAdd.alive = true;
                fretboards.Add(fretboardToAdd);
            }
            if (fretboards[fretboards.Count - 1].position3D.Z > -1000)
            {
                Fretboard3D fretboardToAdd = new Fretboard3D(fretboardTex, effect, graphics);
                float newScale = ((gameConfiguration.themeSetting.laneSize * 5) + (gameConfiguration.themeSetting.laneBorderSize * 4))
                                 / (float)fretboardTex.Width;
                fretboardToAdd.scale3D = new Vector3(newScale, 1f, newScale);
                fretboardToAdd.position3D = new Vector3(0f, 0f, fretboards[fretboards.Count - 1].position3D.Z - fretboardHeight);
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
