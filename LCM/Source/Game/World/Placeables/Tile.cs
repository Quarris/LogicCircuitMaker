using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class Tile : IInteractable {
        public int Layer { get; }
        public Point Position { get; private set; }
        public Size Size => this.Component.Size;
        public Rectangle Area => new Rectangle(this.Position, this.Size);

        public readonly Component Component;
        public readonly Dictionary<string, Connector> Connectors;

        public RectangleF InteractableArea { get; }

        public Tile(Point position, Component component) {
            this.Position = position;
            this.Component = component;
            this.Connectors = new Dictionary<string, Connector>();
            foreach (KeyValuePair<string, Func<Point, Connector>> pair in component.Inputs.Concat(component.Outputs)) {
                this.Connectors.Add(pair.Key, pair.Value(position));
            }

            this.Layer = 0;

            this.InteractableArea = new RectangleF(position, this.Size.ToSize2());
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.Component.GetTexture(), this.Position.ToVector2(), Constants.ComponentColor);
            this.DrawConnectors(sb, gameTime);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDraw(this.Component.GetTexture(), this.Position.ToVector2(), Color.Multiply(Constants.ComponentColor, 1.5f));
        }

        public bool CanInteract(InteractType type) {
            return true;
        }

        public void Interact(InteractionManager manager, InteractType type) {
            if (type == InteractType.RClickPress) {
                LevelManager.RemoveTile(this.Position);
            }
        }

        private void DrawConnectors(SpriteBatch sb, GameTime gameTime) {
            foreach (KeyValuePair<string, Connector> connector in this.Connectors) {
                connector.Value.Draw(sb, gameTime);
            }
        }
    }
}