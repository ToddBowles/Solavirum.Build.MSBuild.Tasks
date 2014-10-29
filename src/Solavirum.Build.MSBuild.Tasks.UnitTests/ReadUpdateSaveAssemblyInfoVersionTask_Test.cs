using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IO;
using System.Text.RegularExpressions;

namespace Solavirum.Build.MSBuild.Tasks.UnitTests
{
    [TestClass]
    public class ReadUpdateSaveAssemblyInfoVersionTask_Test
    {
        private const string TestAssemblyInfoFilesDirectory = "TestAssemblyInfoFiles";
        private const string OnlyAssemblyVersionFileName = "OnlyAssemblyVersion.cs";

        private const string RUSAIVT_E_DeploymentDir = "ReadUpdateSaveAssemblyInfoVersionTask_Test";
        [TestMethod]
        [DeploymentItem(TestAssemblyInfoFilesDirectory + @"\" + OnlyAssemblyVersionFileName, RUSAIVT_E_DeploymentDir)]
        public void ReadUpdateSaveAssemblyInfoVersionTask_Execute()
        {
            var oldVersion = "0.0.0.1";

            var assemblyFilePath = Path.Combine(RUSAIVT_E_DeploymentDir, OnlyAssemblyVersionFileName);
            var assemblyVersionAssemblyInfoInteractor = new AssemblyVersionAssemblyInfoVersionInteractor();
            var oldVersionFromFile = assemblyVersionAssemblyInfoInteractor.ReadVersion(assemblyFilePath);
            Assert.AreEqual(oldVersion, oldVersionFromFile);

            var newVersion = new Version(1, 1, 1, 2);
            var versionGenerator = Substitute.For<VersionGenerator>();
            versionGenerator.GenerateNewVersionFromSeed(Arg.Any<Version>()).Returns(newVersion);

            var task = new ReadUpdateSaveAssemblyInfoVersionTask(versionGenerator, assemblyVersionAssemblyInfoInteractor);
            task.AssemblyInfoSourcePath = assemblyFilePath;
            task.Execute();

            var outputVersion = task.GeneratedVersion;

            Assert.AreEqual(newVersion.ToString(), outputVersion);
            var versionFromFile = assemblyVersionAssemblyInfoInteractor.ReadVersion(assemblyFilePath);
            Assert.AreEqual(newVersion.ToString(), versionFromFile);
        }
    }
}
