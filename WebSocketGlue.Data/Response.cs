using System.Runtime.Serialization;

namespace WebSocketGlue.Data {
  [DataContract(Namespace = "http://data.websocketglue.guibber.com/")]
  public class Response : Packet {
    [DataMember]
    public string RequestId {get;set;}
    [DataMember]
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