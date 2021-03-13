/* Title: GameplayScreen.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using A_Worrior_For_Fun.StateManagement;

namespace A_Worrior_For_Fun.Screens
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
    /// The screen that houses the game
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private LevelState levelState = LevelState.Start; //Needs to be changed to reflect new system.
        private Level0 level0;
        private Level1 level1;

        //private Song song1;
        private SoundEffect levelComplete;
        private SoundEffect lose;
        private SpriteFont bangers;

        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private GamePadState gamePadState;
        private GamePadState previousGamePadState;


        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        /// <summary>
        /// The constructor for the GameplayScreen
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            level0 = new Level0();
            level1 = new Level1();
        }

        /// <summary>
        /// A helper method that lets the game re-load its data/levels.
        /// </summary>
        private void LoadState()
        {
            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    level0.LoadContent(_content);
                    break;
                case LevelState.Zero:
                    level1.LoadContent(_content);
                    break;
                case LevelState.One:
                    level1.LoadContent(_content);
                    break;
                case LevelState.EndW:
                    break;
                case LevelState.EndL:
                    break;
            }

            //song1 = _content.Load<Song>("sawsquarenoise - Stage 1");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Volume = 0.25f;
            //MediaPlayer.Play(song1);

            levelComplete = _content.Load<SoundEffect>("LevelComplete");
            lose = _content.Load<SoundEffect>("GameOver");

            bangers = _content.Load<SpriteFont>("Bangers");

            Thread.Sleep(1000);
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Loads the content of the screen
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("Bangers");
            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    level0.LoadContent(_content);
                    break;
                case LevelState.Zero:
                    level1.LoadContent(_content);
                    break;
                case LevelState.One:
                    //nothing yet.
                    level1.LoadContent(_content);
                    break;
                case LevelState.EndW:
                    break;
                case LevelState.EndL:
                    break;
            }

            //song1 = _content.Load<Song>("sawsquarenoise - Stage 1");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Volume = 0.25f;
            //MediaPlayer.Play(song1);

            levelComplete = _content.Load<SoundEffect>("LevelComplete");
            lose = _content.Load<SoundEffect>("GameOver");

            bangers = _content.Load<SpriteFont>("Bangers");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Deactivates the screen
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unloads the screen's data
        /// </summary>
        public override void Unload()
        {
            _content.Unload();
        }

        /// <summary>
        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.        
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="otherScreenHasFocus">A bool for is another screen has focus</param>
        /// <param name="coveredByOtherScreen">A bool for if another screen covers this one</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);


            previousGamePadState = gamePadState;
            gamePadState = GamePad.GetState(0);
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Is called when the screen is active
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="input">The input state object</param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                //Levels state machine
                switch (levelState)
                {
                    case LevelState.Start:
                        if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                        {
                            levelState = LevelState.Zero;
                        }
                        break;
                    case LevelState.Zero:
                        level0.Update(gameTime);
                        if (level0.Won)
                        {
                            levelComplete.Play();
                            levelState = LevelState.One;
                            level1 = new Level1(level0.HP);
                            LoadState();
                        }
                        if (level0.Lost)
                        {
                            lose.Play();
                            levelState = LevelState.EndL;
                        }
                        break;
                    case LevelState.One:
                        level1.Update(gameTime);
                        if (level1.Won)
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
                            MainMenuScreen mms = new MainMenuScreen();
                            mms.ContentPasser(_content);

                            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), mms);
                        }
                        break;
                    case LevelState.EndL:
                        if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                        {
                            MainMenuScreen mms = new MainMenuScreen();
                            mms.ContentPasser(_content);

                            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), mms);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Draws the Screen
        /// </summary>
        /// <param name="gameTime">the game's time</param>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.SeaGreen, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Levels state machine
            switch (levelState)
            {
                case LevelState.Start:
                    spriteBatch.DrawString(bangers, "Welcome Warrior For Fun!", new Vector2(175, 100), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "WASD to move and arrows to attack", new Vector2(105, 150), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "Press [Space] to Begin!", new Vector2(195, 200), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    break;
                case LevelState.Zero:
                    level0.Draw(gameTime, spriteBatch);
                    break;
                case LevelState.One:
                    level1.Draw(gameTime, spriteBatch);
                    break;
                case LevelState.EndW:
                    spriteBatch.DrawString(bangers, "You Have WON!!!!", new Vector2(243, 100), Color.Gold, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "Press [Space] to go to the main menu", new Vector2(30, 150), Color.Gold, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "or [Esc] to open the menu", new Vector2(150, 200), Color.Gold, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    break;
                case LevelState.EndL:
                    spriteBatch.DrawString(bangers, "You Have DIED!!!", new Vector2(243, 100), Color.Red, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "Press [Space] to go to the main menu", new Vector2(30, 150), Color.Red, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "or [Esc] to open the menu", new Vector2(150, 200), Color.Red, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                    break;
            }

            //spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
            //spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
            //                      _enemyPosition, Color.DarkRed);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
