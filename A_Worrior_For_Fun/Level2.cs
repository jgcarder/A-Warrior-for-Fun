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
    public class Level2
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
        private int numEnemies = 14;
        private bool won = false;
        private bool lost = false;
        private int hp;

        //Moving sprites and health
        private EnemySprite[] enemiesH;
        private EnemySprite2[] enemiesV;
        private EnemySprite3[] enemiesO;
        private HealthSprite health;
        private PlayerSprite player;

        //Textures
        private Texture2D texture;
        private Texture2D _background;
        private Texture2D _clouds;

        //Sound stuff
        private SoundEffect playerHit;
        private SoundEffect basicEnemyHit;

        /// <summary>
        /// Constructor that Takes in an int.
        /// </summary>
        /// <param name="previousHp">The Hp from the previous level</param>
        public Level2()
        {
            System.Random rand = new Random();

            health = new HealthSprite();
            hp = health.Lives;
            player = new PlayerSprite();
            player.WorldLength = 2000;

            enemiesH = new EnemySprite[]
            {
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), true),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), true),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), false),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), false),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), false)
            };
            enemiesV = new EnemySprite2[]
            {
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), true),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), true),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), false),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), false),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), true)
            };
            enemiesO = new EnemySprite3[]
            {
                new EnemySprite3(new Vector2((float)rand.Next(100, 1900), (float)rand.Next(50, 450))),
                new EnemySprite3(new Vector2((float)rand.Next(100, 1900), (float)rand.Next(50, 450))),
                new EnemySprite3(new Vector2((float)rand.Next(100, 1900), (float)rand.Next(50, 450))),
                new EnemySprite3(new Vector2((float)rand.Next(100, 1900), (float)rand.Next(50, 450)))
            };
        }

        /// <summary>
        /// Constructor that Takes in an int.
        /// </summary>
        /// <param name="previousHp">The Hp from the previous level</param>
        public Level2(int previousHp)
        {
            System.Random rand = new Random();

            health = new HealthSprite(previousHp);
            hp = previousHp;
            player = new PlayerSprite();
            player.WorldLength = 2000;

            enemiesH = new EnemySprite[]
            {
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), true),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), true),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), false),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), false),
                new EnemySprite(new Vector2((float)rand.Next(200, 1800), (float)rand.Next(50, 450)), false)
            };
            enemiesV = new EnemySprite2[]
            {
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), true),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), true),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), false),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), false),
                new EnemySprite2(new Vector2((float)rand.Next(50, 1950), (float)rand.Next(200, 300)), true)
            };
            enemiesO = new EnemySprite3[]
            {
                new EnemySprite3(new Vector2((float)rand.Next(250, 1900), (float)rand.Next(50, 450))),
                new EnemySprite3(new Vector2((float)rand.Next(250, 1900), (float)rand.Next(50, 450))),
                new EnemySprite3(new Vector2((float)rand.Next(250, 1900), (float)rand.Next(50, 450))),
                new EnemySprite3(new Vector2((float)rand.Next(250, 1900), (float)rand.Next(50, 450)))
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
            foreach (var enemy in enemiesO)
            {
                enemy.LoadContent(content);
            }

            texture = content.Load<Texture2D>("colored_packed");
            _background = content.Load <Texture2D>("Level 3 background");
            _clouds = content.Load<Texture2D>("Clouds");
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
            foreach (var enemy in enemiesO)
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
            foreach (var enemy in enemiesO)
            {
                enemy.Color = Color.White;
            }

            //EnemyO activation
            foreach (var enemy in enemiesO)
            {
                enemy.PlayerPos = player.Position;
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
            foreach (var enemy in enemiesO)
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
            float playerX = MathHelper.Clamp(player.Position.X, 350, 2000);
            float offsetX = 350 - playerX;

            Matrix transform;

            //Background & Playground
            transform = Matrix.CreateTranslation(offsetX, 0, 0);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transform);
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            //Drawing enemies
            foreach (var enemy in enemiesH)
            {
                enemy.Draw(gameTime, spriteBatch);
            }
            foreach (var enemy in enemiesV)
            {
                enemy.Draw(gameTime, spriteBatch);
            }
            foreach (var enemy in enemiesO)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            //Drawing player and health
            player.Draw(gameTime, spriteBatch);
            if(offsetX <= 0)
            {
                float tempX = Math.Abs(offsetX);
                health.Position = new Vector2(tempX += health.Position.X, health.Position.Y);
            }
            else
            {
                health.Position = new Vector2(3, 432);
            }
            health.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            transform = Matrix.CreateTranslation(offsetX * 1.1f, 0, 0);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transform);
            spriteBatch.Draw(_clouds, Vector2.Zero, Color.White);
            spriteBatch.End();

        }

    }
}
