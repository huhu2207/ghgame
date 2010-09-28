﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;
using MinGH.EngineExtensions;
using GameEngine;

namespace MinGH.Fretboard
{
    /// <summary>
    /// Maintains the status of a fretboard list.
    /// </summary>
    public static class FretboardUpdater
    {
        /// <summary>
        /// Updates The fretboard list so that all pixels from 0 to -1000 will have a part
        /// of a fretboard.  This gives off an endless board effect.
        /// </summary>
        /// <param name="fretboards">A list of fretboard objects lined up one after another.</param>
        /// <param name="fretboardTex">The texture used for the fretboards.</param>
        /// <param name="effect">The shader effect to use.</param>
        /// <param name="graphics">A graphics device.</param>
        /// <param name="themeSetting">The current game's theme setting.</param>
        /// <param name="currStep">The current step in which every fretboard must move this update.</param>
        /// <param name="instrument">Which instrument is currently being played.</param>
        public static void UpdateFretboards(List<GameObject> fretboards, Texture2D fretboardTex,
                                            Effect effect, GraphicsDevice graphics,
                                            ThemeSetting themeSetting, float currStep, string instrument,
                                            float fretboardDepth)
        {
            foreach (FretboardBackground fretboard in fretboards)
            {
                fretboard.position3D += new Vector3(0f, 0f, currStep);
            }


            int laneSize = 0;
            float fretboardTexDepth = fretboardTex.Height * ((laneSize * 5) / (float)fretboardTex.Width);
            float newScale = 0;
            if (instrument == "Drums")
            {
                laneSize = themeSetting.laneSizeDrums;
                newScale = (laneSize * 4) + (themeSetting.laneSeparatorSize * 3);
                fretboardTexDepth = fretboardTex.Height * ((laneSize * 4) / (float)fretboardTex.Width);
            }
            else
            {
                laneSize = themeSetting.laneSizeGuitar;
                newScale = (laneSize * 5) + (themeSetting.laneSeparatorSize * 4);
                fretboardTexDepth = fretboardTex.Height * ((laneSize * 5) / (float)fretboardTex.Width);
            }

            if (fretboards.Count == 0)
            {
                FretboardBackground fretboardToAdd = new FretboardBackground(fretboardTex, effect, graphics, newScale);
                fretboardToAdd.scale3D = new Vector3(newScale, newScale, 1f);
                fretboardToAdd.position3D = new Vector3(0f, 0f, -fretboardDepth);
                fretboardToAdd.alive = true;
                fretboardToAdd.rotation3D = new Vector3(-MathHelper.PiOver2, 0f, 0f);
                fretboards.Add(fretboardToAdd);

            }
            if (fretboards[fretboards.Count - 1].position3D.Z > -fretboardDepth)
            {
                FretboardBackground fretboardToAdd = new FretboardBackground(fretboardTex, effect, graphics, laneSize);
                fretboardToAdd.scale3D = new Vector3(newScale, newScale, 1f);
                fretboardToAdd.position3D = new Vector3(0f, 0f, fretboards[fretboards.Count - 1].position3D.Z - newScale);
                fretboardToAdd.alive = true;
                fretboardToAdd.rotation3D = new Vector3(-MathHelper.PiOver2, 0f, 0f);
                fretboards.Add(fretboardToAdd);
            }
            if (fretboards[0].position3D.Z > 0 + fretboards[0].pixelsPerSpriteSheetStepY)
            {
                fretboards.RemoveAt(0);
            }
        }
    }
}
