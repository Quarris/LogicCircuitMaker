using System;
using System.Collections.Generic;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Cameras;
using MLEM.Extended.Extensions;
using MLEM.Extensions;
using MLEM.Startup;
using MonoGame.Extended;
using MonoGame.Extended.Collections;

namespace LCM.Game {
    public class Level {
        public readonly ObservableCollection<Tile> Tiles;
        public readonly ISet<IInteractable> Interactables;
        public readonly ObservableCollection<Wire> Wires;

        public Level() {
            this.Tiles = new ObservableCollection<Tile>();
            this.Interactables = new HashSet<IInteractable>();
            this.Wires = new ObservableCollection<Wire>();

            this.Tiles.ItemAdded += (sender, args) => {
                Tile tile = args.Item;
                this.Interactables.Add(tile);
                foreach (Connector connector in tile.Connectors.Values) {
                    this.Interactables.Add(connector);
                }
            };
            this.Tiles.ItemRemoved += (sender, args) => {
                Tile tile = args.Item;
                this.Interactables.Remove(tile);
                foreach (Connector connector in tile.Connectors.Values) {
                    this.Interactables.Remove(connector);
                }
            };

            this.Wires.ItemAdded += (sender, args) => {
                Wire wire = args.Item;
                foreach (WireSegment segment in wire.Segments) {
                    this.Interactables.Add(segment);
                    this.Interactables.Add(segment.Point1);
                    this.Interactables.Add(segment.Point2);
                }
            };

            this.Wires.ItemRemoved += (sender, args) => {
                Wire wire = args.Item;
                foreach (WireSegment segment in wire.Segments) {
                    this.Interactables.Remove(segment);
                    this.Interactables.Remove(segment.Point1);
                    this.Interactables.Remove(segment.Point2);
                }
            };
        }

        public void Update(GameTime gameTime) {
            GameState state = LCMGame.Inst.GameState;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            Camera camera = GameState.Get.Camera;

            // Render the grid
            RectangleF frustum = camera.GetVisibleRectangle().ToExtended();
            int minX = (frustum.Left / Constants.PixelsPerUnit).Floor();
            int minY = (frustum.Top / Constants.PixelsPerUnit).Floor();
            int maxX = (frustum.Right / Constants.PixelsPerUnit).Ceil();
            int maxY = (frustum.Bottom / Constants.PixelsPerUnit).Ceil();
            for (int x = minX; x < maxX; x++) {
                int thickness = x == 0 ? 16 : x % 10 == 0 ? 8 : 4;
                sb.DrawLine(x * Constants.PixelsPerUnit, minY * Constants.PixelsPerUnit, x * Constants.PixelsPerUnit, maxY * Constants.PixelsPerUnit, Constants.LineColor, thickness);
            }
            for (int y = minY; y < maxY; y++) {
                int thickness = y == 0 ? 16 : y % 10 == 0 ? 8 : 4;
                sb.DrawLine(minX * Constants.PixelsPerUnit, y * Constants.PixelsPerUnit, maxX * Constants.PixelsPerUnit, y * Constants.PixelsPerUnit, Constants.LineColor, thickness);
            }

            // Render Wires
            foreach (Wire wire in this.Wires) {
                wire.Draw(sb, gameTime);
            }

            // Render tiles
            foreach (Tile tile in this.Tiles) {
                tile.Draw(sb, gameTime);
            }
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

        public void AddWire(Wire wire) {
            this.Wires.Add(wire);
        }

        public void RemoveWire(Wire wire) {
            this.Wires.Remove(wire);
        }
    }
}