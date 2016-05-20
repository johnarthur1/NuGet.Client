﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Core.v3.Tests
{
    public class LocalFolderUtilityTests
    {
        [Fact]
        public void LocalFolderUtility_GetPackagesV3MaxPathTest()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (int i=0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;
                Exception actual = null;

                // Act
                try
                {
                    var packages = LocalFolderUtility.GetPackagesV3(path, testLogger).ToList();
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2MaxPathTest()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (int i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;

                Exception actual = null;

                // Act
                try
                {
                    var packages = LocalFolderUtility.GetPackagesV2(path, testLogger).ToList();
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV3MaxPathTest()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (int i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;
                Exception actual = null;

                // Act
                try
                {
                    var package = LocalFolderUtility.GetPackageV3(path, "a", NuGetVersion.Parse("1.0.0"), testLogger);
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2MaxPathTest()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var longString = string.Empty;

                for (int i = 0; i < 1000; i++)
                {
                    longString += "abcdef";
                }

                var path = root + Path.DirectorySeparatorChar + longString;
                Exception actual = null;

                // Act
                try
                {
                    var package = LocalFolderUtility.GetPackageV2(path, "a", NuGetVersion.Parse("1.0.0"), testLogger);
                }
                catch (Exception ex)
                {
                    actual = ex;
                }

                // Assert
                Assert.True(actual is FatalProtocolException);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2ValidPackage()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                SimpleTestPackageUtility.CreateFolderFeedV2(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                SimpleTestPackageUtility.CreateFolderFeedV2(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                SimpleTestPackageUtility.CreateFolderFeedV2(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, testLogger)
                    .OrderBy(p => p.Identity.Id, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(p => p.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(3, packages.Count);
                Assert.Equal(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), packages[0].Identity);
                Assert.Equal(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), packages[1].Identity);
                Assert.Equal(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), packages[2].Identity);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2ReadWithV3()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                SimpleTestPackageUtility.CreateFolderFeedV2(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                SimpleTestPackageUtility.CreateFolderFeedV2(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                SimpleTestPackageUtility.CreateFolderFeedV2(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, testLogger);

                // Assert
                Assert.Equal(0, packages.Count());
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV3ValidPackage()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, testLogger)
                    .OrderBy(p => p.Identity.Id, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(p => p.Identity.Version)
                    .ToList();

                // Assert
                Assert.Equal(3, packages.Count);
                Assert.Equal(new PackageIdentity("a", NuGetVersion.Parse("1.0.0")), packages[0].Identity);
                Assert.Equal(new PackageIdentity("b", NuGetVersion.Parse("1.0.0")), packages[1].Identity);
                Assert.Equal(new PackageIdentity("c", NuGetVersion.Parse("1.0.0")), packages[2].Identity);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackagesV3ReadWithV2()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, new PackageIdentity("b", NuGetVersion.Parse("1.0.0")));
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, new PackageIdentity("c", NuGetVersion.Parse("1.0.0")));

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, testLogger);

                // Assert
                Assert.Equal(0, packages.Count());
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                SimpleTestPackageUtility.CreateFolderFeedV2(root, a);
                SimpleTestPackageUtility.CreateFolderFeedV2(root, b);
                SimpleTestPackageUtility.CreateFolderFeedV2(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(root, a, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                Assert.Equal(a, foundA.GetReader().GetIdentity());
                Assert.Contains("a.1.0.0.nupkg", foundA.Path);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2NotFound()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                SimpleTestPackageUtility.CreateFolderFeedV2(root, b);
                SimpleTestPackageUtility.CreateFolderFeedV2(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2NotFoundEmptyDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2NotFoundMissingDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV2(Path.Combine(root, "missing"), a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV2NonNormalizedVersions()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a1 = new PackageIdentity("a", NuGetVersion.Parse("1.0"));
                var a2 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var a3 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0.0"));
                var a4 = new PackageIdentity("a", NuGetVersion.Parse("1.0.00"));

                SimpleTestPackageUtility.CreateFolderFeedV2(root, a1);
                SimpleTestPackageUtility.CreateFolderFeedV2(root, a2);
                SimpleTestPackageUtility.CreateFolderFeedV2(root, a3);
                SimpleTestPackageUtility.CreateFolderFeedV2(root, a4);

                // Act
                var foundA1 = LocalFolderUtility.GetPackageV2(root, a1, testLogger);
                var foundA2 = LocalFolderUtility.GetPackageV2(root, a2, testLogger);
                var foundA3 = LocalFolderUtility.GetPackageV2(root, a3, testLogger);
                var foundA4 = LocalFolderUtility.GetPackageV2(root, a4, testLogger);

                // Assert
                Assert.Equal("1.0", foundA1.Nuspec.GetVersion().ToString());
                Assert.Equal("1.0.0", foundA2.Nuspec.GetVersion().ToString());
                Assert.Equal("1.0.0.0", foundA3.Nuspec.GetVersion().ToString());
                Assert.Equal("1.0.00", foundA4.Nuspec.GetVersion().ToString());
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2NotFoundEmptyDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV2NotFoundMissingDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(Path.Combine(root, "missing"), testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV2NotFoundEmptyDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV2NotFoundMissingDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV3()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3(root, a);
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(root, a, testLogger);

                // Assert
                Assert.Equal(a, foundA.Identity);
                Assert.Equal(a, foundA.Nuspec.GetIdentity());
                Assert.True(foundA.IsNupkg);
                Assert.Equal(a, foundA.GetReader().GetIdentity());
                Assert.Contains("a.1.0.0.nupkg", foundA.Path);
            }
        }

        [Fact]
        public async Task LocalFolderUtility_GetPackageV3NotFound()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
                var c = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3(root, b);
                await SimpleTestPackageUtility.CreateFolderFeedV3(root, c);

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV3NotFoundEmptyDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(root, a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackageV3NotFoundMissingDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var a = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));

                // Act
                var foundA = LocalFolderUtility.GetPackageV3(Path.Combine(root, "missing"), a, testLogger);

                // Assert
                Assert.Null(foundA);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV3NotFoundEmptyDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesV3NotFoundMissingDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(Path.Combine(root, "missing"), testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV3NotFoundEmptyDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public void LocalFolderUtility_GetPackagesByIdV3NotFoundMissingDir()
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();

                // Act
                var packages = LocalFolderUtility.GetPackagesV3(root, "a", testLogger).ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        public static IEnumerable<object[]> GetValidVersions()
        {
            foreach (var s in ValidVersions())
            {
                yield return new object[] { s };
            }
        }

        private static IEnumerable<string> ValidVersions()
        {
            yield return "0.0.0";
            yield return "1.0.0";
            yield return "0.0.1";
            yield return "1.0.0-BETA";
            yield return "1.0.0";
            yield return "1.0";
            yield return "1.0.0.0";
            yield return "1.0.1";
            yield return "1.0.01";
            yield return "00000001.000000000.0000000001";
            yield return "00000001.000000000.0000000001-beta";
            yield return "1.0.01-alpha";
            yield return "1.0.1-alpha.1.2.3";
            yield return "1.0.1-alpha.1.2.3+metadata";
            yield return "1.0.1-alpha.1.2.3+a.b.c.d";
            yield return "1.0.1+metadata";
            yield return "1.0.1+security.fix.ce38429";
            yield return "1.0.1-alpha.10.a";
            yield return "1.0.1--";
            yield return "1.0.1-a.really.long.version.release.label";
            yield return "1238234.198231.2924324.2343432";
            yield return "1238234.198231.2924324.2343432+final";
            yield return "00.00.00.00-alpha";
            yield return "0.0-alpha.1";
            yield return "9.9.9-9";
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public void LocalFolderUtility_VerifyPackageCanBeFoundV2_NonNormalizedOnDisk(string versionString)
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("a", version);

                SimpleTestPackageUtility.CreateFolderFeedV2(root, identity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV2(root, "a", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV2(root, "a", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV2(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public void LocalFolderUtility_VerifyPackageCanBeFoundV2_NormalizedOnDisk(string versionString)
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("a", version);
                var normalizedIdentity = new PackageIdentity("a", normalizedVersion);

                SimpleTestPackageUtility.CreateFolderFeedV2(root, normalizedIdentity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV2(root, "a", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV2(root, "a", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV2(root, "a", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV2(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public async Task LocalFolderUtility_VerifyPackageCanBeFoundV3_NonNormalizedOnDisk(string versionString)
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("a", version);

                await SimpleTestPackageUtility.CreateFolderFeedV3(root, identity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV3(root, "a", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV3(root, "a", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV3(root, "a", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV3(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [MemberData("GetValidVersions")]
        public async Task LocalFolderUtility_VerifyPackageCanBeFoundV3_NormalizedOnDisk(string versionString)
        {
            using (var root = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var testLogger = new TestLogger();
                var version = NuGetVersion.Parse(versionString);
                var normalizedVersion = NuGetVersion.Parse(NuGetVersion.Parse(versionString).ToNormalizedString());
                var identity = new PackageIdentity("a", version);
                var normalizedIdentity = new PackageIdentity("a", normalizedVersion);

                await SimpleTestPackageUtility.CreateFolderFeedV3(root, normalizedIdentity);

                // Act
                var findPackage = LocalFolderUtility.GetPackageV3(root, "a", version, testLogger);
                var findPackageNormalized = LocalFolderUtility.GetPackageV3(root, "a", normalizedVersion, testLogger);
                var findById = LocalFolderUtility.GetPackagesV3(root, "a", testLogger).Single();
                var findAll = LocalFolderUtility.GetPackagesV3(root, testLogger).Single();

                // Assert
                Assert.Equal(identity, findPackage.Identity);
                Assert.Equal(identity, findPackageNormalized.Identity);
                Assert.Equal(identity, findById.Identity);
                Assert.Equal(identity, findAll.Identity);
            }
        }

        [Theory]
        [InlineData("packageA.1.0.0.nupkg", "packageA", "packageA.1.0.0")]
        [InlineData("packageA.1.0.nupkg", "packageA", "packageA.1.0")]
        [InlineData("packageA.1.0.0.0.nupkg", "packageA", "packageA.1.0.0.0")]
        [InlineData("packageA.1.0.0-alpha.nupkg", "packageA", "packageA.1.0.0-alpha")]
        [InlineData("packageA.1.0.0-alpha.1.2.3.nupkg", "packageA", "packageA.1.0.0-alpha.1.2.3")]
        [InlineData("packageA.1.0.0-alpha.1.2.3+a.b.c.nupkg", "packageA", "packageA.1.0.0-alpha.1.2.3")]
        [InlineData("packageA.1.0.01.nupkg", "packageA", "packageA.1.0.01")]
        [InlineData("packageA.0001.0.01.nupkg", "packageA", "packageA.0001.0.01")]
        [InlineData("packageA.1.1.1.nupkg", "packageA.1", "packageA.1.1.1")]
        [InlineData("packageA.1.1.1.1.nupkg", "packageA.1.1", "packageA.1.1.1.1")]
        [InlineData("packageA.01.1.1.nupkg", "packageA.01", "packageA.01.1.1")]
        [InlineData("packageA..1.1.nupkg", "packageA.", "packageA..1.1")]
        [InlineData("a.1.0.nupkg", "a", "a.1.0")]
        public void LocalFolderUtility_GetIdentityFromFile_Valid(string fileName, string id, string expected)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));

            // Act
            var identity = LocalFolderUtility.GetIdentityFromNupkgPath(file, id);

            // Assert
            Assert.Equal(id, identity.Id);
            Assert.Equal(expected, $"{identity.Id}.{identity.Version.ToString()}");
        }

        [Theory]
        [InlineData("packageA.x.nupkg", "packageA")]
        [InlineData("packageA.nupkg", "packageA")]
        [InlineData("packageA.nupkg", "packageB")]
        [InlineData("packageA.nupkg", "packageAAA")]
        [InlineData("packageA.1.0.0.0.0.nupkg", "packageA")]
        [InlineData("packageA.1.0.0-beta-#.nupkg", "packageA")]
        [InlineData("packageA.1.0.0-beta.1.01.nupkg", "packageA")]
        [InlineData("packageA.1.0.0-beta+a+b.nupkg", "packageA")]
        [InlineData("1", "packageA")]
        [InlineData("1.nupkg", "packageA")]
        [InlineData("packageB.1.0.0.nupkg", "packageA")]
        [InlineData("packageB.1.0.0.nupkg", "packageB1.0.0.0")]
        [InlineData("packageB.1.0.0.nuspec", "packageB")]
        [InlineData("file", "packageB")]
        [InlineData("file.txt", "packageB")]
        [InlineData("packageA.1.0.0.symbols.nupkg", "packageA")]
        public void LocalFolderUtility_GetIdentityFromFile_Invalid(string fileName, string id)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));

            // Act
            var identity = LocalFolderUtility.GetIdentityFromNupkgPath(file, id);

            // Assert
            Assert.Null(identity);
        }

        [Theory]
        [InlineData("packageA.1.0.0.0.0-beta.nupkg", "packageA", false)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA", true)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA.1", true)]
        [InlineData("packageA.1.0.0-beta-#.nupkg", "packageA", false)]
        [InlineData("packageA.1.0.0-beta.txt", "packageA.1", false)]
        public void LocalFolderUtility_IsPossiblePackageMatch(string fileName, string id, bool expected)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));

            // Act
            var result = LocalFolderUtility.IsPossiblePackageMatch(file, id);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("packageA.1.0.0.0.0-beta.nupkg", "packageA", "1.0.0", false)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA", "1.0.0-beta", true)]
        [InlineData("packageA.1.0.0-beta.symbols.nupkg", "packageA", "1.0.0-beta", false)]
        [InlineData("packageA.1.0.0-beta.nupkg", "packageA.1", "0.0-beta", true)]
        [InlineData("packageA.1.0.0-beta-#.nupkg", "packageA", "1.0.0", false)]
        [InlineData("packageA.1.0.0-beta.txt", "packageA.1", "1.0.0", false)]
        public void LocalFolderUtility_IsPossiblePackageMatch(string fileName, string id, string version, bool expected)
        {
            // Arrange
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName));
            var identity = new PackageIdentity(id, NuGetVersion.Parse(version));

            // Act
            var result = LocalFolderUtility.IsPossiblePackageMatch(file, identity);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
