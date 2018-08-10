using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebSocketGlue.Data {
  public class Packet : EqualsOverride {
    public string ConnectionId {get;set;}
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