using Microsoft.Build.Framework;
using Solavirum.Build.MSBuild.Tasks.Implementation;
using System;

namespace Solavirum.Build.MSBuild.Tasks
{
    public class ReadUpdateSaveAssemblyInfoVersionTask : ITask
    {
        public ReadUpdateSaveAssemblyInfoVersionTask()
            : this(new BuildToJulianRevisionIncrementVersionGenerator(new DateTimeNowCurrentTimeProvider()), new AssemblyVersionAssemblyInfoVersionInteractor())
        {

        }

        public ReadUpdateSaveAssemblyInfoVersionTask(VersionGenerator versionGenerator, AssemblyInfoVersionInteractor assemblyInfoVersionInteractor)
        {
            _VersionGenerator = versionGenerator;
            _AssemblyInfoVersionInteractor = assemblyInfoVersionInteractor;
        }

        private readonly VersionGenerator _VersionGenerator;
        private readonly AssemblyInfoVersionInteractor _AssemblyInfoVersionInteractor;

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }


        public string AssemblyInfoSourcePath { get; set; }

        [Output]
        public string GeneratedVersion { get; set; }

        public bool Execute()
        {
            var current = new Version(_AssemblyInfoVersionInteractor.ReadVersion(AssemblyInfoSourcePath));
            var newVersion = _VersionGenerator.GenerateNewVersionFromSeed(current);
            _AssemblyInfoVersionInteractor.WriteVersion(newVersion.ToString(), AssemblyInfoSourcePath);

            GeneratedVersion = newVersion.ToString();
            return true;
        }
    }
}
