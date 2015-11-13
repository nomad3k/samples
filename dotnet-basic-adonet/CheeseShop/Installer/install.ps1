[CmdletBinding()]
Param (
	[Parameter(Position=0,Mandatory = $true)]
	[string] $Profile,
	
	[Parameter(Position=1,Mandatory = $false)]
	[string] $Deployment = ""
)

. .\install_tools.ps1

$scriptDir = split-path -parent $MyInvocation.MyCommand.Definition
$buildDir = Get-BuildDir $scriptDir
$profilesDir = Get-ProfilesDir $scriptDir

$validatedDeployment = Get-ValidatedDeployment $profilesDir $Deployment
$validatedProfile = Get-ValidatedProfile $profilesDir $validatedDeployment $Profile
$installDir = "C:\inetpub\$validatedDeployment\"

Write-Host "Installing External Portal Client for deployment: $validatedDeployment and profile: $validatedProfile"

New-Item $installDir -ItemType Directory -ErrorAction SilentlyContinue | Out-Null

$profileDir = Get-ProfileDir $profilesDir $validatedDeployment $validatedProfile

Enable-MaintenancePage $profileDir $installDir

Remove-WebSite $installDir

Copy-Build $buildDir $installDir
Apply-Profile $profileDir $installDir

Disable-MaintenancePage $installDir

Write-Host "Install complete"
