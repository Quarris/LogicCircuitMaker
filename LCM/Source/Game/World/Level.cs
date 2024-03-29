using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Game.Save;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Cameras;
using MLEM.Extended.Extensions;
using MLEM.Extensions;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using Newtonsoft.Json;

namespace LCM.Game {
    public class Level {
        public readonly ObservableCollection<Tile> Tiles = new ObservableCollection<Tile>();
        public readonly ISet<IInteractable> Interactables = new HashSet<IInteractable>();
        public readonly ObservableCollection<Wire> Wires = new ObservableCollection<Wire>();

        public readonly LogicSimulator LogicSimulator;

        public Level() {
            this.LogicSimulator = new LogicSimulator(this);
            this.Tiles.ItemAdded += (sender, args) => {
                Tile tile = args.Item;
                this.Interactables.Add(tile);
                foreach (Connector connector in tile.Connectors.Select(c => c.Value)) {
                    this.Interactables.Add(connector);
                }
            };
            this.Tiles.ItemRemoved += (sender, args) => {
                Tile tile = args.Item;
                this.Interactables.Remove(tile);
                foreach (Connector connector in tile.Connectors.Select(c => c.Value)) {
                    this.Interactables.Remove(connector);
                    connector.OnRemoved();
                }
            };

            this.Wires.ItemAdded += (sender, args) => {
                Wire wire = args.Item;
                this.Interactables.Add(wire);
            };

            this.Wires.ItemRemoved += (sender, args) => {
                Wire wire = args.Item;
                this.Interactables.Remove(wire);
            };
        }

        public void Update(GameTime gameTime) {
            GameState state = LCMGame.Inst.GameState;
            this.LogicSimulator.Update(gameTime);
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

        public bool TryAddTile(Point position, Tile tile) {
            if (!this.IsAreaOccupied(position, tile.Size)) {
                this.Tiles.Add(tile);
                return true;
            }

            Console.WriteLine($"Couldn't add tile {tile} at {position}");
            return false;
        }

        public void RemoveTile(Point point) {
            Tile toRemove = this.GetTileAt(point);
            this.Tiles.Remove(toRemove);
        }

        public Tile GetTileAt(Point point) {
            return this.Tiles.FirstOrDefault(tile => tile.Area.Contains(point));
        }

        public bool IsAreaOccupied(Point position, Size size) {
            Rectangle area = new Rectangle(position, size);
            return this.Tiles.Any(tile => tile.Area.Intersects(area));
        }

        public void AddWire(Wire wire) {
            this.Wires.Add(wire);
        }

        public void RemoveWire(Wire wire) {
            wire.OnRemoved();
            this.Wires.Remove(wire);
        }

        public SavedLevel Save() {
            return new SavedLevel {
                Tiles = this.Tiles.Select(tile => tile.Save()).ToList(),
                Wires = this.Wires.Select(wire => wire.Save()).ToList()
            };
        }
    }
}