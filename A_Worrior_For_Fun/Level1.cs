/* Title: Level1.cs
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace A_Worrior_For_Fun
{
    /// <summary>
    /// A Level class for the game
    /// </summary>
    public class Level1
    {
        /// <summary>
        /// Bool used to see if the player has won.
        /// </summary>
        public bool Won { get { return won; } }

        /// <summary>
        /// Bool used to see if the player has lost.
        /// </summary>
        public bool Lost { get { return lost; } }

        public int Health { get { return hp; } }

        //Assistant variables
        private int numEnemies = 8;
        private bool won = false;
        private bool lost = false;
        private int hp;

        //Moving sprites and health
        private EnemySprite[] enemiesH;
        private EnemySprite2[] enemiesV;
        private HealthSprite health;
        private PlayerSprite player;

        //Textures
        private Texture2D texture;

        //Sound stuff
        private SoundEffect playerHit;
        private SoundEffect basicEnemyHit;

        /// <summary>
        /// Constructor for the level.
        /// </summary>
        public Level1()
        {
            System.Random rand = new Random();

            health = new HealthSprite();
            hp = health.Lives;
            player = new PlayerSprite();

            enemiesH = new EnemySprite[]
            {
                new EnemySprite(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), true),
                new EnemySprite(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), true),
                new EnemySprite(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), false),
                new EnemySprite(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), false)
            };
            enemiesV = new EnemySprite2[]
            {
                new EnemySprite2(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), true),
                new EnemySprite2(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), true),
                new EnemySprite2(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), false),
                new EnemySprite2(new Vector2((float)rand.Next(100, 700), (float)rand.Next(20, 475)), false)
            };
        }

        /// <summary>
        /// Constructor that Takes in an int.
        /// </summary>
        /// <param name="previousHp">The Hp from the previous level</param>
        public Level1(int previousHp)
        {
            System.Random rand = new Random();

            health = new HealthSprite(previousHp);
            hp = previousHp;
            player = new PlayerSprite();

            enemiesH = new EnemySprite[]
            {
                new EnemySprite(new Vector2((float)rand.Next(200, 600), (float)rand.Next(50, 450)), true),
                new EnemySprite(new Vector2((float)rand.Next(200, 600), (float)rand.Next(50, 450)), true),
                new EnemySprite(new Vector2((float)rand.Next(200, 600), (float)rand.Next(50, 450)), false),
                new EnemySprite(new Vector2((float)rand.Next(200, 600), (float)rand.Next(50, 450)), false)
            };
            enemiesV = new EnemySprite2[]
            {
                new EnemySprite2(new Vector2((float)rand.Next(50, 750), (float)rand.Next(200, 300)), true),
                new EnemySprite2(new Vector2((float)rand.Next(50, 750), (float)rand.Next(200, 300)), true),
                new EnemySprite2(new Vector2((float)rand.Next(50, 750), (float)rand.Next(200, 300)), false),
                new EnemySprite2(new Vector2((float)rand.Next(50, 750), (float)rand.Next(200, 300)), false)
            };
        }

        /// <summary>
        /// Loads the level's content.
        /// </summary>
        /// <param name="content">The content for the level</param>
        public void LoadContent(ContentManager content)
        {
            playerHit = content.Load<SoundEffect>("PlayerHit");
            basicEnemyHit = content.Load<SoundEffect>("BasicEnemyHit");

            health.LoadContent(content);
            player.LoadContent(content);

            foreach (var enemy in enemiesH)
            {
                enemy.LoadContent(content);
            }
            foreach (var enemy in enemiesV)
            {
                enemy.LoadContent(content);
            }

            texture = content.Load<Texture2D>("colored_packed");
        }

        /// <summary>
        /// Updates the level.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public void Update(GameTime gameTime)
        {
            //Updateing assets
            health.Update(gameTime);
            player.Update(gameTime);

            foreach (var enemy in enemiesH)
            {
                enemy.Update(gameTime);
            }
            foreach (var enemy in enemiesV)
            {
                enemy.Update(gameTime);
            }

            //resetting colors
            player.Color = Color.White;
            foreach (var enemy in enemiesH)
            {
                enemy.Color = Color.White;
            }
            foreach (var enemy in enemiesV)
            {
                enemy.Color = Color.White;
            }

            //Enemies collision
            foreach (var enemy in enemiesH)
            {
                enemy.Update(gameTime);

                switch (player.Attack)
                {
                    case Attack.Up:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundUp))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();

                        }
                        break;
                    case Attack.Right:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundRight))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.Down:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundDown))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.Left:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundLeft))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.None:
                        //Do nothing
                        break;
                }

                if (!enemy.Killed && enemy.Bounds.CollidesWith(player.Bounds))
                {
                    health.Lives--;
                    hp--;
                    player.Color = Color.Red;
                    player.Position = new Vector2(100, 250);
                    playerHit.Play();
                }

            }
            foreach (var enemy in enemiesV)
            {
                enemy.Update(gameTime);

                switch (player.Attack)
                {
                    case Attack.Up:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundUp))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.Right:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundRight))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.Down:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundDown))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.Left:
                        if (!enemy.Killed && enemy.Bounds.CollidesWith(player.SwordBoundLeft))
                        {
                            enemy.Color = Color.Red;
                            numEnemies--;
                            enemy.Killed = true;
                            basicEnemyHit.Play();
                        }
                        break;
                    case Attack.None:
                        //Do nothing
                        break;
                }

                if (!enemy.Killed && enemy.Bounds.CollidesWith(player.Bounds))
                {
                    health.Lives--;
                    hp--;
                    player.Color = Color.Red;
                    player.Position = new Vector2(100, 250);
                    playerHit.Play();
                }

            }

            //Checking ending conditions
            if (health.Lives <= 1)
            {
                player.Dead = true;
                lost = true;
            }
            if (numEnemies <= 0)
            {
                won = true;
            }
        }

        /// <summary>
        /// Draws all of the assets for the first level.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="spriteBatch">The sprite batch used in drawing</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Drawing environment
            var source1 = new Rectangle(64 + 16 * 1, 0, 16, 16);
            var source2 = new Rectangle(64 + 16 * 2, 0, 16, 16);
            var source3 = new Rectangle(64 + 16 * 3, 0, 16, 16);
            var source4 = new Rectangle(80, 32, 16, 16);
            spriteBatch.Draw(texture, new Vector2(77, 44), source1, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(700, 300), source1, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(69, 444), source1, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(123, 200), source1, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(221, 88), source1, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(599, 111), source2, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(600, 400), source2, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(326, 214), source2, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(666, 99), source2, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(428, 275), source2, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(497, 7), source3, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(555, 470), source3, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(710, 460), source3, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(200, 50), source3, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(501, 66), source3, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(110, 110), source4, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(510, 234), source4, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(400, 250), source4, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, new Vector2(400, 767), source4, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);

            //Drawing enemies
            foreach (var enemy in enemiesH)
            {
                enemy.Draw(gameTime, spriteBatch);
            }
            foreach (var enemy in enemiesV)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            //Drawing player and health
            player.Draw(gameTime, spriteBatch);
            health.Draw(gameTime, spriteBatch);

        }
    }
}
