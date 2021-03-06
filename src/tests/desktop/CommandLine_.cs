using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Desktop;
using NUnit.Framework;

namespace Nohros.Test.Desktop
{
    [TestFixture]
    public class CommandLine_
    {
        const string kTestCommandLine = "nohros.exe loose_00 loose_01 --switch_00=00 --switch_01 -switch_02=02 --switch_03:J:\\sex\\bigass.avi";

        [Test]
        public void CommandLine_ctor()
        {
            CommandLine command_line = new CommandLine("nohros.exe");
            Assert.AreEqual("nohros.exe", command_line.Program);
            Assert.AreEqual("nohros.exe", command_line.CommandLineString);
        }

        [Test]
        public void CommandLine_ForCurrentProcess() {
            CommandLine command_line = CommandLine.ForCurrentProcess;
            Assert.AreEqual(Environment.CommandLine, command_line.CommandLineString);
        }

        [Test]
        public void CommandLine_LooseValues() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.ParseFromString(kTestCommandLine);
            Assert.AreEqual(2, command_line.LooseValues.Count);
            Assert.AreEqual("loose_00", command_line.LooseValues[0]);
            Assert.AreEqual("loose_01", command_line.LooseValues[1]);
        }

        [Test]
        public void CommandLine_GetSwitchValue() {
            CommandLine command_line = CommandLine.FromString(kTestCommandLine);

            Assert.AreEqual("00", command_line.GetSwitchValue("switch_00"));
            Assert.AreEqual(string.Empty, command_line.GetSwitchValue("switch_01"));
            Assert.AreEqual("02", command_line.GetSwitchValue("switch_02"));
            Assert.AreEqual("J:\\sex\\bigass.avi", command_line.GetSwitchValue("switch_03"));
        }

        [Test]
        public void CommandLine_AppendSwitch() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.AppendSwitch("V", "/");
            Assert.AreEqual(1, command_line.SwitchCount);
            Assert.AreEqual(true, command_line.HasSwitch("V"));
            Assert.AreEqual(string.Empty, command_line.GetSwitchValue("V"));
            Assert.AreEqual("nohros.exe /V", command_line.CommandLineString);
        }

        [Test]
        public void CommandLine_AppendSwitchWithValue() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.AppendSwitchWithValue("path", "o", "c:\\mypath with space", ':');
            Assert.AreEqual(1, command_line.SwitchCount);
            Assert.AreEqual(true, command_line.HasSwitch("path"));
            Assert.AreEqual("c:\\mypath with space", command_line.GetSwitchValue("path"));
            Assert.AreEqual("nohros.exe -path:\"c:\\mypath with space\"", command_line.CommandLineString);
        }

        [Test]
        public void CommandLine_AppendLooseValue() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.AppendLooseValue("debug");
            Assert.AreEqual(1, command_line.LooseValues.Count);
            Assert.AreEqual("debug", command_line.LooseValues[0]);
            Assert.AreEqual("nohros.exe debug", command_line.CommandLineString);
        }

        [Test]
        public void CommandLine_Fail() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.AppendSwitch("V", "/");
            Assert.AreEqual(1, command_line.SwitchCount);
            Assert.AreEqual(true, command_line.HasSwitch("V"));
            Assert.AreEqual(string.Empty, command_line.GetSwitchValue("V"));
            Assert.AreEqual("nohros.exe /V", command_line.CommandLineString);

            command_line.AppendSwitchWithValue("path", "o", "c:\\mypath with space", ':');
            Assert.AreEqual(2, command_line.SwitchCount);
            Assert.AreEqual(true, command_line.HasSwitch("path"));
            Assert.AreEqual("c:\\mypath with space", command_line.GetSwitchValue("path"));
            Assert.AreEqual("nohros.exe /V -path:\"c:\\mypath with space\"", command_line.CommandLineString);

            command_line.AppendLooseValue("debug");
            Assert.AreEqual(1, command_line.LooseValues.Count);
            Assert.AreEqual("debug", command_line.LooseValues[0]);
            Assert.AreEqual("nohros.exe /V -path:\"c:\\mypath with space\" debug", command_line.CommandLineString);
            Assert.AreEqual("c:\\mypath with space", command_line.GetSwitchValue("path"));
            Assert.AreEqual("debug", command_line.LooseValues[0]);

            command_line = CommandLine.FromString("nohros.exe \"");
            Assert.AreEqual("nohros.exe", command_line.Program);
            Assert.AreEqual(1, command_line.LooseValues.Count);

            command_line = CommandLine.FromString("nohros.exe \"galo doido\" --switch_00 /switch_01");
            Assert.AreEqual("nohros.exe", command_line.Program);
            Assert.AreEqual(1, command_line.LooseValues.Count);
            Assert.AreEqual("galo doido", command_line.LooseValues[0]);
            Assert.AreEqual(true, command_line.HasSwitch("switch_00"));
            Assert.AreEqual(true, command_line.HasSwitch("switch_01"));

            command_line = CommandLine.FromString("nohros.exe \"galo doido\" --switch_00 /path:J:switch-inside-string=: =loose_with_switch_separator /switch_:");
            Assert.AreEqual(false, command_line.HasSwitch("galo doido"));
            Assert.AreEqual(true, command_line.HasSwitch("switch_00"));
            Assert.AreEqual(true, command_line.HasSwitch("path"));
            Assert.AreEqual("J:switch-inside-string=:", command_line.GetSwitchValue("path"));
            Assert.AreEqual(true, command_line.HasSwitch("switch_"));
            Assert.AreEqual(string.Empty, command_line.GetSwitchValue("switch_"));
            Assert.AreEqual("=loose_with_switch_separator", command_line.LooseValues[1]);

            command_line = CommandLine.FromString("nohros.exe \"galo doido\" --switch_00 -- /path:J:switch-inside-string=: =loose_with_switch_separator /switch_:");
            Assert.AreEqual(false, command_line.HasSwitch("galo doido"));
            Assert.AreEqual(true, command_line.HasSwitch("switch_00"));
            Assert.AreEqual(false, command_line.HasSwitch("path"));
            Assert.AreEqual(false, command_line.HasSwitch("switch_"));
            Assert.AreEqual("/path:J:switch-inside-string=:", command_line.LooseValues[1]);
            Assert.AreEqual("=loose_with_switch_separator", command_line.LooseValues[2]);
            Assert.AreEqual("/switch_:", command_line.LooseValues[3]);
        }
    }
}