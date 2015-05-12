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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace PointToKey.Util
{
    internal class VJoyInputSender : IJoystickInputSender, IDisposable
    {
        private vJoy joystick = null;
        private bool acquired = false;
        private bool disposed = false;

        public bool Initialise()
        {
            joystick = new vJoy();

            if (CheckJoyEnabled() == false)
                return false;

            if (CheckJoyDriverVersion() == false)
                return false;

            // TODO: Multiple joysticks
            uint joyId = 0;

            bool? joyStatus = CheckJoyStatus(joyId);

            switch (joyStatus)
            {
                case false:
                    return false;

                case null:
                    if (AcquireJoy(joyId) == false)
                        return false;
                    break;

                case true:
                    // Nothing to do
                    break;
            }
            
            return true;
        }

        private bool CheckJoyEnabled()
        {
            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                Debug.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return false;
            }

            Debug.WriteLine("Vendor: {0}\nProduct: {1}\nVersion Number: {2}",
                joystick.GetvJoyManufacturerString(),
                joystick.GetvJoyProductString(),
                joystick.GetvJoySerialNumberString());

            return true;
        }

        private bool CheckJoyDriverVersion()
        {
            uint dllVersion = 0;
            uint driverVersion = 0;
            bool matches = joystick.DriverMatch(ref dllVersion, ref driverVersion);

            Debug.WriteLine("The joystick driver reports that its version {0} our assembly wrapper version",
                matches ? "matches" : "doesn't match", "");

            return matches;
        }

        private bool? CheckJoyStatus(uint id)
        {
            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Console.WriteLine("vJoy Device {0} is already owned by this feeder", id);
                    return true;
                case VjdStat.VJD_STAT_FREE:
                    Console.WriteLine("vJoy Device {0} is free", id);
                    return null;
                case VjdStat.VJD_STAT_BUSY:
                    Console.WriteLine(
                    "vJoy Device {0} is already owned by another feeder - Cannot continue", id);
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    Console.WriteLine(
                    "vJoy Device {0} is not installed or disabled - Cannot continue", id);
                    return false;
                default:
                    Console.WriteLine("vJoy Device {0} general error - Cannot continue", id);
                    return false;
            };
        }

        private bool AcquireJoy(uint id)
        {
            // Write access to vJoy Device - Basic
            VjdStat status;
            status = joystick.GetVJDStatus(id);

            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) ||
                ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                Debug.WriteLine("Failed to acquire vJoy device number {0}.", id);
                return false;
            }
            else
            {
                Debug.WriteLine("Acquired: vJoy device number {0}.", id);
                return true;
            }
        }

        private void RelinquishJoy(uint id)
        {
            joystick.RelinquishVJD(id);
        }

        public void SendButtonsDown(int buttons)
        {
            throw new NotImplementedException();
        }

        public void SendButtonsPress(int buttons)
        {
            throw new NotImplementedException();
        }

        public void SendButtonsUp(int buttons)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (acquired)
                {
                    RelinquishJoy(0);
                }
            }
        }
    }
}
