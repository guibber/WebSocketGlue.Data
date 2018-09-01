using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebSocketGlue.Data {
  [Serializable]
  public class EqualsOverride {
    public override bool Equals(object obj) {
      return obj != null && GetType() == obj.GetType();
    }

    public override int GetHashCode() {
      return 0;
    }

    public static bool ObjectEquals(object obj1, object obj2) {
      if (obj1 is null && obj2 is null)
        return true;
      if (obj1 is null || obj2 is null)
        return false;
      if (IsAnonymousObject(obj1) && IsAnonymousObject(obj2))
        return AnonymousEquals(obj1, obj2);
      if (IsObject(obj1) && IsObject(obj2))
        return true;
      return obj1.GetType().IsArray ? ArrayEquals(obj1, obj2) : Equals(obj1, obj2);
    }

    protected static bool ArrayEquals(object obj1, object obj2) {
      return ((IStructuralEquatable)obj1).Equals((IStructuralEquatable)obj2, StructuralComparisons.StructuralEqualityComparer);
    }

    protected static bool AnonymousEquals(object obj1, object obj2) {
      var d1 = ToDictionary(obj1);
      var d2 = ToDictionary(obj2);
      if (d1.Any(p => !Equals(d2[p.Key], p.Value)))
        return false;
      return d1.Count() == d2.Count();
    }

    private static Dictionary<string, object> ToDictionary(object obj) {
      return obj.GetType().GetRuntimeProperties().ToDictionary(p => p.Name, p => p.GetValue(obj));
    }

    private static bool IsAnonymousObject(object obj) {
      return obj.GetType().ToString().StartsWith("<>") && obj.GetType().ToString().Contains("AnonymousType");
    }

    private static bool IsObject(object obj) {
      return obj.GetType() == typeof(object);
    }
  }
}