/* File: EnemySprite3.cs
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
    /// A class for the enemy that moves in ALL directions
    /// </summary>
    public class EnemySprite3
    {

        private Vector2 position;
        private bool isActive;
        private bool leftRight = false;
        private Texture2D texture;
        private short animationFrame;
        private double timer;
        private double animationTimer;
        private Color color = Color.White;

        private const int _radius = 110;

        private Vector2 playerPos = Vector2.Zero;

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
        /// The position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// The player position when nearby.
        /// </summary>
        public Vector2 PlayerPos
        {
            get
            {
                return playerPos;
            }

            set
            {
                playerPos = value;
            }
        }

        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// The constructor for the new enemy
        /// </summary>
        /// <param name="position">The starting position</param>
        public EnemySprite3 (Vector2 position)
        {
            this.position = position;
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
        /// Updates the enemy sprite
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            
            double distance = Math.Sqrt(Math.Pow((bounds.Center.X - playerPos.X), 2) + Math.Pow((bounds.Center.Y - playerPos.Y), 2));
            //double xDist = Math.Abs(bounds.Center.X - playerPos.X);
            //double yDist = Math.Abs(bounds.Center.Y = playerPos.Y);

            if (distance <= _radius)
            {
                isActive = true;
            }
            //if (xDist <= 50 && yDist <= 50)
            //{
            //    isActive = true;
            //}  
            else isActive = false;

            if (!Killed) //if alive
            {
                if(isActive) //if active
                {
                    double x = playerPos.X - position.X;
                    double y = playerPos.Y - position.Y;

                    if (x <= 0) leftRight = true;
                    else leftRight = false;

                    if (Math.Abs(x) >= Math.Abs(y))
                    {
                        x = x / Math.Abs(y);
                        y = y / Math.Abs(y);
                    }
                    else
                    {
                        y = y / Math.Abs(x);
                        x = x / Math.Abs(x);
                    }

                    if (y >= 2) y = 2;
                    if (x >= 2) x = 2;
                    if (y <= -2) y = -2;
                    if (x <= -2) x = -2;
                    position += new Vector2((float)x, (float)y) * 30 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                source = new Rectangle(368, 128, 16, 16);
            }
            else
            {
                if(isActive)
                {
                    spriteEffect = (leftRight) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    source = new Rectangle((animationFrame * 16) + 272, 128, 16, 16);
                }
                else
                {
                    source = new Rectangle((1 * 16) + 272, 128, 16, 16);
                }

            }

            spriteBatch.Draw(texture, position, source, color, 0f, new Vector2(16, 16), 2f, spriteEffect, 0f);
        }

    }
}
