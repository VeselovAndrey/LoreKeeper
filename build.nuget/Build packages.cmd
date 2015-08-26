@echo off 
del release /F /Q
rmdir release
mkdir release
nuget pack LoreKeeper.nuspec -Symbols -OutputDirectory release
nuget pack LoreKeeper.Core.nuspec -Symbols -OutputDirectory release
nuget pack LoreKeeper.EF6.nuspec -Symbols -OutputDirectory release
nuget pack LoreKeeper.Bootstrappers.Autofac.nuspec -Symbols -OutputDirectory release
pause