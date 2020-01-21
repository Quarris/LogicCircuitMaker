using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class Component {
        public Size Size;
        public string Name;
        public Dictionary<string, Connector> Inputs;
        public Dictionary<string, Connector> Outputs;
    }

    public class Connector {
        public Vector2 Position;
        public Direction2 Direction;
    }
}