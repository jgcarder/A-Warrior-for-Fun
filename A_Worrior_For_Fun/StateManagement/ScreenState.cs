/* File: ScreenState.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;


namespace A_Worrior_For_Fun.StateManagement
{
    /// <summary>
    /// Enumerations of the possible screen states
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active, 
        TransitionOff,
        Hidden
    }
}
