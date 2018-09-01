using System.Runtime.Serialization;

namespace WebSocketGlue.Data {
  [DataContract(Namespace = "http://data.websocketglue.guibber.com/")]
  public class Error : Packet {
    [DataMember]
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