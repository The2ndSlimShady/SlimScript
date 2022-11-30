param(
    [string]$version = ""
)

if ($version -eq ""){
    Write-Error "No Version String"
    break
}

Remove-Item -Path publish/ -Recurse
dotnet publish -c Release -o publish

$txt = [System.IO.File]::ReadAllLines("InstallerScript.iss")

$txt[4] = "#define MyAppVersion `"${version}`""

[System.IO.File]::WriteAllLines("InstallerScript.iss", $txt)

ISCC.exe /Qp /F"SlimScriptInstaller-v${version}" InstallerScript.iss