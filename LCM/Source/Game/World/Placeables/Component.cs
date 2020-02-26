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
        public readonly Dictionary<string, Func<Tile, Connector>> Inputs;
        public readonly Dictionary<string, Func<Tile, Connector>> Outputs;

        public Component(int id, Size size, string name) {
            this.Size = size;
            this.Name = name;
            this.Id = id;
            this.Inputs = new Dictionary<string, Func<Tile, Connector>>();
            this.Outputs = new Dictionary<string, Func<Tile, Connector>>();
            Components.ComponentList.Insert(this.Id, this);
        }

        public virtual bool CanOperate(Tile tile) {
            foreach (Connector connector in tile.Inputs.Values) {
                if (connector == LogicState.Undefined) {
                    return false;
                }
            }

            return true;
        }

        public abstract void Operate(Tile tile);

        public abstract TextureRegion GetTexture();
    }
}