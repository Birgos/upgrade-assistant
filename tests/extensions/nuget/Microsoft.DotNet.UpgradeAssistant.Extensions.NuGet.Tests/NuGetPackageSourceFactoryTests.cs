// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Configuration;
using Xunit;

namespace Microsoft.DotNet.UpgradeAssistant.Extensions.NuGet.Tests
{
    public class NuGetPackageSourceFactoryTests
    {
        [Fact]
        public void CallGetPackageSourcesWithNullLoadNuGetDefaultConfigs()
        {
            string? path = null;
            using var mock = AutoMock.GetLoose();
            ISettings nugetSettings = CreateSettings(".\\");
            var sut = mock.Mock<INuGetSettingsWrapper>();
            sut.Setup(nugetSettingsWrapper => nugetSettingsWrapper.LoadDefaultSettings(path)).Returns(nugetSettings);
            var nugetPackageSourceFactory = new NuGetPackageSourceFactory(new NullLogger<NuGetPackageSourceFactory>(), sut.Object);
            var nugetPackageSources = nugetPackageSourceFactory.GetPackageSources(path);

            sut.Verify(nugetSettingsWrapper => nugetSettingsWrapper.LoadDefaultSettings(path));
        }

        private static Settings CreateSettings(string? path)
        {
            string settingsFileName = "nuget.config";
            var settings = new Settings(path, settingsFileName);

            return settings;
        }

        [Fact]
        public void CallGetPackageSourcesWithSpecificPathLoadNuGetDefaultConfigsWithinSpecifiedPath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            using var mock = AutoMock.GetLoose();
            ISettings nugetSettings = CreateSettings(path);
            var sut = mock.Mock<INuGetSettingsWrapper>();
            sut.Setup(nugetSettingsWrapper => nugetSettingsWrapper.LoadDefaultSettings(path)).Returns(nugetSettings);
            var nugetPackageSourceFactory = new NuGetPackageSourceFactory(new NullLogger<NuGetPackageSourceFactory>(), sut.Object);
            var nugetPackageSources = nugetPackageSourceFactory.GetPackageSources(path);

            sut.Verify(nugetSettingsWrapper => nugetSettingsWrapper.LoadDefaultSettings(path));
            Assert.Equal("https://localhost:4712/nuget", nugetPackageSources.First().Source);
        }
    }
}
