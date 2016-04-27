format PE GUI 4.0

include 'win32w.inc'

	dummy = InitCommonControls

section '.code' code readable writeable executable

start:
	mov    eax, (buffer - compName) / 2 - 1
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
	invoke LoadLibrary, shell32
	mov    [mbParam.hInstance], eax
	invoke MessageBoxIndirect, mbParam
	invoke ExitProcess, 0

data0:
	mis	 dd ?
	sec	 dd ?
	min	 dd ?
	caption  du "System uptime",0
	fmt	 du "Uptime of %s:",0Ah,"%02d days, %02d:%02d:%02d.%03d",0
	shell32  du "SHELL32.DLL",0
	mbParam  MSGBOXPARAMS sizeof.MSGBOXPARAMS, 0, 0, buffer, caption, MB_OK + MB_USERICON, 47, 0, 0, 0
	compName du 20h dup 0
	buffer	 du 40h dup 0

data import
     library kernel32,'KERNEL32.DLL',\
	     user32,'USER32.DLL',\
	     comctl32,'COMCTL32.DLL'

     import  kernel32,\
	     GetComputerName,'GetComputerNameW',\
	     GetTickCount,'GetTickCount',\
	     ExitProcess,'ExitProcess',\
	     LoadLibrary,'LoadLibraryW'

     import  user32,\
	     wsprintf,'wsprintfW',\
	     MessageBoxIndirect,'MessageBoxIndirectW'

     import comctl32,\
	     InitCommonControls,'InitCommonControls'
end data

section '.rsrc' resource data readable

	directory 24,manifest
	resource  manifest,\
		  1, LANG_NEUTRAL, winxp
	resdata winxp
		  file 'exe.manifest'
endres