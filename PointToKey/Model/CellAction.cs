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
using System.Windows.Input;
using WindowsInput;

namespace PointToKey.Model
{
    public enum CellActionType { None, KeyDown, KeyPress, StringEntry, VJoyButtonsDown, VJoyButtonsPress };

    [Serializable]
    public class CellAction
    {
        public CellActionType ActionType { get; set; }

        public Key KeyCode { get; set; }

        public string StringEntryString { get; set; }

        public int VJoyButtons { get; set; }

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
