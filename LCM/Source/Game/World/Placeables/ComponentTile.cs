using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LCM.Extensions;
using LCM.Game.Save;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
    public class ComponentTile : Tile {
        public override string Name => this.Component.Name;
        public readonly Component Component;

        public ComponentTile(Point position, Component component) : base(position, component.Size) {
            this.Component = component;

            foreach (var tuple in component.Inputs) {
                this.Inputs.Add(tuple.Key, new Input(this, tuple.Value.Position, tuple.Value.Direction, tuple.Value.Length));
            }

            foreach (var tuple in component.Outputs) {
                this.Outputs.Add(tuple.Key, new Output(this, tuple.Value.Position, tuple.Value.Direction, tuple.Value.Length, Compiler.Compile(component.Inputs.Keys, tuple.Value.Function)));
            }
        }

        public override Texture2D GetTexture() {
            return this.Component.Texture;
        }

        public override SavedTile Save() {
            return new SavedComponentTile {
                Position = this.Position,
                Component = Components.ComponentList.First(kv => kv.Value.Equals(this.Component)).Key
            };
        }

        public override string ToString() {
            return $"{this.Component.Name}{this.Position}";
        }
    }
}