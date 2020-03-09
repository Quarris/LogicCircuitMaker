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

            foreach (KeyValuePair<string, InputTemplate> tuple in component.Inputs) {
                InputTemplate template = tuple.Value;
                this.Inputs.Add(tuple.Key, new Input(tuple.Key, this, template.Position, template.Direction, template.Optional, template.Length));
            }

            foreach (KeyValuePair<string, OutputTemplate> tuple in component.Outputs) {
                OutputTemplate template = tuple.Value;
                this.Outputs.Add(tuple.Key, new Output(tuple.Key, this, template.Position, template.Direction, template.Optional, template.Length, Compiler.Compile(component.Inputs.Keys, template.Function)));
            }
        }

        protected override Texture2D GetTexture() {
            return this.Component.Texture;
        }

        protected override SavedTile SaveInternal() {
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