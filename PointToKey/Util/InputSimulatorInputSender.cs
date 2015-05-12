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

using PointToKey.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;

namespace PointToKey.Util
{
    internal class InputSimulatorInputSender : IKeyboardInputSender
    {
        private InputSimulator inputSimulator = new InputSimulator();

        public void SendKeyDown(Key keyCode)
        {
            inputSimulator.Keyboard.KeyDown(KeyCodeToVirtualKeyCode(keyCode));
        }

        public void SendKeyUp(Key keyCode)
        {
            inputSimulator.Keyboard.KeyUp(KeyCodeToVirtualKeyCode(keyCode));
        }

        public void SendKeyPress(Key keyCode)
        {
            inputSimulator.Keyboard.KeyPress(KeyCodeToVirtualKeyCode(keyCode));
        }

        public void SendTextEntry(string text)
        {
            inputSimulator.Keyboard.TextEntry(text);
        }

        private VirtualKeyCode KeyCodeToVirtualKeyCode(Key keyCode)
        {
            var newKey = KeyInterop.VirtualKeyFromKey(keyCode);
            return (VirtualKeyCode)newKey;
        }
    }
}
