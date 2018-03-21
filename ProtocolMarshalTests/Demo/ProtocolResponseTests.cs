using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace ProtocolMarshal.Demo.Tests
{
    [TestClass()]
    public class ProtocolResponseTests
    {
        [TestMethod()]
        public void UnmarshalTest()
        {
            // serialize
            var p1 = new ProtocolResponse
            {
                MessageResponse = new MessageResponse(1, "ok", 10000),
                LoginStage = 1,
                Sex = 0,
                Name = "John",
                Param = new Dictionary<string, int> {{"aa", 11}},
                EntitySet = new HashSet<MessageEntity> {new MessageEntity(1, 11, false, "xxx1")},
                EntityList = new List<MessageEntity> {new MessageEntity(2, 22, false, "xxx2")},
                EntityArray = new MessageEntity[1]
            };

            p1.EntityArray[0] = new MessageEntity(4, 44, false, "xxx4");

            byte[] info = p1.ToBytes();

            var file_name = @"response_demo.dat";
            var fs = new FileStream(file_name, FileMode.Create);
            fs.Write(info, 0, info.Length);
            fs.Flush();
            fs.Close();

            // deserialize
            fs = new FileStream(file_name, FileMode.Open);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            var p2 = new ProtocolResponse();
            p2.ParseFrom(info);

            Assert.AreEqual(p1.MessageResponse, p2.MessageResponse);
            Assert.AreEqual(p1.LoginStage, p2.LoginStage);
            Assert.AreEqual(p1.Sex, p2.Sex);
            Assert.AreEqual(p1.Name, p2.Name);
            Assert.AreEqual(p1.Param.Count, p2.Param.Count);
            Assert.AreEqual(p1.EntityArray.Length, p2.EntityArray.Length);
            Assert.AreEqual(p1.EntityList.Count, p2.EntityList.Count);
            Assert.AreEqual(p1.EntitySet.Count, p2.EntitySet.Count);
        }
    }
}