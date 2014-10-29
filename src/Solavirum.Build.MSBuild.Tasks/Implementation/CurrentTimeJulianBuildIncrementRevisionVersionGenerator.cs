using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solavirum.Build.MSBuild.Tasks.Implementation
{
    public class BuildToJulianRevisionIncrementVersionGenerator : VersionGenerator
    {
        public BuildToJulianRevisionIncrementVersionGenerator(CurrentTimeProvider now)
        {
            _TimeProvider = now;
        }

        private readonly CurrentTimeProvider _TimeProvider;

        public Version GenerateNewVersionFromSeed(Version seed)
        {
            var newBuild = GenerateJulianDate(_TimeProvider.Now());
            int newRevision = 0;
            if (ShouldIncrementRevision(seed.Build, newBuild))
            {
                newRevision = seed.Revision + 1;
            }
            return new Version(seed.Major, seed.Minor, newBuild, newRevision);
        }

        private int GenerateJulianDate(DateTimeOffset seedDate)
        {
            var converted = seedDate.ToString("yy") + seedDate.DayOfYear;
            var asInteger = int.Parse(converted);

            return asInteger;
        }

        private bool ShouldIncrementRevision(int oldBuild, int newBuild)
        {
            return oldBuild.Equals(newBuild);
        }
    }
}
