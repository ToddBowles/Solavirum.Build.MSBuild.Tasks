A collection of custom MSBuild tasks.

# Description
Right now there is only the one, ReadUpdateSaveAssemblyInfoVersionTask which takes a path to an AssemblyInfo file as a parameter, reads the version from the AssemblyVersion attribute, updates it to be [MAJOR].[MINOR].YYDDD.N, writes it back to the AssemblyInfo file and then outputs the version to an output named GeneratedVersion.

* YY = the last two digits of the current year (i.e. 2014 = 14)
* DDD = the number of the day in the year (i.e. January 14 would be 014)
* N = a monotonically increasing value which resets to 0 every day, representing the build number during a day (i.e. starts at 0, increments by one for each addition build).
* [MAJOR] and [MINOR] are just whatever the values were in the AssemblyVersion attribute.

# Usage
This is a contrived example, where it simply updates the specified AssemblyInfo file before each build and then outputs the version that it was just updated to into the build output.

``` XML
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        <GeneratedAssemblyVersion></GeneratedAssemblyVersion>
    </PropertyGroup>
    <UsingTask TaskName="Solavirum.Build.MSBuild.Tasks.ReadUpdateSaveAssemblyInfoVersionTask" AssemblyFile="$(SolutionDir)\tools\Solavirum.Build.MSBuild.Tasks.dll" />
    <Target Name="UpdateVersion">
        <Message Text="Updating AssemblyVersion in AssemblyInfo." Importance="high" />
        <ReadUpdateSaveAssemblyInfoVersionTask AssemblyInfoSourcePath="$(ProjectDir)\Properties\AssemblyInfo.cs">
            <Output TaskParameter="GeneratedVersion" PropertyName="GeneratedAssemblyVersion" />
        </ReadUpdateSaveAssemblyInfoVersionTask>
        <Message Text="New AssemblyVersion is $(GeneratedAssemblyVersion)" Importance="high" />
    <Target Name="BeforeBuild">
        <CallTarget Targets="UpdateVersion" />
    </Target>
</Project>
```
The content of the example would be placed in a .targets file in your project somewhere, and that targets file would be imported into your project file like this 

`<Import Project="$(ProjectDir)\Customized.targets" />`

# Warnings
The task was written for a very specific purpose, so:
* If your AssemblyVersions has * in it anywhere, it will fail.
* If you use any of the other version attributes in your AssemblyInfo it will not update them (it only knows about AssemblyVersion).

Also, it has barely any error handling, so it will just throw exceptions if it doesn't encounter EXACTLY the situtation it expects.
