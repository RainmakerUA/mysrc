Import-Module .\RM.BinPatcher\RM.BinPatcher.Module.dll

Copy-Item .\notepad.exe .\notepad.exe.bak
Optimize-File .\patch.txt .\notepad.exe
