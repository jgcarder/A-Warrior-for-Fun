/* Title: MainMenuScreen.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using A_Worrior_For_Fun.StateManagement;

namespace A_Worrior_For_Fun.Screens
{
    
    /// <summary>
    /// The first thing to pop up when starting the game
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        private ContentManager _content;

        private Game game;

        /// <summary>
        /// The constructor for the main menu screen
        /// </summary>
        public MainMenuScreen(Game game) : base("Main Menu")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var optionsMenuEntry = new MenuEntry("Options");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            this.game = game;

            if(this.game.Components.Count > 3)
            {
                this.game.Components.RemoveAt(this.game.Components.Count - 1);
            }

        }

        /// <summary>
        /// A method that allows for the passing in of the content manager
        /// </summary>
        /// <param name="content"></param>
        public void ContentPasser(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// A method that initializes the music/sounds
        /// </summary>
        public void SongStart()
        {
            Song stage1 = _content.Load<Song>("sawsquarenoise - Stage 1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            SoundEffect.MasterVolume = 0.25f;
            MediaPlayer.Play(stage1);
        }

        /// <summary>
        /// Event handler for the play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(game)); //, new CutSceneScreen());
        }

        /// <summary>
        /// Event handler for the options button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            OptionsMenuScreen op = new OptionsMenuScreen();
            op.LoadSongs(_content);
            ScreenManager.AddScreen(op, e.PlayerIndex);
        }

        /// <summary>
        /// Message box caller
        /// </summary>
        /// <param name="playerIndex">Player who selected it</param>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Confirming player exit method/event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
