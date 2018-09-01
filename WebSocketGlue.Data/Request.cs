using System.Runtime.Serialization;

namespace WebSocketGlue.Data {
  [DataContract(Namespace = "http://data.websocketglue.guibber.com/")]
  public class Request : Packet {
    [DataMember]
    public string RequestId {get;set;}
    [DataMember]
    public string Method {get;set;}
    [DataMember]
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