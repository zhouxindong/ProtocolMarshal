using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtocolMarshal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolMarshal.Tests
{
    [TestClass()]
    public class OctetsTests
    {
        [TestMethod()]
        public void OctetsTest()
        {
            var octet = new Octets();
            Assert.AreEqual(0, octet.Size);
            Assert.AreEqual(128, octet.Buffer().Length);
        }

        [TestMethod()]
        public void OctetsTest1()
        {
            var octet = new Octets(0); // at least 16
            Assert.AreEqual(16, octet.Buffer().Length);

            octet = new Octets(16);
            Assert.AreEqual(16, octet.Buffer().Length);

            octet = new Octets(20);
            Assert.AreEqual(32, octet.Buffer().Length); // 16* 
        }

        [TestMethod()]
        public void OctetsTest2()
        {
            var octet = new Octets(32);
            var octet2 = new Octets(octet);
            Assert.AreEqual(16, octet2.Buffer().Length);
        }
    }
}