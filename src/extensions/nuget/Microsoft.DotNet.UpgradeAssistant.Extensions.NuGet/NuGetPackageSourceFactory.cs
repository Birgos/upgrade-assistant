﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;

namespace Microsoft.DotNet.UpgradeAssistant.Extensions.NuGet
{
    public class NuGetPackageSourceFactory : INuGetPackageSourceFactory
    {
        private const string DefaultPackageSource = "https://api.nuget.org/v3/index.json";

        private readonly ILogger<NuGetPackageSourceFactory> _logger;
        private readonly INuGetSettingsWrapper _nuGetSettingsWrapper;

        public NuGetPackageSourceFactory(ILogger<NuGetPackageSourceFactory> logger, INuGetSettingsWrapper nuGetSettingsWrapper)
        {
            _logger = logger;
            _nuGetSettingsWrapper = nuGetSettingsWrapper;
        }

        public IEnumerable<PackageSource> GetPackageSources(string? path)
        {
            var packageSources = new List<PackageSource>();
            var nugetSettings = _nuGetSettingsWrapper.LoadDefaultSettings(path);
            var sourceProvider = new PackageSourceProvider(nugetSettings);
            packageSources.AddRange(sourceProvider.LoadPackageSources().Where(e => e.IsEnabled));

            if (packageSources.Count == 0)
            {
                packageSources.Add(new PackageSource(DefaultPackageSource));
            }

            _logger.LogDebug("Found package sources: {PackageSources}", packageSources);

            return packageSources;
        }
    }
}
