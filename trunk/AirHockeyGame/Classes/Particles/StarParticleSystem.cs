#region File Description
//-----------------------------------------------------------------------------
// ExplosionSmokeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace AirHockeyGame
{
    /// <summary>
    /// ExplosionSmokeParticleSystem is a specialization of ParticleSystem which
    /// creates a circular pattern of smoke. It should be combined with
    /// ExplosionParticleSystem for best effect.
    /// </summary>
    public class StarParticleSystem : ParticleSystem
    {
        public StarParticleSystem(AirHockey game, int howManyEffects)
            : base(game, howManyEffects)
        {
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "star";

            // less initial speed than the explosion itself
            minInitialSpeed = 50;
            maxInitialSpeed = 80;

            // acceleration is negative, so particles will accelerate away from the
            // initial velocity.  this will make them slow down, as if from wind
            // resistance. we want the smoke to linger a bit and feel wispy, though,
            // so we don't stop them completely like we do ExplosionParticleSystem
            // particles.
            minAcceleration = -20;
            maxAcceleration = -50;

            // explosion smoke lasts for longer than the explosion itself, but not
            // as long as the plumes do.
            minLifetime = 0.3f;
            maxLifetime = 0.5f;

            minScale = 0.4f;
            maxScale = .7f;

            minNumParticles = 10;
            maxNumParticles = 15;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            spriteBlendMode = SpriteBlendMode.Additive;

            DrawOrder = AlphaBlendDrawOrder;
        }
    }
}
