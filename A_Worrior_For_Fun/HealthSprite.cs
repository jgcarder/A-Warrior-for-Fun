/* Title: HealthSprite.cs
 * Author: Jack Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace A_Worrior_For_Fun
{
    /// <summary>
    /// A class supporting the creation of a health sprite
    /// </summary>
    public class HealthSprite
    {
        /// <summary>
        /// The counter for the lives a player has. 3 total: 4, 3, 2, 1=dead
        /// </summary>
        public int Lives = 4;

        private Texture2D texture;

        private Vector2 position = new Vector2(3, 3);

        /// <summary>
        /// Loads the content associated with the Health Sprite.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("colored_packed");
        }

        /// <summary>
        /// Updates the Health, not used but included on principle.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            //Not used.
        }

        /// <summary>
        /// Draws the Health sprite.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var source = new Rectangle();
            if (Lives <= 1)
            {
                source = new Rectangle(608, 176, 16, 16);
            }
            else
            {
                source = new Rectangle((16 * Lives) + 608, 160, 16, 16);
            }

            spriteBatch.Draw(texture, position, source, Color.White, 0f, new Vector2(0, 0), 3f, SpriteEffects.None, 0f);
            
        }

    }
}
