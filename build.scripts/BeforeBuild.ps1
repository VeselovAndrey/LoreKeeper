param([String]$solutionDirectory="")

# ===== Load settings
. $($(Split-Path $script:MyInvocation.MyCommand.Path) + "\BuildSettings.ps1") -solutionDirectory $solutionDirectory

# ===== Restore packages for LoreKeeper
foreach ($prj in $portableProjects) {
	cd $($solutionDirectory + $prj)

	&$nugetExecutable restore -PackagesDirectory $packagesDirectory
}