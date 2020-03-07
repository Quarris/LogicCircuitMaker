using System;
using System.IO;
using LCM.Game;
using LCM.Game.Save;
using Newtonsoft.Json;

namespace LCM.Utilities {
    public static class FileManager {
        public static readonly DirectoryInfo AppData = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogicCircuitMaker"));
        public static readonly DirectoryInfo LevelDirectory = new DirectoryInfo(Path.Combine(AppData.FullName, "Levels"));
        private static readonly JsonSerializer Serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.Auto};

        private static void CreateAppData() {
            if (!AppData.Exists) {
                Console.WriteLine($"Creating AppData at {AppData}");
                AppData.Create();
            }
        }

        public static bool SaveLevel(Level level, string name, bool force) {
            CreateAppData();
            if (!LevelDirectory.Exists) {
                LevelDirectory.Create();
            }

            FileInfo file = new FileInfo(Path.Combine(LevelDirectory.FullName, name + ".json"));

            if (file.Exists && !force) {
                Console.WriteLine("Could not save level. File already exists");
                return false;
            }

            if (force) Console.WriteLine("Overriding save");
            Console.WriteLine($"Saving level to {name}");
            using (JsonWriter writer = new JsonTextWriter(file.CreateText()))
                Serializer.Serialize(writer, level.Save());
            return true;
        }

        public static bool LoadLevel(string name, out Level level) {
            CreateAppData();
            if (!LevelDirectory.Exists) {
                LevelDirectory.Create();
                level = null;
                return false;
            }

            FileInfo file = new FileInfo(Path.Combine(LevelDirectory.FullName, name));

            if (!file.Exists) {
                Console.WriteLine($"Level file {file.FullName} doesn't exist");
                level = null;
                return false;
            }

            try {
                using (JsonReader reader = new JsonTextReader(file.OpenText()))
                    level = Serializer.Deserialize<SavedLevel>(reader).Load();
            }
            catch (Exception e) {
                Console.WriteLine($"Could not deserialize level json from file {file.FullName}\n {e.StackTrace}");
                level = null;
                return false;
            }

            return true;
        }
    }
}