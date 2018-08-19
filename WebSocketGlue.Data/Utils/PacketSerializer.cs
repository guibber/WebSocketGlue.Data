using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace WebSocketGlue.Data.Utils {
  public class PacketSerializer<T> : IPacketSerializer<T> {
    public T Deserialize(MemoryStream stream) {
      if (stream.Length == 0)
        return default(T);
      stream.Seek(0, SeekOrigin.Begin);
      using (var reader = new StreamReader(stream, Encoding.UTF8, false, 8192, true))
      using (var jsonReader = new JsonTextReader(reader)) {
        return BuildSerializerWithSafeConverter().Deserialize<T>(jsonReader);
      }
    }

    public void Serialize(T packet, MemoryStream stream) {
      stream.Seek(0, SeekOrigin.Begin);
      using (var writer = new StreamWriter(stream, Encoding.UTF8, 8192, true))
      using (var jsonWriter = new JsonTextWriter(writer)) {
        BuildSerializer().Serialize(jsonWriter, packet, typeof(T));
        jsonWriter.Flush();
      }
      stream.Seek(0, SeekOrigin.Begin);
    }

    private static JsonSerializer BuildSerializer() {
      return new JsonSerializer {
                                  TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                                  TypeNameHandling = TypeNameHandling.All
                                };
    }

    private static JsonSerializer BuildSerializerWithSafeConverter() {
      var s = BuildSerializer();
      s.Converters.Add(_Converter);
      return s;
    }

    private static readonly SafeUnknownTypeConverter _Converter = new SafeUnknownTypeConverter();
  }
}