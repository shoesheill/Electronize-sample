!include "MUI2.nsh"

Outfile "uninstall.exe"

Section "Uninstall"
  Delete "$INSTDIR\*.*" ; Deletes all files in the installation directory
  RMDir /r "$INSTDIR"   ; Removes the installation directory
  DeleteRegKey HKCU "Software\WebApplication5" ; Removes app registry entry
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WebApplication5" ; Removes uninstaller registry entry
SectionEnd
