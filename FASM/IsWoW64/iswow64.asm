format PE64 console 5.0
entry main

include 'win64w.inc'

section '.code' code readable executable

proc main
     invoke initConsole
     cmp    rax, 0
     je     .exit
     mov    [hConOut], rax
     invoke writeConsole, [hConOut], [wOutFormat]
.done:
     invoke closeConsole, [hConOut]
.exit:
     invoke ExitProcess, 0
endp

proc _initConsole
     invoke AllocConsole
     cmp    rax, 0
     jne    .ret0
     invoke GetStdHandle, STD_OUTPUT_HANDLE
     cmp    rax, INVALID_HANDLE_VALUE
     je     .ret0
     ret
.ret0:
     xor    rax, rax
     ret
endp

proc _writeConsole hCon, wStr
     local count: QWORD, written: QWORD

     cmp    [hCon], 0
     jz     .ret
     invoke lstrlen, [wStr]
     jng    .ret
     mov    [count], rax
     invoke WriteConsole, [hCon], [wStr], [count], [written], 0
.ret:
     ret
endp

proc _closeConsole hCon
     invoke CloseHandle, [hCon]
     invoke FreeConsole
     ret
endp

proc _strToInt wStr
     STIF_DEFAULT     = 0x00000000
     STIF_SUPPORT_HEX = 0x00000001
     local num: DWORD

     invoke StrToIntEx, [wStr], STIF_DEFAULT, [num]
     jz     .ret0
     xor    rax, rax
     mov    eax, [num]
     ret
.ret0:
     xor rax, rax
     ret
endp

section '.cdata' readable

wType32bit du "x86 (wow64)", 0
wType64bit du "x64", 0
wOutFormat du "Process type is %s", 0

initConsole  dq _initConsole
writeConsole dq _writeConsole
closeConsole dq _closeConsole
strToInt     dq _strToInt

section '.data' readable writable

hConOut dq 0
wOutBuf du 40h dup 0

section '.idata' import data readable

library kernel32,'KERNEL32.DLL',\
	     user32,'USER32.DLL',\
	     shlwapi, 'SHLWAPI.dll'

import kernel32,\
	     AllocConsole, 'AllocConsole',\
	     CloseHandle, 'CloseHandle',\
	     ExitProcess,'ExitProcess',\
	     FreeConsole, 'FreeConsole',\
	     GetLastError, 'GetLastError',\
	     GetStdHandle, 'GetStdHandle',\
	     IsWow64Process, 'IsWow64Process',\
	     LoadLibrary,'LoadLibraryW',\
	     lstrcmpi, 'lstrcmpiW',\
	     lstrlen, 'lstrlenW',\
	     OpenProcess, 'OpenProcess',\
	     WriteConsole, 'WriteConsoleW'

;import user32,\
;	      lstrlen, 'lstrlenW'

import shlwapi,\
	     StrToIntEx, 'StrToIntExW'