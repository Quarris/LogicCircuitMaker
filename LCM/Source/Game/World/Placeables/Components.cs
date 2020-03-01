using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCM.Utilities;
using Microsoft.Xna.Framework.Content;
using MLEM.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LCM.Game {
    public static class Components {

        private static readonly string ComponentDir = Path.Combine(LCMGame.Inst.Content.RootDirectory, "Components");
        public static readonly Dictionary<string, LogicTemplate> ComponentList = new Dictionary<string, LogicTemplate>();

        public static void LoadComponents(ContentManager content) {
            DirectoryInfo directory = new DirectoryInfo(ComponentDir);
            foreach (FileInfo file in directory.EnumerateFiles()) {
                LogicTemplate temp = content.LoadJson<LogicTemplate>("Components/" + file.Name, string.Empty);
                Console.WriteLine(temp.Texture);
                ComponentList.Add(file.Name, temp);
            }
        }
    }
}