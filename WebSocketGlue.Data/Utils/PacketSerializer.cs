using System.Collections.Generic;
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

    public T Deserialize(string packet) {
      return JsonConvert.DeserializeObject<T>(packet, _Settings);
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

    public string Serialize(T packet) {
      return JsonConvert.SerializeObject(packet, packet.GetType(), _Settings);
    }

    private static JsonSerializer BuildSerializer() {
      return new JsonSerializer {
                                  TypeNameAssemblyFormatHandling = _Settings.TypeNameAssemblyFormatHandling,
                                  TypeNameHandling = _Settings.TypeNameHandling
                                };
    }

    private static JsonSerializer BuildSerializerWithSafeConverter() {
      var s = BuildSerializer();
      s.Converters.Add(_Converter);
      return s;
    }

    private static readonly SafeUnknownTypeConverter _Converter = new SafeUnknownTypeConverter();

    private static readonly JsonSerializerSettings _Settings = new JsonSerializerSettings {
                                                                                            TypeNameHandling = TypeNameHandling.All,
                                                                                            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                                                                                            Converters = new List<JsonConverter> {_Converter}
                                                                                          };
  }
}