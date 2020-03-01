using System;
using LCM.Game;
using Newtonsoft.Json;

namespace LCM.Utilities {
    public class TokenJsonConverter : JsonConverter<Token> {

        public override void WriteJson(JsonWriter writer, Token value, JsonSerializer serializer) {
           writer.WriteValue(value.AsString());
        }

        public override Token ReadJson(JsonReader reader, Type objectType, Token existingValue, bool hasExistingValue, JsonSerializer serializer) {
            Console.WriteLine("Parsing Json Token");
            string str = reader.Value.ToString();
            Console.WriteLine(str);
            return Parser.Parse(str);
        }
    }
}