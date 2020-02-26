using System.Collections.Generic;
using System.IO;

namespace LCM.Game {
    public static class Components {

        private static readonly string ComponentDir = Path.Combine(LCMGame.Inst.Content.RootDirectory, "Components");
        public static readonly List<Component> ComponentList = new List<Component>();

        public static Component NotGate { get; private set; }
        public static Component AndGate { get; private set; }
        public static Component OrGate { get; private set; }
        public static Component Input { get; private set; }
        public static Component Output { get; private set; }
        public static Component Splitter { get; private set; }
        public static Component XorGate { get; private set; }

        public static void LoadComponents() {
            NotGate = new NotGate();
            AndGate = new AndGate();
            OrGate = new OrGate();
            Input = new Input();
            Output = new Output();
            XorGate = new XorGate();
            Splitter = new Splitter();
        }
    }
}