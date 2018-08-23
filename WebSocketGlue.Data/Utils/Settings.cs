using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebSocketGlue.Data.Utils {
  public class Settings {
    public static readonly SafeUnknownTypeConverter _Converter = new SafeUnknownTypeConverter();

    public static readonly JsonSerializerSettings _JsonSerializerSettings = new JsonSerializerSettings {
                                                                                                         TypeNameHandling = TypeNameHandling.All,
                                                                                                         TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                                                                                                         Converters = new List<JsonConverter> {_Converter}
                                                                                                       };
  }
}