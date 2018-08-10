namespace WebSocketGlue.Data.Tests {
  public class SampleOverrideEqualsObject : EqualsOverride {
    public string Prop1 {get;set;}
    public object Prop2 {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (SampleOverrideEqualsObject)obj;
      return Equals(Prop1, o.Prop1) &&
             ObjectEquals(Prop2, o.Prop2);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}