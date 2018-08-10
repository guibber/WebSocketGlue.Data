namespace WebSocketGlue.Data {
  public class Connect : Packet {
    public string PreviousConnectionId {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (Connect)obj;
      return Equals(PreviousConnectionId, o.PreviousConnectionId);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}