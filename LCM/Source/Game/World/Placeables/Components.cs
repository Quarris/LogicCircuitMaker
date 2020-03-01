using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCM.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Startup;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LCM.Game {
    public static class Components {

        private static readonly string ComponentDir = Path.Combine(LCMGame.Inst.Content.RootDirectory, "Components");
        public static readonly Dictionary<string, LogicTemplate> ComponentList = new Dictionary<string, LogicTemplate>();

        public static void LoadComponents(ContentManager content) {
            DirectoryInfo directory = new DirectoryInfo(ComponentDir);
            foreach (FileInfo file in directory.EnumerateFiles()) {
                LogicTemplate template = content.LoadJson<LogicTemplate>("Components/" + file.Name, string.Empty);
                Console.WriteLine(template.Inputs.Values.ToArray()[0].Position);
                template.Texture = MlemGame.LoadContent<Texture2D>("Textures/Components/" + file.Name.Replace(".json", ""));
                ComponentList.Add(file.Name, template);
            }
        }
    }
}