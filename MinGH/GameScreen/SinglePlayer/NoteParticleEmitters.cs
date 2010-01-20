using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;
using ProjectMercury.Emitters;
using MinGH.Config;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// A manager class that generates and stores the particle emitters for the
    /// note explosion animation.
    /// </summary>
    public class NoteParticleEmitters
    {
		/// <summary>
		/// The list of the actual emitters.  There should only be 5 for MinGH, but a list
		/// is used in case of future note additions.
		/// </summary>
        public List<Emitter> emitterList { get; set; }
		
		/// <summary>
		/// A list that mirrors the emitterList, but instead stores the locations for each
		/// particle emitter.  The two lists should be encapsulated into one list contating
		/// a custom class...
		/// </summary>
        public List<Vector2> explosionLocations { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NoteParticleEmitters()
        {
            emitterList = new List<Emitter>();
            explosionLocations = new List<Vector2>();
        }

		/// <summary>
		/// Initializes each of the emitters and adds them to the emitter list.
		/// To know more about the emitters themselves, look at the Mercury
		/// Particle Engine documentation.
		/// </summary>
        public void initalizeEmittersGuitarSingle()
        {
            CircleEmitter emitterToAdd;

            for (int i = 0; i < 5; i++)
            {
                emitterToAdd = new CircleEmitter();

                emitterToAdd.Budget = 3000;
                emitterToAdd.ReleaseQuantity = 500;
                emitterToAdd.ReleaseScale = new VariableFloat { Anchor = 40f, Variation = 0.5f };
                emitterToAdd.ReleaseSpeed = new VariableFloat { Anchor = 200f, Variation = 1f };
                emitterToAdd.ReleaseOpacity = new VariableFloat { Anchor = 0.2f, Variation = 0f };
                emitterToAdd.Term = 0.3f;
                emitterToAdd.Radiate = true;
                emitterToAdd.Ring = false;
                emitterToAdd.Radius = 10f;
                emitterToAdd.ParticleTextureAssetName = "Particles\\FlowerBurst";

                switch (i)
                {
                    case 0:
                        emitterToAdd.ReleaseColour = Color.Green.ToVector3();
                        break;
                    case 1:
                        emitterToAdd.ReleaseColour = Color.Red.ToVector3();
                        break;
                    case 2:
                        emitterToAdd.ReleaseColour = Color.Yellow.ToVector3();
                        break;
                    case 3:
                        emitterToAdd.ReleaseColour = Color.Blue.ToVector3();
                        break;
                    case 4:
                        emitterToAdd.ReleaseColour = Color.Orange.ToVector3();
                        break;
                }

                emitterList.Add(emitterToAdd);
            }
        }

        public void initalizeEmittersDrumsSingle(ThemeSetting themeSetting)
        {
            LineEmitter lineEmitterToAdd = new LineEmitter();
            lineEmitterToAdd.Budget = 5000;
            lineEmitterToAdd.ReleaseQuantity = 1200;
            lineEmitterToAdd.ReleaseScale = new VariableFloat { Anchor = 50f, Variation = 0.5f };
            lineEmitterToAdd.ReleaseSpeed = new VariableFloat { Anchor = 200f, Variation = 1f };
            lineEmitterToAdd.ReleaseOpacity = new VariableFloat { Anchor = 0.2f, Variation = 0f };
            lineEmitterToAdd.Term = 0.3f;
            lineEmitterToAdd.ParticleTextureAssetName = "Particles\\FlowerBurst";
            lineEmitterToAdd.ReleaseColour = Color.Orange.ToVector3();
            lineEmitterToAdd.Length = (themeSetting.laneBorderSize * 4) + (themeSetting.laneSize * 4);
            emitterList.Add(lineEmitterToAdd);

            CircleEmitter circleEmitterToAdd;
            for (int i = 1; i < 5; i++)
            {
                circleEmitterToAdd = new CircleEmitter();

                circleEmitterToAdd.Budget = 3000;
                circleEmitterToAdd.ReleaseQuantity = 500;
                circleEmitterToAdd.ReleaseScale = new VariableFloat { Anchor = 40f, Variation = 0.5f };
                circleEmitterToAdd.ReleaseSpeed = new VariableFloat { Anchor = 200f, Variation = 1f };
                circleEmitterToAdd.ReleaseOpacity = new VariableFloat { Anchor = 0.2f, Variation = 0f };
                circleEmitterToAdd.Term = 0.3f;
                circleEmitterToAdd.Radiate = true;
                circleEmitterToAdd.Ring = false;
                circleEmitterToAdd.Radius = 10f;
                circleEmitterToAdd.ParticleTextureAssetName = "Particles\\FlowerBurst";

                switch (i)
                {
                    case 1:
                        circleEmitterToAdd.ReleaseColour = Color.Red.ToVector3();
                        break;
                    case 2:
                        circleEmitterToAdd.ReleaseColour = Color.Yellow.ToVector3();
                        break;
                    case 3:
                        circleEmitterToAdd.ReleaseColour = Color.Blue.ToVector3();
                        break;
                    case 4:
                        circleEmitterToAdd.ReleaseColour = Color.Green.ToVector3();
                        break;
                }

                emitterList.Add(circleEmitterToAdd);
            }
        }

      
		/// <summary>
		/// Generates the locations for all the emitters according to a padding, noteWidth
		/// and the hitbar's Y value
		/// </summary>
        /// <param name="gameConfiguration">
        /// The current game configuration.
        /// </param>
        public void initializeLocationsGuitarSingle(ThemeSetting themeSetting, int hitBarYValue)
        {
            for (int i = 0; i < 5; i++)
            {
                int X = themeSetting.distanceUntilLeftMostLaneGuitarSingle + (themeSetting.laneSize * i) +
                        (themeSetting.laneBorderSize * i + 1) + (themeSetting.laneSize / 2);

                explosionLocations.Add(new Vector2(X, hitBarYValue));
            }
        }

        public void initializeLocationsDrumsSingle(ThemeSetting themeSetting, int hitBarYValue)
        {
            // We have to move the bass pedal emitter to the center of the drum lanes
            int bassX = themeSetting.distanceUntilLeftMostLaneDrumSingle + (themeSetting.laneBorderSize * 2) +
                        (themeSetting.laneSize * 2);
            explosionLocations.Add(new Vector2(bassX, hitBarYValue));

            for (int i = 0; i < 4; i++)
            {
                int notesX = themeSetting.distanceUntilLeftMostLaneDrumSingle + (themeSetting.laneSize * i) +
                        (themeSetting.laneBorderSize * i + 1) + (themeSetting.laneSize / 2);

                explosionLocations.Add(new Vector2(notesX, hitBarYValue));
            }
        }
    }
}
