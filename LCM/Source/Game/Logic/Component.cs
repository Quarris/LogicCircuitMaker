using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class Component {
        public string Name;
        public Size Size;
        public Dictionary<string, InputTemplate> Inputs;
        public Dictionary<string, OutputTemplate> Outputs;
        public Texture2D Texture;
    }

    public class InputTemplate {
        public Vector2 Position;
        public Direction2 Direction;
        public bool Optional = false;
        public float Length = 0.3f;
    }

    public class OutputTemplate : InputTemplate {
        public Token Function;
    }
}