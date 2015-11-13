function Get-FileExistsOnPath
{
	param(
		[string]$file
	)

	$results = ($Env:Path).Split(";") | Get-ChildItem -filter $file -erroraction silentlycontinue
	$found = ($results -ne $null)
	return $found
}

function Get-GitCommit
{
	if ((Get-FileExistsOnPath "git.exe")) {
		$gitLog = git log --oneline -1
		return $gitLog.Split(' ')[0]
	}
	else {
		return "0000000"		
	}
}

function Set-XmlElementValue([xml] $xml, [string] $xpath, [string] $value) {
	Select-Xml -xml $xml -XPath $xpath | Select-Object -first 1 | ForEach-Object { $_.Node.'#text' = $value }
}

function Copy-MatchedFiles
{
	param(
		[Parameter(Mandatory=$True)]
		[string] $Path,

		[Parameter(Mandatory=$False)]
		[string] $SubPath = $null,

		[Parameter(Mandatory=$True)]
		[string] $Destination,

		[Parameter(Mandatory=$False)]
	    [string[]] $FileSpec = $null,

		[Parameter(Mandatory=$False)]
		[switch] $Recurse = $False
		)

	$oldLocation = Get-Location

	try {
		Set-Location $Path

		if ($SubPath -ne $null) {
			$Path = Join-Path $Path $SubPath
		}

		$sourcePathFileInfo = New-Object System.IO.FileInfo($Path)
		$sourcePathDir = $sourcePathFileInfo.DirectoryName

		Create-Directory $Destination

		foreach ($file in Get-ChildItem -Path $Path -Recurse -Include $FileSpec | Resolve-Path -Relative ) {

			$sourcePathName = Join-Path $Path $file

			if (-not $Recurse)
			{
				$sourceFileInfo = New-Object System.IO.FileInfo($sourcePathName)
				$sourceFileDir = $sourceFileInfo.DirectoryName

				if ($sourceFileDir -ne $sourcePathDir) {
					continue
				}
			}

			$destinationFileName = Join-Path $Destination $file
			$destinationFile = New-Object System.IO.FileInfo($destinationFileName)
			$dir = $destinationFile.Directory

			Create-Directory $destinationFile.Directory
			Copy-Item $file $destinationFile.FullName
		}
	}
	Finally {
		Set-Location $oldLocation
	}
}

function Create-Directory([string] $directory) {
	New-Item "$directory" -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
}

function Get-OutputBuildDir([string] $outputPath) {
	return Join-Path $outputPath "build"
}

function Get-OutputProfilesDir([string] $outputPath) {
	return Join-Path $outputPath "profiles"
}

function Rename-AppConfigFiles([string] $profilesPath, [string] $exeName) {
	
	foreach ($file in Get-ChildItem -Recurse $profilesPath -Filter "app.config") {
		$dirName = $file.DirectoryName
		$destFilename = "${dirName}\${exeName}.config"
		Move-Item $file.FullName $destFilename | Out-Null
	}
}

Function Get-PackagePath {
	Param(
		[string]$packageName
	)

	$packagePath = Get-ChildItem "$packagesDir\$packageName.*" |
						Sort-Object Name -Descending | 
						Select-Object -First 1
	Return "$packagePath"
}