namespace WebSocketGlue.Data {
  public class Request : Packet {
    public string RequestId {get;set;}
    public string Method {get;set;}
    public string Url {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (Request)obj;
      return Equals(RequestId, o.RequestId) &&
             Equals(Method, o.Method) &&
             Equals(Url, o.Url);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}