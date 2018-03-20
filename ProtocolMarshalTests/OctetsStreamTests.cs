using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtocolMarshal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolMarshal.Tests
{
    [TestClass()]
    public class OctetsStreamTests
    {
        [TestMethod()]
        public void MarshalTest()
        {
            var marshall = new OctetsStream();
            Assert.AreEqual(0, marshall.Size);
            marshall.Marshal((byte)3);
            Assert.AreEqual(1, marshall.Size);

            marshall.Marshal(true);
            Assert.AreEqual(2, marshall.Size);

            marshall.Marshal((short)32);
            Assert.AreEqual(4, marshall.Size);

            marshall.Marshal('Z');
            Assert.AreEqual(6, marshall.Size);

            marshall.Marshal((int)328);
            Assert.AreEqual(10, marshall.Size);

            marshall.Marshal((long)328392);
            Assert.AreEqual(18, marshall.Size);

            marshall.Marshal(3.14159f);
            Assert.AreEqual(22, marshall.Size);

            marshall.Marshal(38293.3234);
            Assert.AreEqual(30, marshall.Size);

            marshall.Compact_uint32(32);
            Assert.AreEqual(31, marshall.Size);

            marshall.Compact_uint32(100);
            Assert.AreEqual(33, marshall.Size);

            marshall.Compact_sint32(-100);
            Assert.AreEqual(35, marshall.Size);
        }

        [TestMethod()]
        public void MarshalStringTest1()
        {
            var marshal = new OctetsStream();
            marshal.Marshal("hello");
            var m = marshal.Size;
        }

        [TestMethod()]
        public void Unmarshal_byteTest()
        {
            var marshall = new OctetsStream();
            Assert.AreEqual(0, marshall.Size);
            marshall.Marshal((byte)3);
            Assert.AreEqual(1, marshall.Size);
        }
    }
}