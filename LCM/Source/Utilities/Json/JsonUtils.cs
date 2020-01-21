
namespace LCM.Utilities.Json {
    public static class JsonUtils {
        public static readonly Newtonsoft.Json.JsonSerializer Serializer = new Newtonsoft.Json.JsonSerializer();

        static JsonUtils() {
            Serializer.Converters.Add(new SizeJsonConverter());
        }
    }
}