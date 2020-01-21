using System;
using System.Collections.Generic;
using System.IO;
using LCM.Utilities.Json;

namespace LCM.Game {
    public static class Components {

        private static readonly string ComponentDir = Path.Combine(LCMGame.Inst.Content.RootDirectory, "Components");
        public static readonly List<Component> ComponentList = new List<Component>();

        public static void LoadComponents() {
            ComponentList.Clear();

            DirectoryInfo dir = new DirectoryInfo(ComponentDir);
            Console.WriteLine($"Loading {dir.GetFiles().Length} components");
            foreach (FileInfo file in dir.GetFiles("*.json")) {
                using (var reader = file.OpenText()) {
                    Component component = JsonUtils.Serializer.Deserialize<Component>(new Newtonsoft.Json.JsonTextReader(reader));
                    ComponentList.Add(component);
                    Console.WriteLine($"Adding component from {file.Name}");
                }
            }

            Console.WriteLine($"Loaded {ComponentList.Count} components");

        }
    }
}