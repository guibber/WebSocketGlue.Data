using System.IO;

namespace WebSocketGlue.Data.Utils {
  public interface IPacketSerializer {
    T Deserialize<T>(MemoryStream stream);
    T Deserialize<T>(string packet);
    void Serialize<T>(T packet, MemoryStream stream);
    string Serialize<T>(T packet);
  }
}