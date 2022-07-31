﻿/*
 * Switcheroo - The incremental-search task switcher for Windows.
 * http://www.switcheroo.io/
 * Copyright 2009, 2010 James Sulak
 * Copyright 2014 Regin Larsen
 * 
 * Switcheroo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Switcheroo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Switcheroo.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ManagedWinapi;
using Switcheroo.Core;
using Switcheroo.Properties;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Switcheroo
{
    public partial class OptionsWindow : Window
    {
        private readonly HotKey _hotkey, _curHotkey;
        private HotkeyViewModel _hotkeyViewModel, _curHotkeyViewModel;

        public OptionsWindow()
        {
            InitializeComponent();

            // Show what's already selected     
            _hotkey = (HotKey) Application.Current.Properties["hotkey"];
            _curHotkey = (HotKey)Application.Current.Properties["curHotkey"];

            try
            {
                _hotkey.LoadSettings();             
            }
            catch (HotkeyAlreadyInUseException)
            {
            }
            try
            {                
                _curHotkey.curLoadSettings();
            }
            catch (HotkeyAlreadyInUseException)
            {
            }

            _hotkeyViewModel = new HotkeyViewModel
            {
                KeyCode = KeyInterop.KeyFromVirtualKey((int) _hotkey.KeyCode),
                Alt = _hotkey.Alt,
                Ctrl = _hotkey.Ctrl,
                Windows = _hotkey.WindowsKey,
                Shift = _hotkey.Shift
            };
            _curHotkeyViewModel = new HotkeyViewModel
            {
                KeyCode = KeyInterop.KeyFromVirtualKey((int)_curHotkey.KeyCode),
                Alt = _curHotkey.Alt,
                Ctrl = _curHotkey.Ctrl,
                Windows = _curHotkey.WindowsKey,
                Shift = _curHotkey.Shift
            };

            CurHotKeyCheckBox.IsChecked = Settings.Default.CurEnableHotKey;
            CurHotkeyPreview.Text = _curHotkeyViewModel.ToString();
            CurHotkeyPreview.IsEnabled = Settings.Default.CurEnableHotKey;

            HotKeyCheckBox.IsChecked = Settings.Default.EnableHotKey;
            HotkeyPreview.Text = _hotkeyViewModel.ToString();
            HotkeyPreview.IsEnabled = Settings.Default.EnableHotKey;


            AltTabCheckBox.IsChecked = Settings.Default.AltTabHook;
            AutoSwitch.IsChecked = Settings.Default.AutoSwitch;
            AutoSwitch.IsEnabled = Settings.Default.AltTabHook;
            RunAsAdministrator.IsChecked = Settings.Default.RunAsAdmin;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var closeOptionsWindow = true;

            try
            {
                _hotkey.Enabled = false;

                if (Settings.Default.EnableHotKey)
                {
                    // Change the active hotkey
                    _hotkey.Alt = _hotkeyViewModel.Alt;
                    _hotkey.Shift = _hotkeyViewModel.Shift;
                    _hotkey.Ctrl = _hotkeyViewModel.Ctrl;
                    _hotkey.WindowsKey = _hotkeyViewModel.Windows;
                    _hotkey.KeyCode = (Keys) KeyInterop.VirtualKeyFromKey(_hotkeyViewModel.KeyCode);
                    _hotkey.Enabled = true;

                    Properties.Settings.Default.HotKey = (int)(Keys)KeyInterop.VirtualKeyFromKey(_hotkeyViewModel.KeyCode);
                    Properties.Settings.Default.WindowsKey = _hotkeyViewModel.Windows;
                    Properties.Settings.Default.Alt = _hotkeyViewModel.Alt;
                    Properties.Settings.Default.Ctrl = _hotkeyViewModel.Ctrl;
                    Properties.Settings.Default.Shift = _hotkeyViewModel.Shift;
                    
                }
                if (Settings.Default.CurEnableHotKey)
                {
                    // Change the active hotkey
                    _curHotkey.Alt = _curHotkeyViewModel.Alt;
                    _curHotkey.Shift = _curHotkeyViewModel.Shift;
                    _curHotkey.Ctrl = _curHotkeyViewModel.Ctrl;
                    _curHotkey.WindowsKey = _curHotkeyViewModel.Windows;
                    _curHotkey.KeyCode = (Keys)KeyInterop.VirtualKeyFromKey(_curHotkeyViewModel.KeyCode);
                    _curHotkey.Enabled = true;

                    Properties.Settings.Default.CurHotKey = (int)(Keys)KeyInterop.VirtualKeyFromKey(_curHotkeyViewModel.KeyCode);
                    Properties.Settings.Default.CurWindowsKey = _curHotkeyViewModel.Windows;
                    Properties.Settings.Default.CurAlt = _curHotkeyViewModel.Alt;
                    Properties.Settings.Default.CurCtrl = _curHotkeyViewModel.Ctrl;
                    Properties.Settings.Default.CurShift = _curHotkeyViewModel.Shift;
                }
                Properties.Settings.Default.Save();
                //_hotkey.SaveSettings(_hotkey);
                //_curHotkey.curSaveSettings(_curHotkey);
            }
            catch (HotkeyAlreadyInUseException)
            {
                var boxText = "Sorry! The selected shortcut for activating Switcheroo is in use by another program. " +
                              "Please choose another.";
                MessageBox.Show(boxText, "Shortcut already in use", MessageBoxButton.OK, MessageBoxImage.Warning);
                closeOptionsWindow = false;
            }

            Settings.Default.EnableHotKey = HotKeyCheckBox.IsChecked.GetValueOrDefault();
            Settings.Default.CurEnableHotKey = CurHotKeyCheckBox.IsChecked.GetValueOrDefault();
            Settings.Default.AltTabHook = AltTabCheckBox.IsChecked.GetValueOrDefault();
            Settings.Default.AutoSwitch = AutoSwitch.IsChecked.GetValueOrDefault();
            Settings.Default.RunAsAdmin = RunAsAdministrator.IsChecked.GetValueOrDefault();
            Settings.Default.Save();

            if (closeOptionsWindow)
            {
                Close();
            }
        }

        private void HotkeyPreview_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // The text box grabs all input
            e.Handled = true;

            // Fetch the actual shortcut key
            var key = (e.Key == Key.System ? e.SystemKey : e.Key);

            // Ignore modifier keys
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }

            var previewHotkeyModel = new HotkeyViewModel();
            previewHotkeyModel.Ctrl = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            previewHotkeyModel.Shift = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            previewHotkeyModel.Alt = (Keyboard.Modifiers & ModifierKeys.Alt) != 0;

            var winLKey = new KeyboardKey(Keys.LWin);
            var winRKey = new KeyboardKey(Keys.RWin);
            previewHotkeyModel.Windows = (winLKey.State & 0x8000) == 0x8000 || (winRKey.State & 0x8000) == 0x8000;
            previewHotkeyModel.KeyCode = key;

            var previewText = previewHotkeyModel.ToString();

            // Jump to the next element if the user presses only the Tab key
            if (previewText == "Tab")
            {
                ((UIElement) sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                return;
            }

            HotkeyPreview.Text = previewText;
            _hotkeyViewModel = previewHotkeyModel;
        }
        private void CurHotkeyPreview_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // The text box grabs all input
            e.Handled = true;

            // Fetch the actual shortcut key
            var key = (e.Key == Key.System ? e.SystemKey : e.Key);

            // Ignore modifier keys
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }

            var previewHotkeyModel = new HotkeyViewModel();
            previewHotkeyModel.Ctrl = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            previewHotkeyModel.Shift = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            previewHotkeyModel.Alt = (Keyboard.Modifiers & ModifierKeys.Alt) != 0;

            var winLKey = new KeyboardKey(Keys.LWin);
            var winRKey = new KeyboardKey(Keys.RWin);
            previewHotkeyModel.Windows = (winLKey.State & 0x8000) == 0x8000 || (winRKey.State & 0x8000) == 0x8000;
            previewHotkeyModel.KeyCode = key;

            var previewText = previewHotkeyModel.ToString();

            // Jump to the next element if the user presses only the Tab key
            if (previewText == "Tab")
            {
                ((UIElement)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                return;
            }

            CurHotkeyPreview.Text = previewText;
            _curHotkeyViewModel = previewHotkeyModel;
        }
        private class HotkeyViewModel
        {
            public Key KeyCode { get; set; }
            public bool Shift { get; set; }
            public bool Alt { get; set; }
            public bool Ctrl { get; set; }
            public bool Windows { get; set; }

            public override string ToString()
            {
                var shortcutText = new StringBuilder();

                if (Ctrl)
                {
                    shortcutText.Append("Ctrl + ");
                }

                if (Shift)
                {
                    shortcutText.Append("Shift + ");
                }

                if (Alt)
                {
                    shortcutText.Append("Alt + ");
                }

                if (Windows)
                {
                    shortcutText.Append("Win + ");
                }

                var keyString =
                    KeyboardHelper.CodeToString((uint) KeyInterop.VirtualKeyFromKey(KeyCode)).ToUpper().Trim();
                if (keyString.Length == 0)
                {
                    keyString = new KeysConverter().ConvertToString(KeyCode);
                }

                // If the user presses "Escape" then show "Escape" :)
                if (keyString == "\u001B")
                {
                    keyString = "Escape";
                }

                shortcutText.Append(keyString);
                return shortcutText.ToString();
            }
        }

        private void HotkeyPreview_OnGotFocus(object sender, RoutedEventArgs e)
        {
            // Disable the current hotkey while the hotkey field is active
            _hotkey.Enabled = false;
        }
        private void CurHotkeyPreview_OnGotFocus(object sender, RoutedEventArgs e)
        {
            // Disable the current hotkey while the hotkey field is active
            _hotkey.Enabled = false;
        }

        private void HotkeyPreview_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _hotkey.Enabled = true;
            }
            catch (HotkeyAlreadyInUseException)
            {
                // It is alright if the hotkey can't be reactivated
            }
        }
        private void CurHotkeyPreview_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _hotkey.Enabled = true;
            }
            catch (HotkeyAlreadyInUseException)
            {
                // It is alright if the hotkey can't be reactivated
            }
        }

        private void AltTabCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            AutoSwitch.IsEnabled = true;
        }

        private void AltTabCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AutoSwitch.IsEnabled = false;
            AutoSwitch.IsChecked = false;
        }

        private void HotKeyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HotkeyPreview.IsEnabled = true;
        }

        private void HotKeyCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            HotkeyPreview.IsEnabled = false;
        }
        private void CurHotKeyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CurHotkeyPreview.IsEnabled = true;
        }

        private void CurHotKeyCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            CurHotkeyPreview.IsEnabled = false;
        }
    }
}