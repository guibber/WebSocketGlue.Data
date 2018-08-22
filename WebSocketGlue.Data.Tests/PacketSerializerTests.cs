using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketGlue.Data.Tests.Support;
using WebSocketGlue.Data.Utils;

namespace WebSocketGlue.Data.Tests {
  [TestClass]
  public class PacketSerializerTests : BaseTest {
    [TestMethod]
    public void TestStreamSerializationAndDeserializeWithNullData() {
      var obj = One<Packet>();
      obj.ConnectionId = null;
      obj.Data = null;

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":null,\"Data\":null}", StreamToString(stream));
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializeWithNullData() {
      var obj = One<Packet>();
      obj.ConnectionId = null;
      obj.Data = null;

      var serialized = mSerializer.Serialize(obj);
      Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":null,\"Data\":null}", serialized);
      Assert.AreEqual(obj, mSerializer.Deserialize(serialized));
    }

    [TestMethod]
    [DataRow("string_data", "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"string_data\"}")]
    [DataRow(11L, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":11}")]
    [DataRow(123.45, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":123.45}")]
    public void TestStreamSerializationAndDeserializeWithSimpleData(object data, string expected) {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = data;

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual(expected, StreamToString(stream));
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    [DataRow("string_data", "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"string_data\"}")]
    [DataRow(11L, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":11}")]
    [DataRow(123.45, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":123.45}")]
    public void TestStringSerializationAndDeserializeWithSimpleData(object data, string expected) {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = data;

      var serialized = mSerializer.Serialize(obj);
      Assert.AreEqual(expected, serialized);
      Assert.AreEqual(obj, mSerializer.Deserialize(serialized));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationWithAnonDataObject() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      obj.Data = new {Prop1 = 11};

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"<>f__AnonymousType3`1[[System.Int32, System.Private.CoreLib]], WebSocketGlue.Data.Tests\",\"Prop1\":11}}", StreamToString(stream));
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationWithAnonDataObject() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      obj.Data = new {Prop1 = 11};

      var serialized = mSerializer.Serialize(obj);
      Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"<>f__AnonymousType3`1[[System.Int32, System.Private.CoreLib]], WebSocketGlue.Data.Tests\",\"Prop1\":11}}", serialized);
      Assert.AreEqual(obj, mSerializer.Deserialize(serialized));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationWithDataObject() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      obj.Data = new object();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"System.Object, System.Private.CoreLib\"}}", StreamToString(stream));
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationWithDataObject() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      obj.Data = new object();

      var serialized = mSerializer.Serialize(obj);
      Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"System.Object, System.Private.CoreLib\"}}", serialized);
      Assert.AreEqual(obj, mSerializer.Deserialize(serialized));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationNestedDataWithDataObject() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      var sub = One<Check>();
      sub.ConnectionId = "1236";
      sub.Data = new object();
      obj.Data = sub;

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"WebSocketGlue.Data.Check, WebSocketGlue.Data\",\"ConnectionId\":\"1236\",\"Data\":{\"$type\":\"System.Object, System.Private.CoreLib\"}}}", StreamToString(stream));
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationNestedDataWithDataObject() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      var sub = One<Check>();
      sub.ConnectionId = "1236";
      sub.Data = new object();
      obj.Data = sub;

      var serialized = mSerializer.Serialize(obj);
      Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"WebSocketGlue.Data.Check, WebSocketGlue.Data\",\"ConnectionId\":\"1236\",\"Data\":{\"$type\":\"System.Object, System.Private.CoreLib\"}}}", serialized);
      Assert.AreEqual(obj, mSerializer.Deserialize(serialized));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationWithDataArray() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      var ary = Many<Ack>();
      ary[0].ConnectionId = "1236";
      ary[0].Data = "my data";
      ary[1].ConnectionId = "1237";
      ary[1].Data = 15L;
      ary[2].ConnectionId = "1238";
      ary[2].Data = null;
      obj.Data = ary;

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"WebSocketGlue.Data.Ack[], WebSocketGlue.Data\",\"$values\":[{\"$type\":\"WebSocketGlue.Data.Ack, WebSocketGlue.Data\",\"ConnectionId\":\"1236\",\"Data\":\"my data\"},{\"$type\":\"WebSocketGlue.Data.Ack, WebSocketGlue.Data\",\"ConnectionId\":\"1237\",\"Data\":15},{\"$type\":\"WebSocketGlue.Data.Ack, WebSocketGlue.Data\",\"ConnectionId\":\"1238\",\"Data\":null}]}}", StreamToString(stream));
        Assert.AreEqual(obj, mSerializer.Deserialize(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationWithDataArray() {
      var obj = One<Request>();
      obj.RequestId = "1235";
      obj.Method = "POST";
      obj.Url = "MyUrl";
      obj.ConnectionId = "1234";
      var ary = Many<Ack>();
      ary[0].ConnectionId = "1236";
      ary[0].Data = "my data";
      ary[1].ConnectionId = "1237";
      ary[1].Data = 15L;
      ary[2].ConnectionId = "1238";
      ary[2].Data = null;
      obj.Data = ary;

      var serialized = mSerializer.Serialize(obj);
      Assert.AreEqual("{\"$type\":\"WebSocketGlue.Data.Request, WebSocketGlue.Data\",\"RequestId\":\"1235\",\"Method\":\"POST\",\"Url\":\"MyUrl\",\"ConnectionId\":\"1234\",\"Data\":{\"$type\":\"WebSocketGlue.Data.Ack[], WebSocketGlue.Data\",\"$values\":[{\"$type\":\"WebSocketGlue.Data.Ack, WebSocketGlue.Data\",\"ConnectionId\":\"1236\",\"Data\":\"my data\"},{\"$type\":\"WebSocketGlue.Data.Ack, WebSocketGlue.Data\",\"ConnectionId\":\"1237\",\"Data\":15},{\"$type\":\"WebSocketGlue.Data.Ack, WebSocketGlue.Data\",\"ConnectionId\":\"1238\",\"Data\":null}]}}", serialized);
      Assert.AreEqual(obj, mSerializer.Deserialize(serialized));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationUnknownObjectType() {
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
    public void TestStringSerializationAndDeserializationUnknownObjectType() {
      var obj = One<Request>();
      obj.Data = One<Packet>();

      var s = mSerializer.Serialize(obj);
      s = s.Replace("WebSocketGlue.Data.Packet, WebSocketGlue.Data", "NotAvailable.Packet, NotAvailable");
      var actual = mSerializer.Deserialize(s);
      dynamic actualData = actual.Data;
      Assert.AreEqual(obj.ConnectionId, actual.ConnectionId);
      Assert.AreEqual(((Packet)obj.Data).ConnectionId, actualData.ConnectionId.ToString());
      Assert.IsNotNull(actualData.Data);
    }

    [TestMethod]
    public void TestStreamDeserializationNotLoadedReferencedAssembly() {
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

    [TestMethod]
    public void TestStringDeserializationNotLoadedReferencedAssembly() {
      var obj = One<Request>();
      obj.Data = One<TestClass1>();
      var s = mSerializer.Serialize(obj);
      s = s.Replace("WebSocketGlue.Data.Tests.TestClass1, WebSocketGlue.Data.Tests", "TestAssembly.TestClass1, TestAssembly");
      var actual = mSerializer.Deserialize(s);
      dynamic actualData = actual.Data;
      Assert.AreEqual(obj.ConnectionId, actual.ConnectionId);
      Assert.AreEqual("TestAssembly.TestClass1", actualData.GetType().ToString());
      Assert.AreEqual(((TestClass1)obj.Data).Prop1.ToString(), actualData.Prop1.ToString());
      Assert.AreEqual(((TestClass1)obj.Data).Prop2, actualData.Prop2.ToString());
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