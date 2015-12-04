/*  PointToKey
    Copyright (C) 2015 Nigel Jones

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsInput;

namespace PointToKey.Model
{
    public enum CellActionType { None, KeyDown, KeyPress, StringEntry, VJoyButtonsDown, VJoyButtonsPress };

    [Serializable]
    public class CellAction : DependencyObject
    {
        public CellActionType ActionType { get; set; }

        public Key KeyCode
        {
            get { return (Key)GetValue(KeyCodeProperty); }
            set { SetValue(KeyCodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyCode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyCodeProperty =
            DependencyProperty.Register("KeyCode", typeof(Key), typeof(CellAction), new PropertyMetadata(null));




        public string StringEntryString
        {
            get { return (string)GetValue(StringEntryStringProperty); }
            set { SetValue(StringEntryStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StringEntryString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringEntryStringProperty =
            DependencyProperty.Register("StringEntryString", typeof(string), typeof(CellAction), new PropertyMetadata(string.Empty));




        public int VJoyButtons
        {
            get { return (int)GetValue(VJoyButtonsProperty); }
            set { SetValue(VJoyButtonsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VJoyButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VJoyButtonsProperty =
            DependencyProperty.Register("VJoyButtons", typeof(int), typeof(CellAction), new PropertyMetadata(0));




        public override string ToString()
        {
            string output = ActionType.ToString();

            switch (ActionType)
            {
                case CellActionType.None:
                    output = string.Empty;
                    break;
                case CellActionType.KeyDown:
                case CellActionType.KeyPress:
                    output += " " + KeyCode;
                    break;
                case CellActionType.StringEntry:
                    output += " " + StringEntryString;
                    break;
                case CellActionType.VJoyButtonsDown:
                case CellActionType.VJoyButtonsPress:
                    output += " " + VJoyButtons;
                    break;
                default:
                    throw new Exception("Unknown CellActionType");
            }

            return output;
        }
    }
}
