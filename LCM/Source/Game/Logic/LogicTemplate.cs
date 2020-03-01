using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class LogicTemplate {
        public string Name;
        public Size Size;
        public Dictionary<string, (Vector2 Position, Direction2 Direction)> Inputs;
        public Dictionary<string, (Vector2 Position, Direction2 Direction, Token Function)> Outputs;
        public Texture2D Texture;
    }
}