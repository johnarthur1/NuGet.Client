// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using FluentAssertions;
using NuGet.CommandLine.Test;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.FuncTest.Commands
{
    public class SearchCommandTest
    {
        [Fact]
        public void SearchCommand_BasicTest()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var server = new MockServer())
            using (var config = new SimpleTestPathContext())
            {

                CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    $"source add -name mockSource -source {server.Uri}v3/index.json -configfile {config.NuGetConfig}",
                    waitForExit: true);

                string index = $@"
{{""version"": ""3.0.0"",
  ""resources"": [
    {{
      ""@id"": ""{server.Uri + "search/query"}"",
      ""@type"": ""SearchQueryService"",
      ""comment"": ""Query endpoint of NuGet Search service (primary)""
    }}
    ],
    ""@context"": {{
    ""@vocab"": ""http://schema.nuget.org/services#"",
    ""comment"": ""http://www.w3.org/2000/01/rdf-schema#comment""
  }}
        }}
            ";

                server.Get.Add("/v3/index.json", r => index);



                string queryResult = $@"{{
""@context"": {{
""@vocab"": ""http://schema.nuget.org/schema#"",
""@base"": ""https://api.nuget.org/v3/registration5-semver1/""
}},
""totalHits"": 396,
""data"": [
{{
""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json"",
""@type"": ""Package"",
""registration"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/index.json"",
""id"": ""Newtonsoft.Json"",
""version"": ""12.0.3"",
""description"": ""Json.NET is a popular high-performance JSON framework for .NET"",
""summary"": """",
""title"": ""Json.NET"",
""iconUrl"": ""https://api.nuget.org/v3-flatcontainer/newtonsoft.json/12.0.3/icon"",
""licenseUrl"": ""https://www.nuget.org/packages/Newtonsoft.Json/12.0.3/license"",
""projectUrl"": ""https://www.newtonsoft.com/json"",
""tags"": [
""json""
],
""authors"": [
""James Newton-King""
],
""totalDownloads"": 531607259,
""verified"": true,
""packageTypes"": [
{{
""name"": ""Dependency""
}}
],
""versions"": [{{
""version"": ""3.5.8"",
""downloads"": 461992,
""@id"": ""https://api.nuget.org/v3/registration5-semver1/newtonsoft.json/3.5.8.json""
}}]
}}";

                server.Get.Add("/search/query?q=logging&skip=0&take=20&prerelease=false&semverLevel=2.0.0", r => queryResult);


                server.Start();

                // Act
                var args = new[]
                {
                    "search",
                    "logging",
                };

                var result = CommandRunner.Run(
                    nugetexe,
                    config.WorkingDirectory,
                    string.Join(" ", args),
                    waitForExit: true);

                server.Stop();

                // Assert
                Assert.True(0 == result.Item1, $"{result.Item2} {result.Item3}");
                Assert.Contains("foo", $"{result.Item2} {result.Item3}");
            }
        }

    }
}
