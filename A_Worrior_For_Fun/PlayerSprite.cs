/* Title: PlayerSprite.cs
 * Author: Jack Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using A_Worrior_For_Fun.Collisions;

namespace A_Worrior_For_Fun
{
    /// <summary>
    /// An enum set up for smooth attack operations.
    /// </summary>
    public enum Attack
    {
        Up,
        Right,
        Down,
        Left,
        None,
    }

    /// <summary>
    /// A class the holds all of the data for a player sprite
    /// </summary>
    public class PlayerSprite
    {
        //State updating
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        private GamePadState gamePadState;
        private GamePadState previousGamePadState;

        //Textures
        private Texture2D texture;
        private Texture2D attackTexture;

        //Accessories
        private bool flipped;
        private bool dead;

        private Vector2 position = new Vector2(100, 250);
        private Vector2 velocity;

        private Attack attack;

        //The boundings for collision
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(100-14, 250-15), 16, 16);

        private BoundingRectangle swordBoundUp = new BoundingRectangle(new Vector2(100 + 3, 250 - 48), 8, 32);
        private BoundingRectangle swordBoundRight = new BoundingRectangle(new Vector2(100 + 16, 250 - 2), 32, 8);
        private BoundingRectangle swordBoundDown = new BoundingRectangle(new Vector2(100, 250 + 16), 8, 32);
        private BoundingRectangle swordBoundLeft = new BoundingRectangle(new Vector2(100 - 48, 250 - 15), 32, 8);

        private int _worldLength;

        //Sound effects
        private SoundEffect swordSwing;

        /// <summary>
        /// The collision bounds for the player
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// The collision bounds for a up attack.
        /// </summary>
        public BoundingRectangle SwordBoundUp => swordBoundUp;

        /// <summary>
        /// The collision bounds for a right attack.
        /// </summary>
        public BoundingRectangle SwordBoundRight => swordBoundRight;

        /// <summary>
        /// The collision bounds for a down attack.
        /// </summary>
        public BoundingRectangle SwordBoundDown => swordBoundDown;

        /// <summary>
        /// The collision bounds for a left attack.
        /// </summary>
        public BoundingRectangle SwordBoundLeft => swordBoundLeft;

        public int WorldLength
        {
            get
            {
                return _worldLength;
            }
            set
            {
                _worldLength = value;
            }
        }

        /// <summary>
        /// The attack direction property for the player.
        /// </summary>
        public Attack Attack { get { return attack; } set { attack = value; } }

        /// <summary>
        /// The variabel stating if the player is dead or not.
        /// </summary>
        public bool Dead { get { return dead; } set { dead = value; } }

        /// <summary>
        /// The current color overlay of the player.
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// The current position of the player.
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }

        /// <summary>
        /// Loads the content for the sprite.
        /// </summary>
        /// <param name="content">The content used in loading.</param>
        public void LoadContent(ContentManager content)
        {
            velocity = new Vector2(0,0);

            swordSwing = content.Load<SoundEffect>("Hit_Hurt10");
            

            texture = content.Load<Texture2D>("colored_packed");
            attackTexture = content.Load<Texture2D>("colored_packed");
        }

        /// <summary>
        /// Makes sure the sprite is updated properly.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //updating the game input data
            previousGamePadState = gamePadState;
            gamePadState = GamePad.GetState(0);

            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 acceleration = new Vector2(0,0);

            //User input
            if(!dead)
            {
                //Gamepad input
                position += gamePadState.ThumbSticks.Left * new Vector2(1, 1);
                if (gamePadState.ThumbSticks.Left.X < 0)
                {
                    flipped = true;
                }
                if (gamePadState.ThumbSticks.Left.X > 0)
                {
                    flipped = false;
                }



                //Keyboard input
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    //position += new Vector2(0, -2);
                    acceleration.Y += -1;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    //position += new Vector2(0, 2);
                    acceleration.Y += 1;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    //position += new Vector2(2, 0);
                    acceleration.X += 1;
                    flipped = false;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    //position += new Vector2(-2, 0);
                    acceleration.X += -1;
                    flipped = true;
                }

                velocity += acceleration * t * 150;
                position += velocity * t;
                velocity *= 0.98f;
                if(Math.Abs(velocity.X) <= 2)
                {
                    velocity.X = 0;
                }
                if (Math.Abs(velocity.Y) <= 2)
                {
                    velocity.Y = 0;
                }

                //Attacking input
                if (keyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    attack = Attack.Up;
                    swordSwing.Play();
                }
                else if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
                {
                    attack = Attack.Right;
                    flipped = false;
                    swordSwing.Play();
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    attack = Attack.Down;
                    swordSwing.Play();
                }
                else if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
                {
                    attack = Attack.Left;
                    flipped = true;
                    swordSwing.Play();
                }
                else //if(keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.Left))
                {
                    attack = Attack.None;
                }

                if(position.X < 25)
                {
                    position = new Vector2(25, position.Y);
                }
                if(position.Y < 25)
                {
                    position = new Vector2(position.X, 25);
                }
                if(position.X > _worldLength - 100)
                {
                    position = new Vector2(_worldLength - 100, position.Y);
                }
                if(position.Y > 480)
                {
                    position = new Vector2(position.X, 480);
                }

            }

            //collision
            bounds.X = position.X - 14;
            bounds.Y = position.Y - 16;

            swordBoundUp.X = position.X + 3;
            swordBoundUp.Y = position.Y - 48;
            swordBoundRight.X = position.X + 16;
            swordBoundRight.Y = position.Y - 2;
            swordBoundDown.X = position.X;
            swordBoundDown.Y = position.Y + 16;
            swordBoundLeft.X = position.X - 48;
            swordBoundLeft.Y = position.Y - 15;
        }

        /// <summary>
        /// Draws the sprite and it's accessories
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="spriteBatch">The spriteBatch used in drawing</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Flipping the player
            SpriteEffects spriteEffects = (!flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var source = new Rectangle(31*16, 0, 16, 16);
            spriteBatch.Draw(texture, position, source, Color, 0, new Vector2(16, 16), 2f, spriteEffects, 0);

            //Drawing the sword in the right spots.
            var source2 = new Rectangle(33 * 16, 8 * 16, 16, 16);
            switch (attack)
            {
                case Attack.Up:
                    spriteBatch.Draw(attackTexture, new Vector2(position.X + 8, position.Y - 48), source2, Color.Gold, -1 * MathHelper.PiOver4, new Vector2(16, 16), 2f, SpriteEffects.None, 0);
                    break;
                case Attack.Right:
                    spriteBatch.Draw(attackTexture, new Vector2(position.X+16, position.Y+8), source2, Color.Gold, MathHelper.PiOver4, new Vector2(16, 16), 2f, SpriteEffects.None, 0);
                    break;
                case Attack.Down:
                    spriteBatch.Draw(attackTexture, new Vector2(position.X-37, position.Y+16), source2, Color.Gold, 3 * MathHelper.PiOver4, new Vector2(16, 16), 2f, SpriteEffects.None, 0);
                    break;
                case Attack.Left:
                    spriteBatch.Draw(attackTexture, new Vector2(position.X-48, position.Y-37), source2, Color.Gold, -3 * MathHelper.PiOver4, new Vector2(16, 16), 2f, SpriteEffects.None, 0);

                    break;
                case Attack.None:
                    break;
            }

        }

    }
}
