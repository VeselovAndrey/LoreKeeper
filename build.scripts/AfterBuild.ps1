param([String]$solutionDirectory="")

# ===== Load settings
. $($(Split-Path $script:MyInvocation.MyCommand.Path) + "\BuildSettings.ps1") -solutionDirectory $solutionDirectory

$solutionDirectory

# ===== Clean release folder
if (test-path $outputPackagesDirectory) {	
    Remove-Item $outputPackagesDirectory -Recurse
}
New-Item $outputPackagesDirectory -Type Directory *>$null

# ====== Build NuGet packages
$allNugetFiles = Get-ChildItem $nuspecDirectory -Recurse
$nuspecFiles = $allNugetFiles | where {$_.Name.EndsWith(".nuspec") }

foreach ($nuspecFile in $nuspecFiles) {
    &$nugetExecutable pack $nuspecFile.FullName -Symbols -OutputDirectory $outputPackagesDirectory
}