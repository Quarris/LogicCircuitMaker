using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class Tile : IInteractable {
        public Point TilePosition { get; private set; }
        public Size TileSize => this.Component.Size;
        public Rectangle Area => new Rectangle(this.TilePosition, this.TileSize);

        public readonly Component Component;
        public readonly Dictionary<string, Connector> Connectors;

        public Vector2 DrawPos => this.TilePosition.ToVector2() * Constants.PixelsPerUnit;
        public Size2 DrawSize => this.TileSize.ToSize2() * Constants.PixelsPerUnit;
        public RectangleF DrawArea => new RectangleF(this.DrawPos, this.DrawSize);

        public RectangleF HoveredArea { get; }

        public Tile(Point tilePosition, Component component) {
            this.TilePosition = tilePosition;
            this.Component = component;
            this.Connectors = new Dictionary<string, Connector>();
            foreach (KeyValuePair<string, Func<Point, Connector>> pair in component.Inputs.Concat(component.Outputs)) {
                this.Connectors.Add(pair.Key, pair.Value(tilePosition));
            }

            this.HoveredArea = new RectangleF(tilePosition, this.TileSize.ToSize2());
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCenteredString(
                LCMGame.Inst.Font,
                this.Component.Name,
                this.DrawPos + new Vector2(this.TileSize.Width / 2f, -1) * Constants.PixelsPerUnit,
                0.35f,
                Color.Black
            );
            sb.FillRectangle(this.DrawArea, Color.GreenYellow);
            this.DrawConnectors(sb, gameTime);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawRectangle(this.DrawArea, Color.Black, 4);

            //sb.DrawRectangle(this.HoveredPosition.Translate(-0.5f, -0.5f) * Constants.PixelsPerUnit, new Vector2(Constants.PixelsPerUnit), Color.Black, Constants.PixelsPerUnit/16f);
        }

        public void Interact(InteractionManager manager, InteractType type) {
            if (type == InteractType.RClick) {
                LevelManager.RemoveTile(this.TilePosition);
            }
        }

        private void DrawConnectors(SpriteBatch sb, GameTime gameTime) {
            foreach (KeyValuePair<string, Connector> connector in this.Connectors) {
                connector.Value.Draw(sb, gameTime);
            }
        }
    }
}