##               vJoy installation##
Please READ instructions in [vJoy site](http://vjoystick.sourceforge.net/site/index.php)

[Do not hesitate to contact me](mailto://shaul_ei@users.sourceforge.net)

---------------------------------------------------------
# Release notes for 20-Mar-2015 #

Improved Revision 2.0.5

- Driver itself unchanged.

- Installation bugs fixed (tested on Vista x64).

- Installs on Windows 10 (Tested on Build 9926 x64).

- vJoyConfig: Much improved.


---------------------------------------------------------
# Release notes for 08-Jan-2015 #

New Revision - 2.0.5 - Evolution of 2.0.4

Minor interface changes and bug fixes
[Full description and instructions for the developer](http://sourceforge.net/projects/vjoystick/files/Beta%202.x/2.0.5-080115/vJoy205RN.pdf/download)

---------------------------------------------------------
# Release notes for 24-12-2014 #

vJoy driver version 2.0.4 unchanged.

## Installer Improved: ##
File Organization:

On x64 machines:

- All 64 bit applications will be moved to %vjoy%\x64 including all DLL files.

- All 32 bit applications will be moved to %vjoy%\x86 including all DLL files.

On x86 machines:

All 32 bit applications will be moved to %vjoy%\x86 including all DLL files.

In the Registry:

Under HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{8E31F76F-74C3-47F1-9550-E041EEDC5FBB}_is1:

- Key: DllX86Location

- Key: DllX64Location

---------------------------------------------------------
# Release notes for 22-09-2014 #

vJoy driver version 2.0.4 unchanged.

## Utilities Improved: ##

vJoyConf:
Bug fixed - Now, there is a default POV type (4-Directions) and default number of POVs (0)

vJoyFeeder:

- Now lists all implemented vJoy device.

- Prevents selection of device owned by another application.

JoyMonitor:
Added full support to POVs

- Type of POV ignored

- Up to 4 POVs supported

---------------------------------------------------------

Branch 2.X of vJoy holds vJoy releases that:

- Are NOT compatible with PPJoy

- Enable the user to install 1 to 16 vJoy devices

- The number of devices is configurable

- Each device is individually configurable

- SDK provided: C/C++ and C# Development environment

---------------------------------------------------------
