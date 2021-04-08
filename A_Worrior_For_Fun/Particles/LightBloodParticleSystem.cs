/* File: LightBloodParticleSystem.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A_Worrior_For_Fun.Particles
{
    public class LightBloodParticleSystem : ParticleSystem
    {

        private Color _color = Color.DarkRed;

        public LightBloodParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 40)
        {

        }

        /// <summary>
        /// Sets up the constants
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "circle";
            minNumParticles = 20;
            maxNumParticles = 25;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        /// <summary>
        /// Sets up the particles
        /// </summary>
        /// <param name="p">The particle</param>
        /// <param name="where">Where to place them</param>
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 200);

            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);

            var acceleration = -velocity / lifetime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            var scale = RandomHelper.NextFloat(4, 6);

            p.Initialize(where, velocity, acceleration, _color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);

        }

        /// <summary>
        /// Updates the particle
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="dt"></param>
        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = 0.1f + 0.25f * normalizedLifetime;
        }

        /// <summary>
        /// Places the firework
        /// </summary>
        /// <param name="where">where to place it</param>
        public void PlaceFirework(Vector2 where)
        {
            AddParticles(where);
        }

    }
}
