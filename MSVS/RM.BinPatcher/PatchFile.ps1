[CmdletBinding()]
Param(
    [Parameter(Mandatory = $True, Position = 1)]
    [string]$targetFile,

    [Parameter(Mandatory = $False, Position = 2)]
    [string]$patchName = ".\patch.txt"
)

[System.Environment]::CurrentDirectory = Get-Location
Add-Type -Path .\RM.BinPatcher.dll

[System.IO.Stream] $fs = $null
[System.IO.Stream] $bakFs = $null
[RM.BinPatcher.Patcher] $patcher = $null

if(Test-Path $patchName){}
else
{
    Write-Host "$patchName not found!"
    exit
}

if(Test-Path $targetFile){}
else
{
    Write-Host "$targetFile not found!"
    exit
}

Try
{
    [string[]]$patchLines = Get-Content $patchName
    $patch = [RM.BinPatcher.Model.Patch]::Parse($patchLines)

    $fs = New-Object System.IO.FileStream $targetFile, "Open", "ReadWrite", "None"
    
    Try
    {
        $bakFile = $targetFile + ".bak"
        $bakFs = New-Object System.IO.FileStream $bakFile, "OpenOrCreate", "Write", "None"
        $fs.CopyTo($bakFs)
    }
    Finally
    {
        if($bakFs) {$bakFs.Close();}
    }

    $patcher = New-Object RM.BinPatcher.Patcher $fs
    $result = $patcher.Apply($patch)

    if ($result.IsSuccess)
    {
        Write-Host "Patch applied! Have fun!"
    }
    else
    {
        Write-Host "Error when applying patch: $result.Message"
    }
}
Catch
{
     Write-Host "Exception occured: $_.Exception.Message"
}
Finally
{
    if($fs) {$fs.Close()}
    if($patcher) {$patcher.Dispose()}
}