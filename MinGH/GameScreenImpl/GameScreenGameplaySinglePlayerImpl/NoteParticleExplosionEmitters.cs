using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;
using ProjectMercury.Emitters;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    // Keeps track of the particle emitters themselves and thier explosion locations
    class NoteParticleExplosionEmitters
    {
        public List<CircleEmitter> emitterList = new List<CircleEmitter>();
        public List<Vector2> explosionLocations = new List<Vector2>();

        public void initalizeEmitters()
        {
            CircleEmitter emitterToAdd;

            for (int i = 0; i < 5; i++)
            {
                emitterToAdd = new CircleEmitter();

                emitterToAdd.Budget = 1000;
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

        // NOTE: Fix the 47 magic number sometime
        public void initializeLocations(int noteLeftPadding, int noteWidth, int hitBarYValue)
        {
            for (int i = 0; i < 5; i++)
            {
                int X = noteLeftPadding + (noteWidth * i) + 47;

                explosionLocations.Add(new Vector2(X, hitBarYValue));
            }
        }
    }
}
