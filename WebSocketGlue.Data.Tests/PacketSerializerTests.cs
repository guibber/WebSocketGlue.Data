using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        var actual = mSerializer.Deserialize(stream);
        Assert.AreEqual(actual, obj);
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
        var actual = mSerializer.Deserialize(stream);
        Assert.AreEqual(actual, obj);
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationWithAnonDataObject() {
      var obj = One<Request>();
      obj.Data = new {Prop1 = 11};
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        var actual = mSerializer.Deserialize(stream);
        Assert.AreEqual(actual, obj);
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationWithDataObject() {
      var obj = One<Request>();
      obj.Data = new object();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        var actual = mSerializer.Deserialize(stream);
        Assert.AreEqual(actual, obj);
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
        var actual = mSerializer.Deserialize(stream);
        Assert.AreEqual(actual, obj);
      }
    }

    [TestMethod]
    public void TestSerializationAndDeserializationWithDataArray() {
      var obj = One<Request>();
      obj.Data = Many<Ack>();
      using (var stream = new MemoryStream()) {
        mSerializer.Serialize(obj, stream);
        var actual = mSerializer.Deserialize(stream);
        Assert.AreEqual(actual, obj);
      }
    }

    //[TestMethod]
    //public void TestSerializationAndDeserializationOfDerivedType() {
    //  var obj = One<Request>();
    //  using (var stream = new MemoryStream()) {
    //    mSerializer.Serialize(obj, stream);
    //    var actual = mSerializer.Deserialize(stream);
    //    Assert.AreEqual(actual, obj);
    //  }
    //}

    [TestInitialize]
    public void TestInitialize() {
      mSerializer = new PacketSerializer<Packet>();
    }

    private PacketSerializer<Packet> mSerializer;
  }
}