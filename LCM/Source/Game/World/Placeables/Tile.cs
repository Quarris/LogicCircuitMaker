using System;
using System.Collections.Generic;
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
    public abstract class Tile : IInteractable {
        public int Layer { get; }
        public Point Position { get; }
        public Size Size;
        public abstract string Name { get; }
        public Rectangle Area => new Rectangle(this.Position, this.Size);

        public readonly Dictionary<string, Connector> Inputs = new Dictionary<string, Connector>();
        public readonly Dictionary<string, Output> Outputs = new Dictionary<string, Output>();
        public readonly IEnumerable<KeyValuePair<string, Connector>> Connectors;

        public RectangleF InteractableArea { get; }

        public Tile(Point position, Size size) {
            this.Position = position;
            this.Size = size;
            this.Connectors = this.Inputs.Concat(this.Outputs.Select(kv => new KeyValuePair<string, Connector>(kv.Key, kv.Value)));

            this.Layer = 0;
            this.InteractableArea = new RectangleF(position, this.Size.ToSize2());
        }

        public virtual bool Operate() {
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
            sb.TiledDraw(this.GetTexture(), this.Position.ToVector2(), Constants.ComponentColor);
            foreach (KeyValuePair<string, Connector> connector in this.Connectors) {
                sb.TiledDrawLine(connector.Value.Position, connector.Value.Position + connector.Value.Direction.Offset().ToVector2() / 2f, Color.Black, 6);
                sb.TiledDrawCircle(connector.Value.Position, 1 / 12f, 10, connector.Value.LogicState.Color(), 10);
            }
        }

        public virtual void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.GetTexture(), this.Position.ToVector2(), Color.Multiply(Constants.ComponentColor, 1.5f));
        }

        public abstract Texture2D GetTexture();

        public virtual bool CanInteract(InteractType type) {
            return true;
        }

        public virtual void Interact(InteractionManager manager, InteractType type) {
            if (type == InteractType.RClickPress) {
                LevelManager.RemoveTile(this.Position);
            }
        }

        public virtual void Reset() {
            foreach (Connector connector in this.Connectors.Select(kv => kv.Value)) {
                connector.LogicState = LogicState.Undefined;
            }
        }

        public override string ToString() {
            return $"{this.Name}{this.Position}";
        }

        public virtual void DrawName(SpriteBatch sb, SpriteFont font, float scale, Color color) {
            sb.DrawCenteredString(
                font,
                this.Name,
                this.Position.ToVector2() * Constants.PixelsPerUnit + new Vector2(this.Size.Width / 2f, -1) * Constants.PixelsPerUnit,
                scale,
                color
            );
        }
    }
}