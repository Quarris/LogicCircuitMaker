using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LCM.Extensions;
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
        public readonly LogicTemplate Component;

        public ComponentTile(Point position, LogicTemplate component) : base(position, component.Size) {
            this.Component = component;

            foreach (var tuple in component.Inputs) {
                this.Inputs.Add(tuple.Key, new Connector(this, tuple.Value.Position, tuple.Value.Direction));
            }

            foreach (var tuple in component.Outputs) {
                this.Outputs.Add(tuple.Key, new Output(this, tuple.Value.Position, tuple.Value.Direction, Compiler.Compile(component.Inputs.Keys, tuple.Value.Function)));
            }
        }

        public override Texture2D GetTexture() {
            return this.Component.Texture;
        }

        public override string ToString() {
            return $"{this.Component.Name}{this.Position}";
        }
    }
}