using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Startup;

namespace LCM.Game {
    public static class Components {

        private static readonly string ComponentDir = Path.Combine(LCMGame.Inst.Content.RootDirectory, "Components");
        public static readonly Dictionary<string, Component> ComponentList = new Dictionary<string, Component>();

        public static void LoadComponents(ContentManager content) {
            DirectoryInfo directory = new DirectoryInfo(ComponentDir);
            foreach (FileInfo file in directory.EnumerateFiles()) {
                Component template = content.LoadJson<Component>("Components/" + file.Name, string.Empty);
                template.Texture = MlemGame.LoadContent<Texture2D>("Textures/Components/" + file.Name.Replace(".json", string.Empty));
                ComponentList.Add(file.Name.Replace(".json", string.Empty), template);
            }

            Console.WriteLine($"Loaded { ComponentList.Count } Components.");
        }

        public static Component GetComponentByIndex(int index) {
            if (index < 0 || index > ComponentList.Count) {
                throw new IndexOutOfRangeException($"Component index out of range {index} for range 0-{ComponentList.Count}");
            }

            return ComponentList.Values.ToArray()[index];
        }
    }
}