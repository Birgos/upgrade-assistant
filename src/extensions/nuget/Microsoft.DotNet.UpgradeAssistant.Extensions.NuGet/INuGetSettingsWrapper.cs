// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Configuration;

namespace Microsoft.DotNet.UpgradeAssistant.Extensions.NuGet
{
    public interface INuGetSettingsWrapper
    {
        ISettings LoadDefaultSettings(string? path);
    }
}
