/* Title: WorriorGame.cs
 * Author: Jackson Carder
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A_Worrior_For_Fun
{
    /// <summary>
    /// The main driving class of the game.
    /// </summary>
    public class WorriorGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Sprites and helper variables
        private HealthSprite health;
        private PlayerSprite player;
        private EnemySprite[] enemies;
        private SpriteFont bangers;
        private int numEnimies = 4;

        /// <summary>
        /// Constructorfor the game.
        /// </summary>
        public WorriorGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game and all of it's objects.
        /// </summary>
        protected override void Initialize()
        {
            System.Random rand = new System.Random();
            // TODO: Add your initialization logic here
            health = new HealthSprite();
            player = new PlayerSprite();

            //initializes the enemies
            enemies = new EnemySprite[]
            {
                new EnemySprite(new Vector2(200, 100), false),
                new EnemySprite(new Vector2(410, 130), true),
                new EnemySprite(new Vector2(460, 250), true),
                new EnemySprite(new Vector2(430, 350), false)
            };

            base.Initialize();
        }

        /// <summary>
        /// Loads the Content used intotheir respective places.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            health.LoadContent(Content);
            player.LoadContent(Content);

            //Loads the enemies
            foreach(var enemy in enemies)
            {
                enemy.LoadContent(Content);
            }

            bangers = Content.Load<SpriteFont>("Bangers");
        }

        /// <summary>
        /// The update for the game that makes sure everything is up to par.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);

            //resets color
            player.Color = Color.White;

            foreach(var enemy in enemies)
            {
                enemy.Color = Color.White;
            }
            
            foreach(var enemy in enemies)
            {
                enemy.Update(gameTime);
                if(!enemy.Killed && enemy.Bounds.CollidesWith(player.Bounds))
                {
                    health.Lives--;
                    player.Color = Color.Red;
                    player.Position = new Vector2(100, 250);
                }
                //Yo Jack put a switch statement here to fix the sword bug.
                if(!enemy.Killed && player.Attack != Attack.None && (enemy.Bounds.CollidesWith(player.SwordBoundUp) || enemy.Bounds.CollidesWith(player.SwordBoundRight) || enemy.Bounds.CollidesWith(player.SwordBoundDown) || enemy.Bounds.CollidesWith(player.SwordBoundLeft)))
                {
                    enemy.Color = Color.Red;
                    numEnimies--;
                    enemy.Killed = true;
                }
            }

            if(health.Lives <= 1)
            {
                player.Dead = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the sprites and text for the game.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SeaGreen);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            foreach(var enemy in enemies)
            {
                enemy.Draw(gameTime, _spriteBatch);
            }

            player.Draw(gameTime, _spriteBatch);
            health.Draw(gameTime, _spriteBatch);

            if (numEnimies <= 0)
            {
                _spriteBatch.DrawString(bangers, "YOU WIN!!", new Vector2(50, 3), Color.Gold);
            }
            else if (!player.Dead)
            {
                _spriteBatch.DrawString(bangers, "WASD to move, Arrows to attack.", new Vector2(50, 3), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(bangers, "You are DEAD!!", new Vector2(50, 3), Color.Red);
            }

            
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
