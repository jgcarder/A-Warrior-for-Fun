/* Title: PlayerIndexEventArgs.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace A_Worrior_For_Fun.Screens
{
    /// <summary>
    /// The player event args
    /// </summary>
    public class PlayerIndexEventArgs : EventArgs
    {
        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="playerIndex"></param>
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
