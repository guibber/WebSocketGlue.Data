using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebSocketGlue.Data.Utils {
  public class SafeUnknownTypeConverter : JsonConverter {
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return null;
      if (reader.TokenType != JsonToken.StartObject)
        return serializer.Deserialize(reader);
      var obj = JObject.Load(reader);
      if (serializer.TypeNameHandling == TypeNameHandling.None || obj["$type"] == null)
        return obj;
      if (obj["$type"].ToString().StartsWith("System.Object") && obj.Count == 1 || obj.Count == 0)
        return new object();
      var type = Type.GetType(obj["$type"].ToString());
      return type != null ? obj.ToObject(type, serializer) : obj;
    }

    public override bool CanConvert(Type objectType) {
      return objectType == typeof(object);
    }
  }
}