
[System.Environment]::CurrentDirectory = $pwd
[System.Environment]::CurrentDirectory = "..\target\debug\examples"

$extern = @'
    [System.Runtime.InteropServices.DllImportAttribute("dylib.dll")] public static extern int TestRust(int x, int y);
'@

$rustfn = Add-Type -MemberDefinition $extern -Name RustFn -Namespace RustLib -PassThru

$x = 13
$y = 38
$res = $rustfn::TestRust(13, 38)

Write-Host "$x + $y = $res"

