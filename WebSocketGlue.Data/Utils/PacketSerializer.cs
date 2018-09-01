using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace WebSocketGlue.Data.Utils {
  public class PacketSerializer : IPacketSerializer {
    public T Deserialize<T>(MemoryStream stream) {
      if (stream.Length == 0)
        return default(T);
      stream.Seek(0, SeekOrigin.Begin);
      using (var reader = new StreamReader(stream, Encoding.UTF8, false, 8192, true))
      using (var jsonReader = new JsonTextReader(reader)) {
        return BuildSerializerWithSafeConverter().Deserialize<T>(jsonReader);
      }
    }

    public T Deserialize<T>(string packet) {
      return JsonConvert.DeserializeObject<T>(packet, Settings._JsonSerializerSettings);
    }

    public void Serialize<T>(T packet, MemoryStream stream) {
      stream.Seek(0, SeekOrigin.Begin);
      using (var writer = new StreamWriter(stream, Encoding.UTF8, 8192, true))
      using (var jsonWriter = new JsonTextWriter(writer)) {
        BuildSerializer().Serialize(jsonWriter, packet, typeof(T));
        jsonWriter.Flush();
      }

      stream.Seek(0, SeekOrigin.Begin);
    }

    public string Serialize<T>(T packet) {
      return JsonConvert.SerializeObject(packet, packet.GetType(), Settings._JsonSerializerSettings);
    }

    private static JsonSerializer BuildSerializer() {
      return new JsonSerializer {
                                  TypeNameAssemblyFormatHandling = Settings._JsonSerializerSettings.TypeNameAssemblyFormatHandling,
                                  TypeNameHandling = Settings._JsonSerializerSettings.TypeNameHandling
                                };
    }

    private static JsonSerializer BuildSerializerWithSafeConverter() {
      var s = BuildSerializer();
      s.Converters.Add(Settings._Converter);
      return s;
    }
  }
}