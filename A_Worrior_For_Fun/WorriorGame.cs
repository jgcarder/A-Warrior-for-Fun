/* Title: WorriorGame.cs
 * Author: Jackson Carder
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using A_Worrior_For_Fun.Screens;
using A_Worrior_For_Fun.StateManagement;

namespace A_Worrior_For_Fun
{
    /// <summary>
    /// Enum used in driving the states of the game.
    /// </summary>
    public enum LevelState
    {
        Start, 
        Zero,
        One, 
        EndW,
        EndL
    }

    /// <summary>
    /// The main driving class of the game.
    /// </summary>
    public class WorriorGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;

        ////Sprites and helper variables
        //private HealthSprite health;
        //private PlayerSprite player;
        //private EnemySprite[] enemies;
        private SpriteFont bangers;

        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private GamePadState gamePadState;
        private GamePadState previousGamePadState;

        private LevelState levelState = LevelState.Start; //
        private Level0 level0;
        private Level1 level1;

        private Song song1;
        private SoundEffect levelComplete;
        private SoundEffect lose;

        /// <summary>
        /// Constructorfor the game.
        /// </summary>
        public WorriorGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
            //_screenManager.AddScreen(new SplashScreen(), null);
        }

        /// <summary>
        /// Initializes the game and all of it's objects.
        /// </summary>
        protected override void Initialize()
        {
            //System.Random rand = new System.Random();
            //// TODO: Add your initialization logic here
            level0 = new Level0();
            level1 = new Level1();

            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    break;
                case LevelState.Zero:
                    break;
                case LevelState.One:
                    break;
                case LevelState.EndW:
                    break;
                case LevelState.EndL:
                    break;
            }

            base.Initialize();
        }

        /// <summary>
        /// Loads the Content used intotheir respective places.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //// TODO: use this.Content to load your game content here

            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    level0.LoadContent(Content);
                    break;
                case LevelState.Zero:
                    level1.LoadContent(Content);
                    break;
                case LevelState.One:
                    //nothing yet.
                    level1.LoadContent(Content);
                    break;
                case LevelState.EndW:
                    break;
                case LevelState.EndL:
                    break;
            }

            song1 = Content.Load<Song>("sawsquarenoise - Stage 1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(song1);

            levelComplete = Content.Load<SoundEffect>("LevelComplete");
            lose = Content.Load<SoundEffect>("GameOver");

            bangers = Content.Load<SpriteFont>("Bangers");
        }

        /// <summary>
        /// The update for the game that makes sure everything is up to par.
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        protected override void Update(GameTime gameTime)
        {
            previousGamePadState = gamePadState;
            gamePadState = GamePad.GetState(0);
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    if(keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                    {
                        levelState = LevelState.Zero;
                    }
                    break;
                case LevelState.Zero:
                    level0.Update(gameTime);
                    if(level0.Won)
                    {
                        levelComplete.Play();
                        levelState = LevelState.One;
                        level1 = new Level1(level0.HP);
                        LoadContent();
                    }
                    if(level0.Lost)
                    {
                        lose.Play();
                        levelState = LevelState.EndL;
                    }
                    break;
                case LevelState.One:
                    level1.Update(gameTime);
                    if(level1.Won)
                    {
                        levelComplete.Play();
                        levelState = LevelState.EndW;

                    }
                    if (level1.Lost)
                    {
                        lose.Play();
                        levelState = LevelState.EndL;
                    }
                    break;
                case LevelState.EndW:
                    if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                    {
                        levelState = LevelState.Start;
                        level0 = new Level0();
                        LoadContent();
                    }
                    break;
                case LevelState.EndL:
                    if(keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                    {
                        levelState = LevelState.Start;
                        level0 = new Level0();
                        LoadContent();
                    }
                    break;
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

            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    _spriteBatch.DrawString(bangers,      "Welcome Warrior For Fun!", new Vector2(175, 100), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(bangers, "WASD to move and arrows to attack", new Vector2(105, 150), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(bangers,       "Press [Space] to start", new Vector2(195, 200), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    break;
                case LevelState.Zero:
                    level0.Draw(gameTime, _spriteBatch);
                    break;
                case LevelState.One:
                    level1.Draw(gameTime, _spriteBatch);
                    break;
                case LevelState.EndW:
                    _spriteBatch.DrawString(bangers, "You Have WON!!!!", new Vector2(243, 100), Color.Gold, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(bangers, "Press [Space] to play again", new Vector2(135, 150), Color.Gold, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(bangers, "or [Esc] to quit", new Vector2(235, 200), Color.Gold, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    break;
                case LevelState.EndL:
                    _spriteBatch.DrawString(bangers,       "You Have DIED!!!", new Vector2(243, 100), Color.Red, 0f, new Vector2(0,0), 1.25f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(bangers, "Press [Space] to play again", new Vector2(135, 150), Color.Red, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(bangers,       "or [Esc] to quit", new Vector2(235, 200), Color.Red, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
