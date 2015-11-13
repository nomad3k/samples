Include  ".\build_utils.ps1"

properties {
	$project = "CheeseShop"

	$baseDir = resolve-path .
	$packagesDir = "$baseDir\packages"
	$version = "1.0"
	$toolsDir = "$baseDir\Tools"
	$outputDir = "$baseDir\Output"
	$releaseDir = "$baseDir\Release"
	$global:configuration = "Release"
	$global:platform = "Any CPU"
	$slnFile = "$baseDir\$project.sln"

	$applicationDir = "$baseDir\$project"
	$projectOutputDir = "$outputDir\$project"
	$projectOutputBuildDir = Get-OutputBuildDir $projectOutputDir
	$projectOutputProfilesDir = Get-OutputProfilesDir $projectOutputDir

	$testProjects =
		@( 
			"$baseDir\CheeseShop.Common.Tests\bin\$global:Configuration\CheeseShop.Common.Tests.dll",
			"$baseDir\CheeseShop.Domain.Tests\bin\$global:Configuration\CheeseShop.Domain.Tests.dll"
		 )
}

task Default -depends Release

task Verify40 {
	if ( (Get-ChildItem "$env:windir\Microsoft.NET\Framework\v4.0*") -eq $null ) {
		throw "Building requires .NET 4.0, which doesn't appear to be installed on this machine"
	}
}

task Clean -depends CleanOutputDirectory {
	Remove-Item -Force -Recurse $releaseDir -ErrorAction SilentlyContinue
}

task SetBuildLabel {
	if ($env:BUILD_NUMBER -ne $null) {
		$env:buildlabel = $env:BUILD_NUMBER
	}

	if ($env:buildlabel -eq $null) {
		$env:buildlabel = "13"
	}
}

task SetReportingDataDbVersion {
	$file = "$reportingDatabaseOutputDir\Deployment\_BuildInfo.xml"
	$xml = [xml] (Get-Content $file)
	
	Set-XmlElementValue $xml '/buildInfo/versionMajor' $versionMajor
	Set-XmlElementValue $xml '/buildInfo/versionMinor' $versionMinor
	Set-XmlElementValue $xml '/buildInfo/versionPatch' $env:buildlabel
	Set-XmlElementValue $xml '/buildInfo/version' "${version}.${env:buildlabel}.0" 
	Set-XmlElementValue $xml '/buildInfo/msbuildConfiguration' $global:configuration 
	
	$xml.Save($file)
}

task Init -depends Verify40, Clean, SetBuildLabel {
	
	$commit = Get-GitCommit
	(Get-Content "$baseDir\CommonInfo.cs") |
		ForEach-Object { $_ -replace "\.13", ".$($env:buildlabel)" } |
		ForEach-Object { $_ -replace "{commit}", $commit } |
		Set-Content "$baseDir\CommonInfo.cs" -Encoding UTF8

	New-Item $releaseDir -ItemType directory -ErrorAction SilentlyContinue | Out-Null
}

task Compile -depends Init {

	$v4_net_version = ( Get-ChildItem "$env:windir\Microsoft.NET\Framework\v4.0*").Name

	Write-Host "Compiling for platform '$global:platform' with '$global:configuration' configuration" -ForegroundColor Yellow
#	exec { & "$env:windir\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" "$slnFile" /p:Configuration=$global:configuration /p:Platform=$global:platform }
	exec { & "${env:ProgramFiles(x86)}\MSBuild\12.0\bin\MSBuild.exe" "$slnFile" /p:Configuration=$global:configuration /p:Platform=$global:platform }
}

task Test -depends Compile {
	<# Clear-Host #>

	Write-Host $testProjects

	$nUnit = Get-PackagePath nunit.runners.lite
	Write-Host "nUnit location: $nUnit"

	$assemblies = '"' + ($testProjects -Join '" "') + '"'
	$expression = "$nUnit\nunit-console.exe " + $assemblies + " /result:" + $releaseDir + "\nunit.xml"

	if ($env:BUILD_NUMBER -ne $null) {
		$expression = $expression + " /exclude:Integration"
	}

	Write-Host $expression

	(Invoke-Expression $expression)

	$succeeded = $lastexitcode -eq 0
	
	Write-Host "##teamcity[importData type='nunit' path='$releaseDir\nunit.xml']"

	if ($succeeded -eq $false) {
        throw ("Tests failed. See '$releaseDir\nunit.xml'")
	}
}

task Release -depends DoRelease

task DoRelease -depends Compile, Test, Publish {
	
	Write-Host "Done building"
}

task Publish -depends SetBuildLabel, `
		CleanOutputDirectory, `
		CreateOutputDirectories, `
		CopyApplication, `
		ZipOutput, `
		ResetBuildArtifacts

task CreateOutputDirectories -depends CleanOutputDirectory {
	New-Item "$outputDir" -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
	New-Item "$projectOutputDir" -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
	New-Item "$projectOutputBuildDir" -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
	New-Item "$projectOutputProfilesDir" -ItemType Directory -ErrorAction SilentlyContinue | Out-Null	
}
		
task CleanOutputDirectory {
	Remove-Item "$outputDir" -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
}

task CopyApplication {
	Copy-MatchedFiles -Recurse "$applicationDir" "bin" $projectOutputBuildDir @( '*.dll', '*.pdb' )
	Copy-MatchedFiles -Recurse "$applicationDir" "fonts" $projectOutputBuildDir @( '*.eot', '*.svg', '*.ttf', '*.woff' )
	Copy-MatchedFiles -Recurse "$applicationDir" "Content" $projectOutputBuildDir @( '*.css', '*.jpg', '*.png', '*.gif', '*.xml', '*.js', '*.eot', '*.woff', '*.ttf', '*.svg' )
	Copy-MatchedFiles -Recurse "$applicationDir" "Scripts" $projectOutputBuildDir @( '*.js' )
	Copy-MatchedFiles -Recurse "$applicationDir" "Views" $projectOutputBuildDir @( '*.cshtml', 'web.config' )
	Copy-MatchedFiles "$applicationDir" $null $projectOutputBuildDir @( '*.htm', '*.html', '*.asax', '*.asmx', '*.aspx' )
	
	Copy-Item "$applicationDir\InstallationProfiles\*" -Recurse -Destination $projectOutputProfilesDir | Out-Null
	
	Copy-Item "$applicationDir\Installer\install.ps1" -Destination $projectOutputDir | Out-Null
	Copy-Item "$applicationDir\Installer\install_tools.ps1" -Destination $projectOutputDir | Out-Null
}


task ZipOutput {

	$old = pwd
	cd $outputDir

	try {
		exec { & "$toolsDir\zip.exe" -r -9 "$releaseDir\${project}_v${version}.${env:buildlabel}.zip" "*" }
	}
	finally {
		cd $old
	}
}

task ResetBuildArtifacts {
	exec { git checkout "CommonInfo.cs" }
}
