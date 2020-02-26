using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    public class Tile : IInteractable {
        public int Layer { get; }
        public Point Position { get; }
        public Size Size => this.Component.Size;
        public Rectangle Area => new Rectangle(this.Position, this.Size);

        public readonly Component Component;
        public readonly Dictionary<string, Connector> Inputs = new Dictionary<string, Connector>();
        public readonly Dictionary<string, Connector> Outputs = new Dictionary<string, Connector>();
        public readonly IEnumerable<KeyValuePair<string, Connector>> Connectors;

        public RectangleF InteractableArea { get; }

        public Tile(Point position, Component component) {
            this.Position = position;
            this.Component = component;
            foreach (KeyValuePair<string, Func<Tile, Connector>> pair in component.Inputs) {
                this.Inputs.Add(pair.Key, pair.Value(this));
            }
            foreach (KeyValuePair<string, Func<Tile, Connector>> pair in component.Outputs) {
                this.Outputs.Add(pair.Key, pair.Value(this));
            }
            this.Connectors = this.Inputs.Concat(this.Outputs);

            this.Layer = 0;

            this.InteractableArea = new RectangleF(position, this.Size.ToSize2());
        }

        public bool Operate() {
            if (this.Component.CanOperate(this)) {
                this.Component.Operate(this);
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.Component.GetTexture(LCMGame.Inst.TextureMap), this.Position.ToVector2(), Constants.ComponentColor);
            foreach (KeyValuePair<string, Connector> connector in this.Connectors) {
                sb.TiledDrawLine(connector.Value.Position, connector.Value.Position + connector.Value.Direction.Offset().ToVector2()/2f, Color.Black, 6);
                sb.TiledDrawCircle(connector.Value.Position, 1/12f, 10, Color.Aqua, 10);
            }
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.Component.GetTexture(LCMGame.Inst.TextureMap), this.Position.ToVector2(), Color.Multiply(Constants.ComponentColor, 1.5f));
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