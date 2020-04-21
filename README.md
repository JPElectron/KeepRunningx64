# KeepRunningx64
Allows you to launch a program and ensure it stays running. This could be to restart a crashed application or prevent something from being closed accidentally. Useful in a kiosk, library, school, website demo, or web-based application such as a self-registration/signup type of environment.

Use Keep Running x64 as a custom shell so an application such as Internet Explorer is the only available program and automatically re-launch it when closed. Or repurpose old workstations as thin-clients, having them automatically connect your terminal server.

Tested on Windows 7, 8, 8.1 and Server 2008, 2012
A scripted install/uninstall is not included with this software.
This program runs in the background; without any GUI, taskbar, or system tray icon.
Since this program is 64-bit it can detect other 64-bit or 32-bit applications.

For a 32-bit only version see Keep Running

<b>Installation:</b>

1) Ensure the Microsoft .NET Framework 4.x is installed
2) Extract the contents of the .zip file
3) Modify keeprun64.ini as indicated below
4) Run keeprun64.exe

View the default .ini file that's included.

<b>.ini Settings:</b>

keeprun64.exe does not have to directly launch the executable you are detecting, you may specify a batch file or another program which runs the executable being detected.

Use quotes around the full path if it contains spaces.

Minimally required .ini settings to launch IE on 64-bit Windows

    Detect=iexplore
    Launch="C:\Program Files\Internet Explorer\iexplore.exe"
      to launch IE in kiosk mode add...
    Arguments=-k
      or to launch IE in kiosk mode with a URL add...
    Arguments=-k http://www.jpelectron.com/krun/setup.htm

Minimally required .ini settings to launch Terminal Server client on 64-bit Windows

    Detect=mstsc
    Launch=C:\Windows\System32\mstsc.exe
      to automatically connect in full-screen mode add...
    Arguments=/v:[your_terminal_server_ip] /f

<b>Usage:</b>

Optionally, use Autologon: http://technet.microsoft.com/en-us/sysinternals/bb963905.aspx to set the workstation to logon automatically.

Set keeprun64.exe to start automatically by adding a shortcut in the Startup folder, the registry, or via a Scheduled Task.

Or to have just one program run immediately after login (without explorer, any desktop icons, or a taskbar) set a custom shell...

     For the currently logged on user go to...
       HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System
     For everyone who uses this machine go to...
       HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System

   (If the "System" key does not exist: Edit > New > Key > System)

   Then create a new string value...
   
       Edit > New > String Value
       Value Name: Shell
       Value Data: keeprun64.exe (assuming it is located in the Windows working directory)

   When setting a custom shell keeprun64.ini should be in the root of the user's profile folder
   usually C:\Users\[username]
   Some systems will require that keeprun64.ini be located in C:\windows\system32
   In all other cases keeprun64.ini should be located in the same folder as keeprun64.exe

To log the restarts of a failed application define Launch= as the full path to log-launch.bat: http://jpelectron.com/sample/JPElectron/KeepRun,%20log%20launch.bat and edit this batch file to contain the path to your application.

To restart a service define Detect= as the name of the service executable but define Launch= as the full path to service-restart.bat and edit this batch file to contain the Windows service name.

To detect more than one executable on the same system copy keeprun64.exe and keeprun64.ini into another folder, then use this second instance to detect and launch something else. When running multiple keeprun64.exe's sometimes it's best to rename each keeprun64.exe differently, like keeprun64-for-ie.exe and keeprun64-for-other.exe so you can identify each under task manager, the config file should always be named keeprun64.ini

If you set Reboot=Yes then also add Launch=placeholder to suppress the error prompt.
