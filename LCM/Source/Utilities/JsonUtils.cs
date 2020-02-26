using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace LCM.Utilities {
    public class JsonUtils {
        public static readonly JsonSerializer Serializer = JsonSerializer.Create();

        public static void Init() {
            Serializer.Converters.Add(new Size2JsonConverter());
            Serializer.Converters.Add(new SizeJsonConverter());
        }

    }
}