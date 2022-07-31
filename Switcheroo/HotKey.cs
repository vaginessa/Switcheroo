/*
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

namespace Switcheroo
{
    public class HotKey : ManagedWinapi.Hotkey
    {
        public void LoadSettings()
        {
            KeyCode = (System.Windows.Forms.Keys) Properties.Settings.Default.HotKey;            
            WindowsKey = Properties.Settings.Default.WindowsKey;
            Alt = Properties.Settings.Default.Alt;
            Ctrl = Properties.Settings.Default.Ctrl;
            Shift = Properties.Settings.Default.Shift;
            strName= Properties.Settings.Default.HotKeyName;

        }
        public void curLoadSettings()
        {
            KeyCode = (System.Windows.Forms.Keys)Properties.Settings.Default.CurHotKey;
            WindowsKey = Properties.Settings.Default.CurWindowsKey;
            Alt = Properties.Settings.Default.CurAlt;
            Ctrl = Properties.Settings.Default.CurCtrl;
            Shift = Properties.Settings.Default.CurShift;
            strName = Properties.Settings.Default.CurHotKeyName;
        }

        public void SaveSettings(HotKey _userDefined)
        {
            
            Properties.Settings.Default.HotKey = (int)_userDefined.KeyCode;
            Properties.Settings.Default.WindowsKey = _userDefined.WindowsKey;
            Properties.Settings.Default.Alt = _userDefined.Alt;
            Properties.Settings.Default.Ctrl = _userDefined.Ctrl;
            Properties.Settings.Default.Shift = _userDefined.Shift;
            Properties.Settings.Default.Save();
        }

        public void curSaveSettings(HotKey _userDefined)
        {
            Properties.Settings.Default.CurHotKey = (int)_userDefined.KeyCode;
            Properties.Settings.Default.CurWindowsKey = _userDefined.WindowsKey;
            Properties.Settings.Default.CurAlt = _userDefined.Alt;
            Properties.Settings.Default.CurCtrl = _userDefined.Ctrl;
            Properties.Settings.Default.CurShift = _userDefined.Shift;

            Properties.Settings.Default.Save();
        }
    }
}