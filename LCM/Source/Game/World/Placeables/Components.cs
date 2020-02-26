using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCM.Utilities;
using Newtonsoft.Json;

namespace LCM.Game {
    public static class Components {

        private static readonly string ComponentDir = Path.Combine(LCMGame.Inst.Content.RootDirectory, "Components");
        public static readonly Dictionary<string, Component> ComponentList = new Dictionary<string, Component>();

        public static void LoadComponents() {
            DirectoryInfo directory = new DirectoryInfo(ComponentDir);
            foreach (FileInfo file in directory.EnumerateFiles().Where(file => file.Extension == ".json")) {
                Console.WriteLine(file.Name);
                using (StreamReader reader = File.OpenText(file.FullName)) {
                    Component component = JsonUtils.Serializer.Deserialize<Component>(new JsonTextReader(reader));
                    Console.WriteLine(component);
                }
            }
        }
    }
}