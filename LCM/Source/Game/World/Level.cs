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
        public readonly ISet<IInteractable> Interactables;
        public readonly ObservableCollection<WireSegment> Wires;

        public Level() {
            this.Tiles = new ObservableCollection<Tile>();
            this.Interactables = new HashSet<IInteractable>();
            this.Wires = new ObservableCollection<WireSegment>();

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
                WireSegment wireSegment = args.Item;
                this.Interactables.Add(wireSegment);
                this.Interactables.Add(wireSegment.Point1);
                this.Interactables.Add(wireSegment.Point2);
            };

            this.Wires.ItemRemoved += (sender, args) => {
                WireSegment wireSegment = args.Item;
                this.Interactables.Remove(wireSegment);
            };
        }

        public void Update(GameTime gameTime) {
            GameState state = LCMGame.Inst.GameState;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            foreach (Tile tile in this.Tiles) {
                tile.Draw(sb, gameTime);
            }

            foreach (WireSegment wire in this.Wires) {
                wire.Draw(sb, gameTime);
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

        public void AddWire(WirePoint p1, WirePoint p2) {
            this.Wires.Add(new WireSegment(p1, p2));
        }

        public void RemoveWire(WireSegment wireSegment) {
            this.Wires.Remove(wireSegment);
        }
    }
}