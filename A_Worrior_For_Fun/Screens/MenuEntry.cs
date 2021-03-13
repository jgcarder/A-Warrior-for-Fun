/* Title: MenuEntry.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using A_Worrior_For_Fun.StateManagement;

namespace A_Worrior_For_Fun.Screens
{

    /// <summary>
    // Helper class represents a single entry in a MenuScreen. By default this
    // just draws the entry text string, but it can be customized to display menu
    // entries in different ways. This also provides an event that will be raised
    // when the menu entry is selected.
    /// </summary>
    public class MenuEntry
    {
        private string _text;
        private float _selectionFade;    // Entries transition out of the selection effect when they are deselected
        private Vector2 _position;    // This is set by the MenuScreen each frame in Update

        /// <summary>
        /// Displayed text
        /// </summary>
        public string Text
        {
            private get => _text;
            set => _text = value;
        }

        /// <summary>
        /// Position of text
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        /// <summary>
        /// Handler for if something is selected
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
        }

        /// <summary>
        /// Constructor for the MenuEntry
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        public MenuEntry(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Updated the menu entry
        /// </summary>
        /// <param name="screen"><Screen attached to/param>
        /// <param name="isSelected">A bool for if this option is selected</param>
        /// <param name="gameTime">The game's time</param>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }

        /// <summary>
        /// Draws the menu entry
        /// </summary>
        /// <param name="screen">Screen to draw to</param>
        /// <param name="isSelected">is something selected</param>
        /// <param name="gameTime">The game's time</param>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * _selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, _text, _position, color, 0,
                origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// outputs the height
        /// </summary>
        /// <param name="screen">The screen to check</param>
        /// <returns>The height</returns>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        /// <summary>
        /// outputs the width
        /// </summary>
        /// <param name="screen">The screen to check</param>
        /// <returns>The width</returns>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
