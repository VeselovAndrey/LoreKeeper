param([String]$solutionDirectory="")

# ===== Load settings
. $($(Split-Path $script:MyInvocation.MyCommand.Path) + "\BuildSettings.ps1") -solutionDirectory $solutionDirectory

$totalCode = 0

# ====== Run Dnx test
foreach ($prj in $dnxTests) {
	$dnxTestReportXml = $prj + ".xml"

	cd $($solutionDirectory + "\tests\" + $prj)
	dnx . test -appveyor -xml $dnxTestReportXml
	$errCode = $LASTEXITCODE
	
	$wc = New-Object 'System.Net.WebClient'
	$wc.UploadFile("https://ci.appveyor.com/api/testresults/xunit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\$dnxTestReportXml))

	if ($errCode -ne 0) { exit $errCode; }
}

# ===== Install xUnit console runner and get path to it
&$nugetExecutable install xunit.runner.console -Prerelease -OutputDirectory $packagesDirectory
$xUnitRunner = Get-ChildItem -Path $packagesDirectory -Filter xunit.console.exe -Recurse

# ====== Run console test
foreach ($prj in $consoleTests) {	
	cd $($solutionDirectory + "\tests\" + $prj +"\bin\Release")

	$testAssembly =  ".\" + $prj + ".dll"
	$xUnitParameters = @($testAssembly, "-appveyor")

	&$xUnitRunner.FullName $xUnitParameters
	$errCode = $LASTEXITCODE

	if ($LASTEXITCODE -ne 0) { exit $errCode; }
}

exit 0