using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketGlue.Data.Tests.Support;

namespace WebSocketGlue.Data.Tests {
  [TestClass]
  public class PacketTests : BaseTest {
    public static IEnumerable<object[]> GetTestEqualsData() {
      yield return BA<object>("nulls test", new Packet(), new Packet(), true);
      yield return BA<object>("Data string equals", new Packet {ConnectionId = "123", Data = "456"}, new Packet {ConnectionId = "123", Data = "456"}, true);
      yield return BA<object>("ConnectionId not equals", new Packet {ConnectionId = "124", Data = "456"}, new Packet {ConnectionId = "123", Data = "456"}, false);
      yield return BA<object>("Data string not equals", new Packet {ConnectionId = "123", Data = "456"}, new Packet {ConnectionId = "123", Data = "457"}, false);
      yield return BA<object>("Data int not equals", new Packet {ConnectionId = "123", Data = 6}, new Packet {ConnectionId = "123", Data = 6}, false);
      yield return BA<object>("Data int not equals", new Packet {ConnectionId = "123", Data = 6}, new Packet {ConnectionId = "123", Data = 7}, false);
      yield return BA<object>("Data anonymous equals", new Packet {ConnectionId = "123", Data = new {Test1 = 124}}, new Packet {ConnectionId = "123", Data = new {Test1 = 124}}, true);
      yield return BA<object>("Data anonymous not equals", new Packet {ConnectionId = "123", Data = new {Test1 = 124}}, new Packet {ConnectionId = "123", Data = new {Test1 = 125}}, false);
      yield return BA<object>("Data object equals", new Packet {ConnectionId = "123", Data = new object()}, new Packet {ConnectionId = "123", Data = new object()}, true);
      yield return BA<object>("Data array equals", new Packet {ConnectionId = "123", Data = new[] {1, 2, 3}}, new Packet {ConnectionId = "123", Data = new[] {1, 2, 3}}, true);
      yield return BA<object>("Data array not equals", new Packet {ConnectionId = "123", Data = new[] {1, 2, 3}}, new Packet {ConnectionId = "123", Data = new[] {1, 2, 4}}, false);
      yield return BA<object>("Data array with object equals", new Packet {ConnectionId = "123", Data = new object[] {new Packet(), 2, 3}}, new Packet {ConnectionId = "123", Data = new object[] {new Packet(), 2, 3}}, true);
      yield return BA<object>("Data array with object not equals", new Packet {ConnectionId = "123", Data = new object[] {new Packet(), 2, 3}}, new Packet {ConnectionId = "123", Data = new object[] {new Packet {ConnectionId = "1"}, 2, 3}}, false);
    }

    [DataTestMethod]
    [DynamicData(nameof(GetTestEqualsData), DynamicDataSourceType.Method)]
    public void TestEquals(string name, Packet one, Packet two, bool expected) {
      if (expected)
        Assert.AreEqual(one, two, name);
      else
        Assert.AreNotEqual(one, two, name);
    }

    [TestInitialize]
    public void TestInitialize() { }
  }
}