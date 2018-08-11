using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketGlue.Data.Tests.Support;

namespace WebSocketGlue.Data.Tests {
  [TestClass]
  public class EqualsOverrideTests : BaseTest {
    public static IEnumerable<object[]> GetTestObjectEqualsData() {
      yield return BA<object>("two nulls are equal", null, null, true);
      yield return BA("only one null not equal", null, new object(), false);
      yield return BA<object>("ints equal", 11, 11, true);
      yield return BA<object>("different ints not equal", 11, 111, false);
      yield return BA<object>("string equal", "11", "11", true);
      yield return BA<object>("different strings not equal", "11", "111", false);
      yield return BA<object>("byte arrays equal", new byte[] {0, 8, 16, 32, 64}, new byte[] {0, 8, 16, 32, 64}, true);
      yield return BA<object>("different byte arrays not equal", new byte[] {0, 8, 16, 32, 64}, new byte[] {0, 8, 16, 32, 65}, false);
      yield return BA<object>("int arrays equal", new[] {0, 8, 16, 32, 64}, new[] {0, 8, 16, 32, 64}, true);
      yield return BA<object>("different int arrays not equal", new[] {0, 8, 16, 32, 64}, new[] {0, 8, 16, 32, 65}, false);
      yield return BA<object>("anonymous empty objects equal", new { }, new { }, true);
      yield return BA<object>("anonymous objects equal", new {Prop1 = 1, Prop2 = "2"}, new {Prop1 = 1, Prop2 = "2"}, true);
      yield return BA<object>("anonymous objects not equal", new {Prop1 = 1, Prop2 = "2"}, new {Prop1 = 1, Prop2 = "2", Extra = 1}, false);
      yield return BA("objects equal", new object(), new object(), true);
      yield return BA<object>("different non equalsoverride object arrays not equal", new[] {new object(), new object(), new object()}, new[] {new object(), new object(), new object()}, false);
      yield return BA<object>("different non equalsoverride object arrays not equal",
                              BA(new SampleNonOverrideEqualsObject(),
                                 new SampleNonOverrideEqualsObject(),
                                 new SampleNonOverrideEqualsObject()),
                              BA(new SampleNonOverrideEqualsObject(),
                                 new SampleNonOverrideEqualsObject(),
                                 new SampleNonOverrideEqualsObject()),
                              false);
      yield return BA<object>("equalsoverride object arrays are equal",
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = 1},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = 1},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              true);
      yield return BA<object>("different equalsoverride object arrays are not equal",
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = 3},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = 1},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 3},
                                 new SampleOverrideEqualsObject()),
                              false);
      yield return BA<object>("equalsoverride object arrays with nested equalsoverride objects are equal",
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = new SampleOverrideEqualsObject {Prop1 = "12", Prop2 = null}},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = new SampleOverrideEqualsObject {Prop1 = "12", Prop2 = null}},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              true);
      yield return BA<object>("equalsoverride object arrays with different nested objects are not equal",
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = new SampleOverrideEqualsObject {Prop1 = "13", Prop2 = 6}},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              BA(new SampleOverrideEqualsObject {Prop1 = "1", Prop2 = new SampleOverrideEqualsObject {Prop1 = "13", Prop2 = 5}},
                                 new SampleOverrideEqualsObject {Prop1 = "2", Prop2 = 2},
                                 new SampleOverrideEqualsObject()),
                              false);
    }

    [DataTestMethod]
    [DynamicData(nameof(GetTestObjectEqualsData), DynamicDataSourceType.Method)]
    public void TestObjectEquals(string name, object one, object two, bool expected) {
      var actual = EqualsOverride.ObjectEquals(one, two);
      if (expected)
        Assert.IsTrue(actual, name);
      else
        Assert.IsFalse(actual, name);
    }
  }
}