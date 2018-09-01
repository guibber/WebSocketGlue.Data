using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace WebSocketGlue.Data {
  [DataContract(Namespace = "http://data.websocketglue.guibber.com/")]
  [KnownType(typeof(Ack))]
  [KnownType(typeof(AsyncResponse))]
  [KnownType(typeof(Check))]
  [KnownType(typeof(Connect))]
  [KnownType(typeof(Error))]
  [KnownType(typeof(Notify))]
  [KnownType(typeof(Request))]
  [KnownType(typeof(Response))]
  [KnownType(typeof(Welcome))]
  public class Packet : EqualsOverride {
    [DataMember]
    public string ConnectionId {get;set;}
    [DataMember]
    public object Data {get;set;}

    public override bool Equals(object obj) {
      if (!base.Equals(obj))
        return false;

      var o = (Packet)obj;
      return Equals(ConnectionId, o.ConnectionId) &&
             ObjectEquals(Data, o.Data);
    }

    public override int GetHashCode() {
      return base.GetHashCode();
    }
  }
}