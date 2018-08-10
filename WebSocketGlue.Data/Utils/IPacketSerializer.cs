using System.IO;

namespace WebSocketGlue.Data.Utils {
  public interface IPacketSerializer<T> {
    T Deserialize(MemoryStream stream);
    void Serialize(T packet, MemoryStream stream);
  }
}