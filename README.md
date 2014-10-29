A collection of custom MSBuild tasks.

Right now there is only the one, ReadUpdateSaveAssemblyInfoVersionTask which takes a path to an AssemblyInfo file as a parameter, reads the version from the AssemblyVersion attribute, updates it to be [MAJOR].[MINOR].YYDDD.N, writes it back to the AssemblyInfo file and then outputs the version to an output named GeneratedVersion.

YY = the last two digits of the current year (i.e. 2014 = 14)
DDD = the number of the day in the year (i.e. January 14 would be 014)
N = a monotonically increasing value which resets to 0 every day, representing the build number during a day (i.e. starts at 0, increments by one for each addition build).