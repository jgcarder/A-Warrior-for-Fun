/* File: RainParticleSystem.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace A_Worrior_For_Fun.Particles
{
    public class RainParticleSystem : ParticleSystem
    {

        private Rectangle _source;

        /// <summary>
        /// if its raining or not
        /// </summary>
        public bool IsRaining { get; set; } = true;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="source">The source of the rain</param>
        public RainParticleSystem(Game game, Rectangle source) : base(game, 1200)
        {
            _source = source;
        }

        /// <summary>
        /// Sets up the constants
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "drop";
            minNumParticles = 5;
            maxNumParticles = 10;
        }

        /// <summary>
        /// Sets up the particles
        /// </summary>
        /// <param name="p">the particles</param>
        /// <param name="where">Where to place them</param>
        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 260, Vector2.Zero, Color.LightSkyBlue, scale: RandomHelper.NextFloat(0.1f, 0.4f), lifetime: 3);
        }

        /// <summary>
        /// Updates the particle set
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsRaining)
            {
                AddParticles(_source);
            }
        }

    }
}
