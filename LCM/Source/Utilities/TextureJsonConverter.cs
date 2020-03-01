using System;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;
using Newtonsoft.Json;

namespace LCM.Utilities {
    public class TextureJsonConverter : JsonConverter<Texture2D> {
        public override void WriteJson(JsonWriter writer, Texture2D value, JsonSerializer serializer) {

        }

        public override Texture2D ReadJson(JsonReader reader, Type objectType, Texture2D existingValue, bool hasExistingValue, JsonSerializer serializer) {
            string path = reader.ReadAsString();
            Console.WriteLine(path);
            return MlemGame.LoadContent<Texture2D>("Textures/" + path);
        }
    }
}