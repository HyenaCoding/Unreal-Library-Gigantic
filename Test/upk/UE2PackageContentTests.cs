﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection;
using UELib;
using UELib.Core;
using UELib.Engine;

namespace Eliot.UELib.Test.upk
{
    [TestClass]
    public class UE2PackageContentTests
    {
        public static UnrealPackage GetScriptPackageLinker()
        {
            string packagePath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "upk",
                "TestUC2", "TestUC2.u");
            var linker = UnrealLoader.LoadPackage(packagePath);
            Assert.IsNotNull(linker);
            return linker;
        }

        public static UnrealPackage GetMapPackageLinker(string fileName)
        {
            string packagePath = Path.Join(Packages.UE2MapFilesPath, fileName);
            var linker = UnrealLoader.LoadPackage(packagePath);
            Assert.IsNotNull(linker);
            return linker;
        }

        public static UnrealPackage GetMaterialPackageLinker(string fileName)
        {
            string packagePath = Path.Join(Packages.UE2MaterialFilesPath, fileName);
            var linker = UnrealLoader.LoadPackage(packagePath);
            Assert.IsNotNull(linker);
            return linker;
        }

        [TestMethod]
        public void TestScriptContent()
        {
            using var linker = GetScriptPackageLinker();
            Assert.IsNotNull(linker);
            linker.InitializePackage();

            var exports = linker.Objects
                .Where(obj => (int)obj > 0)
                .ToList();

            UnrealPackageTests.AssertTestClass(linker);
            
            var defaults = UnrealPackageTests.AssertDefaultPropertiesClass(linker);
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "String",
                "\"String_\\\"\\\\0abfnrtv\"");
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Float",
                "0.0123457");
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Vector",
                "(X=1.0000000,Y=2.0000000,Z=3.0000000)");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Plane",
                "(W=0.0000000,X=1.0000000,Y=2.0000000,Z=3.0000000)");
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Rotator",
                "(Pitch=180,Yaw=90,Roll=45)");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Coords",
                "(Origin=(X=0.2000000,Y=0.4000000,Z=1.0000000)," +
                "XAxis=(X=1.0000000,Y=0.0000000,Z=0.0000000)," +
                "YAxis=(X=0.0000000,Y=1.0000000,Z=0.0000000)," +
                "ZAxis=(X=0.0000000,Y=0.0000000,Z=1.0000000))");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Quat",
                "(X=1.0000000,Y=2.0000000,Z=3.0000000,W=4.0000000)");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Range",
                "(Min=80.0000000,Max=40.0000000)");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Scale",
                "(Scale=(X=1.0000000,Y=2.0000000,Z=3.0000000),SheerRate=5.0000000,SheerAxis=6)");
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Color",
                "(R=80,G=40,B=20,A=160)");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Box",
                "(Min=(X=0.0000000,Y=1.0000000,Z=2.0000000)," +
                "Max=(X=0.0000000,Y=2.0000000,Z=1.0000000),IsValid=1)");
            // Not atomic!
            UnrealPackageTests.AssertPropertyTagFormat(defaults, "Matrix",
                "(XPlane=(W=0.0000000,X=1.0000000,Y=2.0000000,Z=3.0000000)," +
                "YPlane=(W=4.0000000,X=5.0000000,Y=6.0000000,Z=7.0000000)," +
                "ZPlane=(W=8.0000000,X=9.0000000,Y=10.0000000,Z=11.0000000)," +
                "WPlane=(W=12.0000000,X=13.0000000,Y=14.0000000,Z=15.0000000))");

            UnrealPackageTests.AssertExportsOfType<UClass>(exports);
        }

        [TestMethod]
        public void TestMapContent()
        {
            using var linker = GetMapPackageLinker("DM-Rankin.ut2");
            linker.InitializePackage();

            var exports = linker.Objects
                .Where(obj => (int)obj > 0)
                .ToList();

            UnrealPackageTests.AssertExportsOfType<USound>(exports);
            UnrealPackageTests.AssertExportsOfType<UPolys>(exports);
        }

        [TestMethod]
        public void TestMaterialContent()
        {
            using var linker = GetMaterialPackageLinker("2k4Fonts.utx");
            linker.InitializePackage();

            var exports = linker.Objects
                .Where(obj => (int)obj > 0)
                .ToList();

            UnrealPackageTests.AssertExportsOfType<UFont>(exports);
            UnrealPackageTests.AssertExportsOfType<UPalette>(exports);
            UnrealPackageTests.AssertExportsOfType<UTexture>(exports);
        }
    }
}