/* Title: OptionsMenu.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace A_Worrior_For_Fun.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various, hopefully useful, ways.
    public class OptionsMenuScreen : MenuScreen
    {
        private enum SongPlaying
        {
            None,
            //Default,
            Stage_1,
            Eight_Bit_Raceway,
            Boss_Theme,
            Aaaaa
        }

        private readonly MenuEntry _songMenuEntry;
        //private readonly MenuEntry _languageMenuEntry;
        private readonly MenuEntry _soundEffectMenuEntry;
        private readonly MenuEntry _volumeMenuEntry;

        private ContentManager _content;

        private static SongPlaying _currentSong = SongPlaying.Stage_1;
        private static int _currentLanguage;
        private static int _volume = 25;
        private static int _sfVolume = 25;

        private Song stage1;
        private Song eightBit;
        private Song bossTheme;
        private Song aaaaa;

        /// <summary>
        /// The constructor
        /// </summary>
        public OptionsMenuScreen() : base("Options")
        {
            _songMenuEntry = new MenuEntry(string.Empty);
            //_languageMenuEntry = new MenuEntry(string.Empty);
            _soundEffectMenuEntry = new MenuEntry(string.Empty);
            _volumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _songMenuEntry.Selected += SongMenuEntrySelected;
            //_languageMenuEntry.Selected += LanguageMenuEntrySelected;
            _soundEffectMenuEntry.Selected += SoundEffectVolumeMenuEntrySelected;
            _volumeMenuEntry.Selected += VolumeMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(_songMenuEntry);
            //MenuEntries.Add(_languageMenuEntry);
            MenuEntries.Add(_soundEffectMenuEntry);
            MenuEntries.Add(_volumeMenuEntry);
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Loads the songs
        /// </summary>
        /// <param name="content">The content Manager</param>
        public void LoadSongs(ContentManager content)
        {
            if (_content == null)
            {
                _content = content;
            }

            stage1 = _content.Load<Song>("sawsquarenoise - Stage 1");
            eightBit = _content.Load<Song>("Wizwars - 8 Bit Raceway");
            bossTheme = _content.Load<Song>("sawsquarenoise - Boss Theme");
            aaaaa = _content.Load<Song>("Lithis - Aaaaa");
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _songMenuEntry.Text = $"Song: {_currentSong}";
            _soundEffectMenuEntry.Text = $"Effects Volume: {_sfVolume.ToString()}";
            _volumeMenuEntry.Text = $"Song Volume: {_volume.ToString()}";
        }

        /// <summary>
        /// Event handler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            _currentSong++;
            if (_currentSong > SongPlaying.Aaaaa)
                _currentSong = 0;

            MediaPlayer.IsRepeating = true;

            switch(_currentSong)
            {
                case SongPlaying.None:
                    MediaPlayer.Stop();
                    break;
                //case SongPlaying.Default:
                //    MediaPlayer.Stop();
                //    //
                //    break;
                case SongPlaying.Stage_1:
                    MediaPlayer.Stop();
                    MediaPlayer.Play(stage1);
                    break;
                case SongPlaying.Eight_Bit_Raceway:
                    MediaPlayer.Stop();
                    MediaPlayer.Play(eightBit);
                    break;
                case SongPlaying.Boss_Theme:
                    MediaPlayer.Stop();
                    MediaPlayer.Play(bossTheme);
                    break;
                case SongPlaying.Aaaaa:
                    MediaPlayer.Stop();
                    MediaPlayer.Play(aaaaa);
                    break;
            }

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundEffectVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            _sfVolume += 5;
            if (_sfVolume > 100) _sfVolume = 0;
            SoundEffect.MasterVolume = 0.01f * _sfVolume;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            _volume+=5;
            if (_volume > 100) _volume = 0;

            MediaPlayer.Volume = 0.01f * _volume;

            SetMenuEntryText();
        }
    }
}
