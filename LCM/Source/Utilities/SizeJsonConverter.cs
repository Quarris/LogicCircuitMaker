using System;
using MLEM.Formatting;
using MonoGame.Extended;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace LCM.Utilities {
    public class SizeJsonConverter : JsonConverter<Size> {

        public override void WriteJson(JsonWriter writer, Size value, JsonSerializer serializer) {
            writer.WriteValue($"{value.Width} {value.Height}");
        }

        public override Size ReadJson(JsonReader reader, Type objectType, Size existingValue, bool hasExistingValue, JsonSerializer serializer) {
            int[] numArray = reader.ReadAsMultiDimensional<int>();
            if (numArray.Length == 2)
                return new Size(numArray[0], numArray[1]);
            if (numArray.Length == 1)
                return new Size(numArray[0], numArray[1]);
            throw new InvalidOperationException("Invalid size");
        }
    }
}