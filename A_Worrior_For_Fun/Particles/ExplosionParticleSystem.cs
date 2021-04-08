/* File: ExplosionParticleSystem.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A_Worrior_For_Fun.Particles
{
    public class ExplosionParticleSystem : ParticleSystem
    {

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="maxExplosions">The maximum number of explosions wanted</param>
        public ExplosionParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 25)
        {

        }

        /// <summary>
        /// Sets up constants
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "explosion";
            minNumParticles = 20;
            maxNumParticles = 25;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        /// <summary>
        /// Initializes a particle
        /// </summary>
        /// <param name="p">a particle</param>
        /// <param name="where">position of particle</param>
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 200);

            var lifetime = RandomHelper.NextFloat(0.5f, 1.0f);

            var acceleration = -velocity / lifetime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            p.Initialize(where, velocity, acceleration, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        /// <summary>
        /// Updates the particles
        /// </summary>
        /// <param name="particle">The particle to update</param>
        /// <param name="dt"></param>
        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
            particle.Color = Color.White * alpha;

            particle.Scale = 0.1f + 0.25f * normalizedLifetime;
        }

        /// <summary>
        /// Places an explosion
        /// </summary>
        /// <param name="where">the position of the explosion</param>
        public void PlaceExplosion(Vector2 where) => AddParticles(where);

    }
}
