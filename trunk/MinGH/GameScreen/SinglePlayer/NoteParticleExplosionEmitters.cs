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
    public class NoteParticleExplosionEmitters
    {
		/// <summary>
		/// The list of the actual emitters.  There should only be 5 for MinGH, but a list
		/// is used in case of future note additions.
		/// </summary>
        public List<CircleEmitter> emitterList { get; set; }
		
		/// <summary>
		/// A list that mirrors the emitterList, but instead stores the locations for each
		/// particle emitter.  The two lists should be encapsulated into one list contating
		/// a custom class...
		/// </summary>
        public List<Vector2> explosionLocations { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NoteParticleExplosionEmitters()
        {
            emitterList = new List<CircleEmitter>();
            explosionLocations = new List<Vector2>();
        }

		/// <summary>
		/// Initializes each of the emitters and adds them to the emitter list.
		/// To know more about the emitters themselves, look at the Mercury
		/// Particle Engine documentation.
		/// </summary>
        public void initalizeEmitters()
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
                int X = themeSetting.distanceUntilLeftMostLaneGuitarSingle + (themeSetting.laneSize * i - 1) + 
                        (themeSetting.laneBorderSize * i) + (themeSetting.laneBorderSize / 2);

                explosionLocations.Add(new Vector2(X, hitBarYValue));
            }
        }
    }
}
