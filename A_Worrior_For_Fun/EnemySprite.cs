/* Title: EnemySprite.cs
 * Author: Jack Carder
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
    /// The class assisting in the creation of enemy sprites 
    /// </summary>
    public class EnemySprite
    {
        private Vector2 position;
        private bool leftRight;
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
        /// Constructor used for creating enemies
        /// </summary>
        /// <param name="position">The starting position</param>
        /// <param name="leftRight">whether is starts moving left or right</param>
        public EnemySprite(Vector2 position, bool leftRight)
        {
            this.position = position;
            this.leftRight = leftRight;
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
            if(timer > 2.0)
            {
                if (leftRight) leftRight = false;
                else leftRight = true;
                timer -= 2.0;
            }

            switch(leftRight)
            {
                case true:
                    position += new Vector2(-1.5f, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case false:
                    position += new Vector2(1.5f, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
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

            //checks to see if an enemy is killed
            if(Killed)
            {
                return;
            }

            //updates the animation
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 5) animationFrame = 1;
                animationTimer -= 0.2;
            }

            SpriteEffects spriteEffect = (leftRight) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            var source = new Rectangle((animationFrame*16) + 272, 112, 16, 16);
            spriteBatch.Draw(texture, position, source, color, 0f, new Vector2(16, 16), 2f, spriteEffect, 0f);
        }

    }
}
