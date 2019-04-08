
format PE64 GUI 5.0
entry start

include 'win64wx.inc'
include '..\multi_icongroup.inc'

HTCAPTION =     2

WIN_W     =     100
WIN_H     =     40
MAX_LEN   =     16

DUMMY     =     InitCommonControls

section '.text' code readable executable

  start:
        sub     rsp,8           ; Make stack dqword aligned

        invoke  GetModuleFileName,0,exePath,MAX_PATH
        stdcall ParseAlertTicks,exePath
        or      rax,rax
        jnz     save
        dec     rax
  save:
        mov     [alerttick],eax

        invoke  GetModuleHandle,0
        mov     [wc.hInstance],rax
        invoke  LoadIcon,0,IDI_APPLICATION
        mov     [wc.hIcon],rax
        mov     [wc.hIconSm],rax
        invoke  LoadCursor,0,IDC_ARROW
        mov     [wc.hCursor],rax
        invoke  RegisterClassEx,wc
        test    rax,rax
        jz      error

        invoke  CreateWindowEx,WS_EX_STATICEDGE+WS_EX_TOOLWINDOW+WS_EX_TOPMOST,\
                        _class,_title,WS_VISIBLE+WS_POPUP,0,0,WIN_W,WIN_H,NULL,NULL,[wc.hInstance],NULL
        test    rax,rax
        jz      error

  msg_loop:
        invoke  GetMessage,msg,NULL,0,0
        cmp     eax,1
        jb      end_loop
        jne     msg_loop
        invoke  TranslateMessage,msg
        invoke  DispatchMessage,msg
        jmp     msg_loop

  error:
        invoke  MessageBox,NULL,_error,_title,MB_ICONERROR+MB_OK

  end_loop:
        invoke  ExitProcess,[msg.wParam]
        ret

proc WindowProc uses rbx rsi rdi, hwnd,wmsg,wparam,lparam
        local   ps:PAINTSTRUCT
; Note that first four parameters are passed in registers,
; while names given in the declaration of procedure refer to the stack
; space reserved for them - you may store them there to be later accessible
; if the contents of registers gets destroyed. This may look like:
        mov     [hwnd],rcx
        mov     [wmsg],rdx
        mov     [wparam],r8
        mov     [lparam],r9

        cmp     edx,WM_SHOWWINDOW
        je      .wmshow

        cmp     edx,WM_PAINT
        je      .wmpaint

        cmp     edx,WM_RBUTTONDOWN
        je      .wmrbuttonclick

        cmp     edx,WM_LBUTTONDOWN
        je      .wmlbuttonclick

        cmp     edx,WM_DESTROY
        je      .wmdestroy

  .defwndproc:
        invoke  DefWindowProc,[hwnd],[wmsg],[wparam],[lparam]
        jmp     .finish
  .wmshow:
        or      r8,r8
        jz      .defwndproc
        invoke  CreateSolidBrush,[_cr_red]
        mov     [hredbrush],rax
        invoke  SetTimer,0,0,[_lapse],TimerProc
        mov     [timerid],rax
        invoke  GetTickCount
        stdcall TimerProc,[hwnd],WM_TIMER,[timerid],rax
        jmp     .finish
  .wmpaint:
        lea     rdx,[ps]
        invoke  BeginPaint,[hwnd],rdx
        stdcall PaintProc,[hwnd],rax
        lea     rdx,[ps]
        invoke  EndPaint,[hwnd],rdx
        jmp     .finish
  .wmlbuttonclick:
        invoke  ReleaseCapture
        invoke  PostMessage,[hwnd],WM_SYSCOMMAND,SC_MOVE+HTCAPTION,0
        jmp     .finish
  .wmrbuttonclick:
        invoke  MessageBox,[hwnd],_confirm,_title,MB_ICONQUESTION+MB_YESNO
        cmp     rax,IDYES
        jne     .finish
  .wmdestroy:
        invoke  DeleteObject,[hredbrush]
        invoke  KillTimer,0,[timerid]
        invoke  PostQuitMessage,0
        xor     eax,eax
  .finish:
        ret
endp

proc ParseAlertTicks uses rbx rsi rdi, pExePath

        mov     [pExePath],rcx
        invoke  lstrlen,rcx
        dec     rax
        push    rax
        shl     rax,1
        mov     rdi,rcx
        add     rdi,rax
        xor     rax,rax
        pop     rcx
        mov     ax,'.'
        std
  repne scasw
        mov     ax,'_'
        cmp     rdi,[pExePath]
        jb      .error
  repne scasw
        cld
        cmp     rdi,[pExePath]
        jb      .error
        add     rdi,2

        cmp     WORD [rdi],'_'
        jne     .error

        ; start parsing
        xor     rax,rax
        xor     rbx,rbx
        xor     rdx,rdx
  .start:
        mov     ax,WORD [rdi]

        cmp     ax,'_'
        je      .next

        ;period ('.') - end
        cmp     ax,'.'
        jne     .num
        add     rdx,rbx
        imul    rax,rdx,1000
        jmp     .return
  ;number
  .num:
        cmp     ax,'0'
        jb      .mod
        cmp     ax,'9'
        ja      .mod
        sub     ax,$30
        imul    ebx,10
        add     ebx,eax
        jmp     .next
  ;modifier
  .mod:
        cmp     ax,'d'
        je      .d
        cmp     ax,'h'
        je      .h
        cmp     ax,'m'
        je      .m
        jmp     .error
    .d:
        imul    ebx,24*60*60
        jmp     .nextBlock
    .h:
        imul    ebx,60*60
        jmp     .nextBlock
    .m:
        imul    ebx,60
        jmp     .nextBlock
  ;next
  .nextBlock:
        add     rdx,rbx
        xor     rbx,rbx
  .next:
        add     rdi,2
        jmp     .start
  .error:
        cld
        xor     rax,rax
  .return:
        ret
endp

proc PaintProc uses rbx rsi rdi, hwnd,hdc
        ;local   rect:RECT

        mov     [hwnd],rcx
        mov     [hdc],rdx

        invoke  SetBkMode,[hdc],TRANSPARENT
        xor     rax,rax
        mov     al,[isAlert]
        or      al,al
        jz      .textout
        invoke  SelectObject,[hredbrush]
        push    rax
        invoke  FillRect,[hdc],_cli_rect,[hredbrush]
        pop     rcx
        invoke  SelectObject,rcx
  .textout:
        stdcall CenteredTextOut,[hwnd],[hdc],result

        ret
endp

proc TimerProc uses rbx rsi rdi, hwnd,msg,id,ticks
        ;mov     [hwnd],rcx
        ;mov     [msg],rdx
        ;mov     [id],r8
        ;mov     [ticks],r9

        cmp     edx,WM_TIMER
        jne     .ret
        cmp     r8,[timerid]
        jne     .ret

        push    rcx
        stdcall FillUptimeString,r9
        pop     rcx
        push    rcx
        invoke  InvalidateRect,rcx,0,1
        pop     rcx
        invoke  UpdateWindow,rcx
  .ret:
        ret
endp

proc FillUptimeString uses rbx rsi rdi, ticks
        local  hr:DWORD,min:DWORD,sec:DWORD,mis:DWORD

        ;mov    [ticks],rcx

        mov    eax,ecx
        cmp    eax,[alerttick]
        jbe    .format_ticks
        mov    [isAlert],1
  .format_ticks:
        mov    [mis],eax
        xor    edx,edx
        mov    ebx,1000
        div    ebx

        mov    [sec],eax
        mov    [mis],edx
        xor    edx,edx
        mov    ebx,60
        div    ebx

        mov    [min],eax
        mov    [sec],edx
        xor    edx,edx
; ebx is still 60
        div    ebx

        mov    [min],edx
        xor    edx,edx
        mov    ebx,24
        div    ebx

        mov    [hr],edx

        or     eax,eax
        jz     .format_hr
        invoke wsprintf,result,_fmt_days,eax,[hr],[min],[sec]
        jmp    .return
  .format_hr:
        invoke wsprintf,result,_fmt_hrs,[hr],[min],[sec]
  .return:
        mov    eax,[sec]
        and    eax,1
        and    [isAlert],al
        ret
endp

proc CenteredTextOut uses rbx rsi rdi, hwnd,hdc,pstr
        local   rect:RECT,size:SIZE,strlen:QWORD
        mov     [hwnd],rcx
        mov     [hdc],rdx
        mov     [pstr],r8

        lea     rdx,[rect]
        invoke  GetClientRect,[hwnd],rdx
        invoke  lstrlen,[pstr]
        mov     [strlen],rax
        lea     r9,[size]
        invoke  GetTextExtentPoint32,[hdc],[pstr],[strlen],r9
        xor     rax,rax
        mov     eax,[rect.right]
        sub     eax,[size.cx]
        shr     eax,1
        xor     rbx,rbx
        mov     ebx,[rect.bottom]
        sub     ebx,[size.cy]
        shr     ebx,1
        invoke  TextOut,[hdc],rax,rbx,[pstr],[strlen]
        ret
endp

section '.data' data readable writeable

  _cli_rect RECT  0,0,WIN_W,WIN_H
  _title    TCHAR '[RM] Uptime ticker',0
  _class    TCHAR 'UptimeWin64',0
  _error    TCHAR 'Startup failed.',0
  _confirm  TCHAR 'Do you really want to close me?',0
  _fmt_hrs  TCHAR '%02d:%02d:%02d',0
  _fmt_days TCHAR '%dd %02d:%02d:%02d',0
  _lapse    dd    1000 ; 1 sec
  _cr_red   dd    $001010FF
  _guard    TCHAR '_.',0

  exePath   TCHAR MAX_PATH+1 dup 0
  wc        WNDCLASSEX sizeof.WNDCLASSEX,0,WindowProc,0,0,NULL,NULL,NULL,COLOR_BTNFACE+1,NULL,_class,NULL
  msg       MSG
  alerttick dd 0
  timerid   dq ?
  hredbrush dq ?
  result    TCHAR MAX_LEN dup 0
  isAlert   db 0

section '.idata' import data readable

  library kernel32,'KERNEL32.DLL',\
          user32,'USER32.DLL',\
          gdi32,'GDI32.DLL',\
          comctl32,'COMCTL32.DLL'

  include 'api\kernel32.inc'
  include 'api\user32.inc'
  include 'api\gdi32.inc'
  import comctl32,\
             InitCommonControls,'InitCommonControls'

section '.rsrc' resource data readable

        directory RT_ICON,icons,\
                  RT_GROUP_ICON,grp_icons,\
                  24,manifest

         resource icons,\
                  1, LANG_NEUTRAL, icon_data1,\
                  2, LANG_NEUTRAL, icon_data2,\
                  3, LANG_NEUTRAL, icon_data3,\
                  4, LANG_NEUTRAL, icon_data4,\
                  5, LANG_NEUTRAL, icon_data5,\
                  6, LANG_NEUTRAL, icon_data6

         resource grp_icons,\
                  10, LANG_NEUTRAL, main_icon

         multi_icon main_icon,\
                    icon_data1, 'main.ico', 0,\
                    icon_data2, 'main.ico', 3,\
                    icon_data3, 'main.ico', 2,\
                    icon_data4, 'main.ico', 5,\
                    icon_data5, 'main.ico', 4,\
                    icon_data6, 'main.ico', 1

        resource manifest,\
                  1, LANG_NEUTRAL, winxp
        resdata winxp
                  file 'exe.manifest'
endres
