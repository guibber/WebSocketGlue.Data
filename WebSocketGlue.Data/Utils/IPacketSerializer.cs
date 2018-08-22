using System.IO;

namespace WebSocketGlue.Data.Utils {
  public interface IPacketSerializer<T> {
    T Deserialize(MemoryStream stream);
    T Deserialize(string packet);
    void Serialize(T packet, MemoryStream stream);
    string Serialize(T packet);
  }
}