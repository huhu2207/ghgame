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

        public void initalizeEmittersDrumsSingle(ThemeSetting themeSetting, GraphicsDevice graphics, Matrix viewMatrix,
                                                 Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.Identity;
            float theZ = -themeSetting.hitMarkerDepth - (themeSetting.hitMarkerSize / 2.0f);

            // We have to move the bass pedal emitter to the center of the drum lanes
            int theX1 = 0;
            int theX2 = (themeSetting.laneSizeDrums * 4) + (themeSetting.laneSeparatorSize * 3);
            Vector3 leftPoint = new Vector3(theX1, 0.0f, theZ);
            Vector3 rightPoint = new Vector3(theX2, 0.0f, theZ);
            Vector3 leftScreenSpaceVector = graphics.Viewport.Project(leftPoint, projectionMatrix, viewMatrix, worldMatrix);
            Vector3 rightScreenSpaceVector = graphics.Viewport.Project(rightPoint, projectionMatrix, viewMatrix, worldMatrix);
            float length = rightScreenSpaceVector.X - leftScreenSpaceVector.X;

            LineEmitter lineEmitterToAdd = new LineEmitter();
            lineEmitterToAdd.Budget = 3000;
            lineEmitterToAdd.ReleaseQuantity = 1500;
            lineEmitterToAdd.ReleaseScale = new VariableFloat { Anchor = 40f, Variation = 0.5f };
            lineEmitterToAdd.ReleaseSpeed = new VariableFloat { Anchor = 300f, Variation = 1f };
            lineEmitterToAdd.ReleaseOpacity = new VariableFloat { Anchor = 0.2f, Variation = 0f };
            lineEmitterToAdd.Term = 0.2f;
            lineEmitterToAdd.ParticleTextureAssetName = "Particles\\FlowerBurst";
            lineEmitterToAdd.ReleaseColour = Color.Orange.ToVector3();
            lineEmitterToAdd.Length = (int)length;
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
                circleEmitterToAdd.Term = 0.2f;
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
        public void initializeLocationsGuitarSingle(ThemeSetting themeSetting, GraphicsDevice graphics, Matrix viewMatrix,
                                                    Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.Identity;
            float theZ = -themeSetting.hitMarkerDepth - (themeSetting.hitMarkerSize / 2.0f);

            for (int i = 0; i < 5; i++)
            {
                int theX = (themeSetting.laneSizeGuitar / 2) + (themeSetting.laneSizeGuitar * i) +
                        (themeSetting.laneSeparatorSize * i);
                Vector3 vectorToUse = new Vector3(theX, 0.0f, theZ);
                Vector3 screenSpaceVector = graphics.Viewport.Project( vectorToUse, projectionMatrix, viewMatrix, worldMatrix);
                explosionLocations.Add(new Vector2(screenSpaceVector.X, screenSpaceVector.Y));
            }
        }

        public void initializeLocationsDrumsSingle(ThemeSetting themeSetting, GraphicsDevice graphics, Matrix viewMatrix,
                                                   Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.Identity;
            float theZ = -themeSetting.hitMarkerDepth - (themeSetting.hitMarkerSize / 2.0f);

            // We have to move the bass pedal emitter to the center of the drum lanes
            int theX = (themeSetting.laneSizeDrums * 2) + themeSetting.laneSeparatorSize;
            Vector3 vectorToUse = new Vector3(theX, 0.0f, theZ);
            Vector3 screenSpaceVector = graphics.Viewport.Project(vectorToUse, projectionMatrix, viewMatrix, worldMatrix);

            explosionLocations.Add(new Vector2(screenSpaceVector.X, screenSpaceVector.Y));

            for (int i = 0; i < 4; i++)
            {
                theX = (themeSetting.laneSizeDrums / 2) + (themeSetting.laneSizeDrums * i) +
                        (themeSetting.laneSeparatorSize * i);
                vectorToUse = new Vector3(theX, 0.0f, theZ);
                screenSpaceVector = graphics.Viewport.Project(vectorToUse, projectionMatrix, viewMatrix, worldMatrix);

                explosionLocations.Add(new Vector2(screenSpaceVector.X, screenSpaceVector.Y));
            }
        }
    }
}
