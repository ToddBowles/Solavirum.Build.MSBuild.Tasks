using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solavirum.Build.MSBuild.Tasks
{
    public interface VersionGenerator
    {
        Version GenerateNewVersionFromSeed(Version seed);
    }
}
