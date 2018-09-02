using System.IO;
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
        AE("{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":null,\"Data\":null}", StreamToString(stream));
        AE(obj, mSerializer.Deserialize<Packet>(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializeWithNullData() {
      var obj = One<Packet>();
      obj.ConnectionId = null;
      obj.Data = null;

      var serialized = mSerializer.Serialize(obj);
      AE("{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":null,\"Data\":null}", serialized);
      AE(obj, mSerializer.Deserialize<Packet>(serialized));
    }

    [TestMethod]
    [DataRow("string_data", "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"string_data\"}")]
    [DataRow(11L, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"11\"}")]
    [DataRow(123.45, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"123.45\"}")]
    public void TestStreamSerializationAndDeserializeWithSimpleData(object data, string expected) {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = data.ToString();

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        AE(expected, StreamToString(stream));
        var actual = mSerializer.Deserialize<Packet>(stream);
        AE(obj, actual);
      }
    }

    [TestMethod]
    [DataRow("string_data", "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"string_data\"}")]
    [DataRow(11L, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"11\"}")]
    [DataRow(123.45, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"123.45\"}")]
    public void TestStringSerializationAndDeserializeWithSimpleData(object data, string expected) {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = data.ToString();

      var serialized = mSerializer.Serialize(obj);
      AE(expected, serialized);
      AE(obj, mSerializer.Deserialize<Packet>(serialized));
    }

    [TestMethod]
    [DataRow("string_data", "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"\\\"string_data\\\"\"}")]
    [DataRow(11L, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"11\"}")]
    [DataRow(123.45, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"123.45\"}")]
    public void TestStreamSerializationAndDeserializeWithSimpleSerializedData(object data, string expected) {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = mSerializer.Serialize(data);

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        AE(expected, StreamToString(stream));
        var actual = mSerializer.Deserialize<Packet>(stream);
        AE(obj, actual);
      }
    }

    [TestMethod]
    [DataRow("string_data", "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"\\\"string_data\\\"\"}")]
    [DataRow(11L, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"11\"}")]
    [DataRow(123.45, "{\"$type\":\"WebSocketGlue.Data.Packet, WebSocketGlue.Data\",\"ConnectionId\":\"1234\",\"Data\":\"123.45\"}")]
    public void TestStringSerializationAndDeserializeWithSimpleSerializedData(object data, string expected) {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = mSerializer.Serialize(data);

      var serialized = mSerializer.Serialize(obj);
      AE(expected, serialized);
      AE(obj, mSerializer.Deserialize<Packet>(serialized));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializeWithNonReferencedType() {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = "data";

      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        using (var changedStream = new MemoryStream()) {
          StringToStream(StreamToString(stream).Replace("WebSocketGlue.Data", "NotReferenced"), changedStream);
          var actual = mSerializer.Deserialize<dynamic>(changedStream);
          AE(obj.ConnectionId, actual.ConnectionId.ToString());
          AE(obj.Data, actual.Data.ToString());
        }
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializeWithNonReferencedType() {
      var obj = One<Packet>();
      obj.ConnectionId = "1234";
      obj.Data = "data";

      var actual = mSerializer.Deserialize<dynamic>(mSerializer.Serialize(obj).Replace("WebSocketGlue.Data", "NotReferenced"));
      AE("NotReferenced.Packet, NotReferenced", actual["$type"].ToString());
      AE(obj.ConnectionId, actual.ConnectionId.ToString());
      AE(obj.Data, actual.Data.ToString());
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationWithAnonDataObject() {
      var obj = new {Prop1 = 11};
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        AE("{\"$type\":\"<>f__AnonymousType3`1[[System.Int32, System.Private.CoreLib]], WebSocketGlue.Data.Tests\",\"Prop1\":11}", StreamToString(stream));
        AE(obj, mSerializer.Deserialize<dynamic>(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationWithAnonDataObject() {
      var obj = new {Prop1 = 11};

      var actual = mSerializer.Serialize(obj);
      AE("{\"$type\":\"<>f__AnonymousType3`1[[System.Int32, System.Private.CoreLib]], WebSocketGlue.Data.Tests\",\"Prop1\":11}", actual);
      AE(obj, mSerializer.Deserialize<dynamic>(actual));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationWithIntArray() {
      var obj = new[] {1, 2, 3, 4, 5};
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        AE("{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}", StreamToString(stream));
        CollectionAssert.AreEqual(obj, mSerializer.Deserialize<dynamic>(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationWithIntArray() {
      var obj = new[] {1, 2, 3, 4, 5};
      var actual = mSerializer.Serialize(obj);
      AE("{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}", actual);
      CollectionAssert.AreEqual(obj, mSerializer.Deserialize<dynamic>(actual));
    }

    [TestMethod]
    public void TestStreamSerializationAndDeserializationWithReferencedObjectArray() {
      var obj = Many<Request>();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        CollectionAssert.AreEqual(obj, mSerializer.Deserialize<dynamic>(stream));
      }
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationWithReferencedObjectArray() {
      var obj = Many<Request>();
      var actual = mSerializer.Serialize(obj);
      CollectionAssert.AreEqual(obj, mSerializer.Deserialize<dynamic>(actual));
    }

    [TestMethod]
    public void TestStringDeserializationNotLoadedReferencedAssembly() {
      var obj = One<TestClass1>();
      var serialized = mSerializer.Serialize(obj);
      serialized = serialized.Replace("WebSocketGlue.Data.Tests.TestClass1, WebSocketGlue.Data.Tests", "TestAssembly.TestClass1, TestAssembly");
      var actual = mSerializer.Deserialize<dynamic>(serialized);
      AE("TestAssembly.TestClass1", actual.GetType().ToString());
      AE(obj.Prop1.ToString(), actual.Prop1.ToString());
      AE(obj.Prop2, actual.Prop2.ToString());
    }

    [TestMethod]
    public void TestStringSerializationAndDeserializationNonJson() {
      var obj = "string_data";
      var actual = mSerializer.Serialize(obj);
      AE(obj, mSerializer.Deserialize<dynamic>(actual));
    }

    [TestInitialize]
    public void TestInitialize() {
      mSerializer = new PacketSerializer();
    }

    private PacketSerializer mSerializer;
  }
}