using System;
using MLEM.Formatting;
using MonoGame.Extended;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace LCM.Utilities {
    public class SizeJsonConverter : JsonConverter {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            Size size = (Size) value;
            writer.WriteValue(string.Format("{0} {1}", size.Width, size.Height));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            int[] numArray = reader.ReadAsMultiDimensional<int>();
            if (numArray.Length == 2)
                return new Size(numArray[0], numArray[1]);
            if (numArray.Length == 1)
                return new Size(numArray[0], numArray[1]);
            throw new InvalidOperationException("Invalid size");
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(Size);
        }
    }
}