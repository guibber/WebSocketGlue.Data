using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketGlue.Data.Tests.Support;
using WebSocketGlue.Data.Utils;

namespace WebSocketGlue.Data.Tests {
  [TestClass]
  public class PacketSerializerTests : BaseTest {
    [TestMethod]
    public void TestSerializationAndDeserializeWithNullData() {
      var obj = One<Packet>();
      obj.ConnectionId = null;
      obj.Data = null;
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    [DataRow("string_data")]
    [DataRow("11")]
    [DataRow("123.45")]
    public void TestSerializationAndDeserializeWithSimpleData(object data) {
      var obj = One<Packet>();
      obj.Data = data;
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationWithAnonDataObject() {
      var obj = One<Request>();
      obj.Data = new {Prop1 = 11};
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationWithDataObject() {
      var obj = One<Request>();
      obj.Data = new object();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationNestedDataWithDataObject() {
      var obj = One<Request>();
      var sub = One<Check>();
      sub.Data = new object();
      obj.Data = sub;

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationWithDataArray() {
      var obj = One<Request>();
      obj.Data = Many<Ack>();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationUnknownObjectType() {
      var obj = One<Request>();
      obj.Data = One<Packet>();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        var s = StreamToString(stream).Replace("WebSocketGlue.Data.Packet, WebSocketGlue.Data", "NotAvailable.Packet, NotAvailable");
        using (var wstream = new MemoryStream()) {
          StringToStream(s, wstream);
          var actual = mSerializer.Deserialize(wstream);
          dynamic actualData = actual.Data;
          Assert.AreEqual(obj.ConnectionId, actual.ConnectionId);
          Assert.AreEqual(((Packet)obj.Data).ConnectionId, actualData.ConnectionId.ToString());
          Assert.IsNotNull(actualData.Data);
        }
      }
    }

    [TestMethod]
    public void TestDeserializationUnloadedReferencedAssemblyXX() {
      var obj = One<Request>();
      obj.Data = One<TestClass1>();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        var s = StreamToString(stream).Replace("WebSocketGlue.Data.Tests.TestClass1, WebSocketGlue.Data.Tests", "TestAssembly.TestClass1, TestAssembly");
        using (var wstream = new MemoryStream()) {
          StringToStream(s, wstream);
          var actual = mSerializer.Deserialize(wstream);
          dynamic actualData = actual.Data;
          Assert.AreEqual(obj.ConnectionId, actual.ConnectionId);
          Assert.AreEqual("TestAssembly.TestClass1", actualData.GetType().ToString());
          Assert.AreEqual(((TestClass1)obj.Data).Prop1.ToString(), actualData.Prop1.ToString());
          Assert.AreEqual(((TestClass1)obj.Data).Prop2, actualData.Prop2.ToString());
        }
      }
    }

    private static string StreamToString(Stream stream) {
      using (var reader = new StreamReader(stream, Encoding.UTF8, false, 8192, true)) {
        return reader.ReadToEnd();
      }
    }

    private static void StringToStream(string s, Stream stream) {
      using (var writer = new StreamWriter(stream, Encoding.UTF8, 8192, true)) {
        writer.Write(s);
        writer.Flush();
      }
    }

    [TestInitialize]
    public void TestInitialize() {
      mSerializer = new PacketSerializer<Packet>();
    }

    private PacketSerializer<Packet> mSerializer;
  }
}