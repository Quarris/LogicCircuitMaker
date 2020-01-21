using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class Component {
        public Size Size;
        public string Name;
        public readonly int Id;
        public Dictionary<string, Connector> Inputs;
        public Dictionary<string, Connector> Outputs;

        public Component(int id, Size size, string name) {
            this.Size = size;
            this.Name = name;
            this.Id = id;
            this.Inputs = new Dictionary<string, Connector>();
            this.Outputs = new Dictionary<string, Connector>();
            Components.ComponentList.Insert(this.Id, this);
        }
    }

    public class Connector {
        public Vector2 Position;
        public Direction2 Direction;

        public Connector(Vector2 position, Direction2 direction) {
            this.Position = position;
            this.Direction = direction;
        }
    }
}