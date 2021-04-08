/* Title: EnemySprite2.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using A_Worrior_For_Fun.Collisions;

namespace A_Worrior_For_Fun
{
    /// <summary>
    /// A class for vertical moving enemies
    /// </summary>
    public class EnemySprite2
    {
        private Vector2 position;
        private bool upDown;
        private Texture2D texture;
        private short animationFrame;
        private double timer;
        private double animationTimer;
        private Color color = Color.White;

        private BoundingCircle bounds;

        /// <summary>
        /// The bounds of the collision area for the enemy
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// The color of the sprite
        /// </summary>
        public Color Color { get { return color; } set { color = value; } }

        /// <summary>
        /// Whether it has been killed or not.
        /// </summary>
        public bool Killed { get; set; } = false;

        /// <summary>
        /// The position of the enemysprite
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Constructor used for creating enemies
        /// </summary>
        /// <param name="position">The starting position</param>
        /// <param name="upDown">whether is starts moving up or down</param>
        public EnemySprite2(Vector2 position, bool upDown)
        {
            this.position = position;
            this.upDown = upDown;
            bounds = new BoundingCircle(new Vector2(this.position.X + 16, this.position.Y + 16), 16);
        }

        /// <summary>
        /// Loads the enemy content.
        /// </summary>
        /// <param name="content">The content used in loading</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("colored_packed");
        }

        /// <summary>
        /// Updates the enemie sprites.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            //Switches direction
            if (timer > 2.0)
            {
                if (upDown) upDown = false;
                else upDown = true;
                timer -= 2.0;
            }

            if (!Killed)
            {
                switch (upDown)
                {
                    case true:
                        position += new Vector2(0, -1.2f) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                    case false:
                        position += new Vector2(0, 1.2f) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        break;
                }
            }


            bounds.Center = position;
        }

        /// <summary>
        /// Draws the enemy sprites.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The strite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle();
            SpriteEffects spriteEffect = new SpriteEffects();
            //updates the animation
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 5) animationFrame = 1;
                animationTimer -= 0.2;
            }

            //checks to see if an enemy is killed
            if (Killed)
            {
                source = new Rectangle(368, 112, 16, 16);
            }
            else
            {
                source = new Rectangle((animationFrame * 16) + 272, 112, 16, 16);
                spriteEffect = (upDown) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }

            spriteBatch.Draw(texture, position, source, color, 0f, new Vector2(16, 16), 2f, spriteEffect, 0f);
        }

    }
}
