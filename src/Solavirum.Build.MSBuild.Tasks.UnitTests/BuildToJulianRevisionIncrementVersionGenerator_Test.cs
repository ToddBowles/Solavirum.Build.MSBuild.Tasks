using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Solavirum.Build.MSBuild.Tasks.Implementation;
using System;

namespace Solavirum.Build.MSBuild.Tasks.UnitTests
{
    [TestClass]
    public class BuildToJulianRevisionIncrementVersionGenerator_Test
    {
        [TestMethod]
        public void BuildToJulianRevisionIncrementVersionGenerator_GenerateNewVersionFromSeed_CorrectlySetsBuildToJulianDateOfCurrentTimeProvider()
        {
            var date = new DateTimeOffset(2013, 11, 12, 0, 0, 0, TimeSpan.FromHours(10));
            var nowProvider = Substitute.For<CurrentTimeProvider>();
            nowProvider.Now().Returns(date);

            var version = new Version(1, 0, 0, 0);
            var generator = new BuildToJulianRevisionIncrementVersionGenerator(nowProvider);

            var result = generator.GenerateNewVersionFromSeed(version);

            Assert.AreEqual(13316, result.Build);
        }

        [TestMethod]
        public void BuildToJulianRevisionIncrementVersionGenerator_GenerateNewVersionFromSeed_CorrectlyIncrementsRevisionWhenBuildStaysTheSame()
        {
            var date = new DateTimeOffset(2013, 11, 12, 0, 0, 0, TimeSpan.FromHours(10));
            var nowProvider = Substitute.For<CurrentTimeProvider>();
            nowProvider.Now().Returns(date);

            var version = new Version(1, 0, 13316, 0);
            var generator = new BuildToJulianRevisionIncrementVersionGenerator(nowProvider);

            var result = generator.GenerateNewVersionFromSeed(version);

            Assert.AreEqual(1, result.Revision);
        }

        [TestMethod]
        public void BuildToJulianRevisionIncrementVersionGenerator_GenerateNewVersionFromSeed_SetsBuildToZeroWhenRevisionChanges()
        {
            var date = new DateTimeOffset(2013, 11, 12, 0, 0, 0, TimeSpan.FromHours(10));
            var nowProvider = Substitute.For<CurrentTimeProvider>();
            nowProvider.Now().Returns(date);

            var version = new Version(1, 0, 12578, 56);
            var generator = new BuildToJulianRevisionIncrementVersionGenerator(nowProvider);

            var result = generator.GenerateNewVersionFromSeed(version);

            Assert.AreEqual(0, result.Revision);
        }
    }
}
