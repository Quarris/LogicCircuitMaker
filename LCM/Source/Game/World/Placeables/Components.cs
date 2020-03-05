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
        public static readonly Dictionary<string, LogicTemplate> ComponentList = new Dictionary<string, LogicTemplate>();

        public static void LoadComponents(ContentManager content) {
            DirectoryInfo directory = new DirectoryInfo(ComponentDir);
            foreach (FileInfo file in directory.EnumerateFiles()) {
                LogicTemplate template = content.LoadJson<LogicTemplate>("Components/" + file.Name, string.Empty);
                template.Texture = MlemGame.LoadContent<Texture2D>("Textures/Components/" + file.Name.Replace(".json", ""));
                ComponentList.Add(file.Name, template);
            }

            Console.WriteLine($"Loaded { ComponentList.Count } Components.");
        }

        public static LogicTemplate GetComponentByIndex(int index) {
            if (index < 0 || index > ComponentList.Count) {
                throw new IndexOutOfRangeException($"Component index out of range {index} for range 0-{ComponentList.Count}");
            }

            return ComponentList.Values.ToArray()[index];
        }
    }
}