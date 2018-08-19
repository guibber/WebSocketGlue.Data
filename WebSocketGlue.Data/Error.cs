namespace WebSocketGlue.Data {
  public class Error : Packet {
    public string Message {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (Error)obj;
      return Equals(Message, o.Message);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}