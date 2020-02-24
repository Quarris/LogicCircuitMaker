using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public abstract class Component {
        public Size Size;
        public readonly string Name;
        public readonly int Id;
        public readonly Dictionary<string, Func<Point, Connector>> Inputs;
        public readonly Dictionary<string, Func<Point, Connector>> Outputs;

        public Component(int id, Size size, string name) {
            this.Size = size;
            this.Name = name;
            this.Id = id;
            this.Inputs = new Dictionary<string, Func<Point, Connector>>();
            this.Outputs = new Dictionary<string, Func<Point, Connector>>();
            Components.ComponentList.Insert(this.Id, this);
        }



        public abstract TextureRegion GetTexture();
    }
}