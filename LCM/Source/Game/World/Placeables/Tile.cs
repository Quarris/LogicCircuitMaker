using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
    public class Tile : IInteractable {
        public int Layer { get; }
        public Point Position { get; }
        public Size Size => this.Component.Size;
        public Rectangle Area => new Rectangle(this.Position, this.Size);

        public readonly LogicTemplate Component;
        public readonly Dictionary<string, Connector> Inputs = new Dictionary<string, Connector>();
        public readonly Dictionary<string, Output> Outputs = new Dictionary<string, Output>();
        public readonly IEnumerable<KeyValuePair<string, Connector>> Connectors;

        public RectangleF InteractableArea { get; }

        public Tile(Point position, LogicTemplate component) {
            this.Position = position;
            this.Component = component;

            foreach (var tuple in component.Inputs) {
                this.Inputs.Add(tuple.Key, new Connector(this, tuple.Value.Position, tuple.Value.Direction));
            }

            foreach (var tuple in component.Outputs) {
                this.Outputs.Add(tuple.Key, new Output(this, tuple.Value.Position, tuple.Value.Direction, Compiler.Compile(component.Inputs.Keys, tuple.Value.Function)));
            }

            this.Layer = 0;

            this.InteractableArea = new RectangleF(position, this.Size.ToSize2());
        }

        public bool Operate() {
            bool requeue = false;
            foreach (Output output in this.Outputs.Values) {
                output.Operate(this);
                if (output.LogicState == LogicState.Undefined) {
                    requeue = true;
                }
            }

            return requeue;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.Component.Texture, this.Position.ToVector2(), Constants.ComponentColor);
            foreach (KeyValuePair<string, Connector> connector in this.Connectors) {
                sb.TiledDrawLine(connector.Value.Position, connector.Value.Position + connector.Value.Direction.Offset().ToVector2()/2f, Color.Black, 6);
                sb.TiledDrawCircle(connector.Value.Position, 1/12f, 10, Color.Aqua, 10);
            }
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.Component.Texture, this.Position.ToVector2(), Color.Multiply(Constants.ComponentColor, 1.5f));
        }

        public bool CanInteract(InteractType type) {
            return true;
        }

        public void Interact(InteractionManager manager, InteractType type) {
            if (type == InteractType.RClickPress) {
                LevelManager.RemoveTile(this.Position);
            }
        }

        public void Reset() {
            foreach (Connector connector in this.Connectors.Select(kv => kv.Value)) {
                connector.LogicState = LogicState.Undefined;
            }
        }

        public override string ToString() {
            return $"{this.Component.Name}{this.Position}";
        }
    }
}