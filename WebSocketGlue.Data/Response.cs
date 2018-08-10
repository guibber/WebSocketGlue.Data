namespace WebSocketGlue.Data {
  public class Response : Packet {
    public string RequestId {get;set;}
    public bool IsSuccessful {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (Response)obj;
      return Equals(RequestId, o.RequestId) &&
             Equals(IsSuccessful, o.IsSuccessful);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}