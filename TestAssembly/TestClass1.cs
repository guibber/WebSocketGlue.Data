namespace TestAssembly {
  public class TestClass1 : EqualsOverride {
    public int Prop1 {get;set;}
    public string Prop2 {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (TestClass1)obj;
      return Equals(Prop1, o.Prop1) &&
             Equals(Prop2, o.Prop2);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}