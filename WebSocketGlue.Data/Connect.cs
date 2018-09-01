using System.Runtime.Serialization;

namespace WebSocketGlue.Data {
  [DataContract(Namespace = "http://data.websocketglue.guibber.com/")]
  public class Connect : Packet {
    [DataMember]
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