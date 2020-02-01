using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MonoGame.Extended;
using MonoGame.Extended.Collections;

namespace LCM.Game {
    public class Level {
        public readonly ObservableCollection<Tile> Tiles;
        public readonly List<IInteractable> Hoverables;
        private readonly Robot robot;

        public Level() {
            this.Tiles = new ObservableCollection<Tile>();
            this.Hoverables = new List<IInteractable>();
            this.robot = new Robot(Vector2.Zero);
            this.Tiles.ItemAdded += (sender, args) => {
                Tile tile = args.Item;
                this.Hoverables.Add(tile);
                foreach (Connector connector in tile.Connectors.Values) {
                    this.Hoverables.Add(connector);
                }
            };
            this.Tiles.ItemRemoved += (sender, args) => {
                Tile tile = args.Item;
                this.Hoverables.Remove(tile);
                foreach (Connector connector in tile.Connectors.Values) {
                    this.Hoverables.Remove(connector);
                }
            };
        }

        public void Update(GameTime gameTime) {
            GameState state = LCMGame.Inst.GameState;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            foreach (Tile tile in this.Tiles) {
                tile.Draw(sb, gameTime);
            }

            this.robot.Draw(sb, gameTime);
        }

        public bool TryAddTile(Point position, Component component) {
            if (!this.IsAreaOccupied(position, component.Size)) {
                Tile tile = new Tile(position, component);
                this.Tiles.Add(tile);
                return true;
            }

            return false;
        }

        public void RemoveTile(Point point) {
            Tile toRemove = this.GetTileAt(point);
            this.Tiles.Remove(toRemove);
        }

        public Tile GetTileAt(Point point) {
            foreach (Tile tile in this.Tiles) {
                if (tile.Area.Contains(point)) {
                    return tile;
                }
            }

            return null;
        }

        public bool IsAreaOccupied(Point position, Size size) {
            Rectangle area = new Rectangle(position, size);
            foreach (Tile tile in this.Tiles) {
                if (tile.Area.Intersects(area)) {
                    return true;
                }
            }

            return false;
        }

        public Point ToTilePos(Point worldPos) {
            return worldPos.Divide(Constants.PixelsPerUnit).Multiply(Constants.PixelsPerUnit);
        }
    }
}