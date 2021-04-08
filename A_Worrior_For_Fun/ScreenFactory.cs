/* File: ScreenFactory.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using A_Worrior_For_Fun.StateManagement;

namespace A_Worrior_For_Fun
{
    // Our game's implementation of IScreenFactory which can handle creating the screens
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            // All of our screens have empty constructors so we can just use Activator
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
