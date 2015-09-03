LockscreenDesktopMatcher
========================
A simple background tool that adjusts the Windows 8 lock screen background to the current desktop wallpaper.
The group poilcy used exists since Windows 8. While this used to work on Pro Editions, these days the policy is documented to be honored only on Enterprise Edition systems.

The lock background is set using the Registry key 
`HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Personalization`
to which the executing user needs Access. Use the Registry Editor to adjust the permissions accordingly.
