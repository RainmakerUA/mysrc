format PE console
entry main
include 'win32axp.inc'



main:
	mov    eax, (buffer - compName) - 1
	mov    [mis], eax
	invoke GetComputerName, compName, mis

	invoke GetTickCount

	mov    [mis], eax
	xor    edx, edx
	mov    ebx, 1000
	div    ebx

	mov    [sec], eax
	mov    [mis], edx
	xor    edx, edx
	mov    ebx, 60
	div    ebx

	mov    [min], eax
	mov    [sec], edx
	xor    edx, edx
; ebx is still 60
	div    ebx

	mov    [min], edx
	xor    edx, edx
	mov    ebx, 24
	div    ebx

; eax - days, edx - hours

	invoke wsprintf, buffer, fmt, compName, eax, edx, [min], [sec], [mis]
	push   buffer
	call   con_out
	invoke ExitProcess, 0
; end main

proc	con_out text
	local h_con: DWORD, count: DWORD, written: DWORD

	invoke AllocConsole
	jnz    .exit
	invoke GetStdHandle, STD_OUTPUT_HANDLE
	cmp    eax, INVALID_HANDLE_VALUE
	je     .free
	mov    [h_con], eax	 ; Handle
	invoke lstrlen, [text]
	cmp    eax, 0
	jng    .free
	mov    [count], eax	 ; Count
	invoke WriteFile, [h_con], [text], [count], [written], 0
    .free:
	invoke CloseHandle, [h_con]
	invoke FreeConsole
    .exit:
	ret
endp

dataZ:
	mis	 dd ?
	sec	 dd ?
	min	 dd ?
	;hr	 dd ?
	;day	 dd ?
	caption  db "System uptime",0
	fmt	 db "Uptime for %s: %02d days, %02d:%02d:%02d.%03d",0
	compName db 20h dup 0
	buffer	 db 40h dup 0

data import
     library kernel32,'KERNEL32.DLL',\
	     user32,'USER32.DLL'

     import  kernel32,\
	     AllocConsole, 'AllocConsole',\
	     CloseHandle, 'CloseHandle',\
	     ExitProcess, 'ExitProcess',\
	     FreeConsole, 'FreeConsole',\
	     GetComputerName, 'GetComputerNameA',\
	     GetStdHandle, 'GetStdHandle',\
	     GetTickCount, 'GetTickCount',\
	     lstrlen, 'lstrlenA',\
	     WriteFile, 'WriteFile'

     import  user32,\
	     wsprintf, 'wsprintfA'
end data