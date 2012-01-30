﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Rtsp.Sdp;

namespace Rtsp.Sdp.Tests
{
    [TestFixture]
    public class SdpFileTest
    {
        Assembly selfAssembly = Assembly.GetExecutingAssembly();

        [Test]
        public void Read1()
        {

            using (var sdpFile = selfAssembly.GetManifestResourceStream("Rtsp.Tests.Sdp.Data.test1.sdp"))
            using (TextReader testReader = new StreamReader(sdpFile))
            {
                SdpFile readenSDP = SdpFile.Read(testReader);

                // Check the reader have read everything
                Assert.AreEqual(string.Empty, testReader.ReadToEnd());
            }

        }

        [Test]
        public void Read2()
        {

            using (var sdpFile = selfAssembly.GetManifestResourceStream("Rtsp.Tests.Sdp.Data.test2.sdp"))
            using (TextReader testReader = new StreamReader(sdpFile))
            {
                SdpFile readenSDP = SdpFile.Read(testReader);

                Assert.AreEqual(0, readenSDP.Version);
                Assert.AreEqual("Teleste", readenSDP.Origin.Username);
                Assert.AreEqual("749719680", readenSDP.Origin.SessionId);
                Assert.AreEqual(2684264576, readenSDP.Origin.SessionVersion);
                Assert.AreEqual("IN", readenSDP.Origin.NetType);
                Assert.AreEqual("IP4", readenSDP.Origin.AddressType);
                Assert.AreEqual("172.16.200.193", readenSDP.Origin.UnicastAddress);
                Assert.AreEqual("COD_9003-P2-0", readenSDP.Session);
                Assert.AreEqual("Teleste MPH H.264 Encoder - HK01121135", readenSDP.SessionInformation);
                Assert.AreEqual(1, readenSDP.Connection.NumberOfAddress, "Number of address");
                Assert.IsInstanceOf<ConnectionIP4>(readenSDP.Connection);
                Assert.AreEqual(16,(readenSDP.Connection as ConnectionIP4).Ttl);
                Assert.AreEqual(1, readenSDP.Timings.Count);
                Assert.Fail("Timing not well implemented...");
                // Check the reader have read everything
                Assert.AreEqual(string.Empty, testReader.ReadToEnd());
            }

        }
        [Test]
        public void Read3()
        {

            using (var sdpFile = selfAssembly.GetManifestResourceStream("Rtsp.Tests.Sdp.Data.test3.sdp"))
            using (TextReader testReader = new StreamReader(sdpFile))
            {
                SdpFile readenSDP = SdpFile.Read(testReader);

                // Check the reader have read everything
                Assert.AreEqual(string.Empty, testReader.ReadToEnd());
            }

        }
    }
}