using System;
using System.Collections.Generic;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MonoGame.Extended;

namespace LCM.Game {
    public class Level {
        private readonly List<Tile> tiles;
        private readonly Robot robot;

        public Level() {
            this.tiles = new List<Tile>();
            this.robot = new Robot(Vector2.Zero);
        }

        public void Update(GameTime gameTime) {
            GameState state = LCMGame.Inst.GameState;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            foreach (Tile tile in this.tiles) {
                tile.Draw(sb, gameTime);
            }

            this.robot.Draw(sb, gameTime);
        }

        public bool TryAddTile(Point position, Component component) {
            if (!this.IsAreaOccupied(position, component.Size)) {
                Tile tile = new Tile(position, component);
                this.tiles.Add(tile);
                return true;
            }

            return false;
        }

        public void RemoveTile(Point point) {
            Tile toRemove = this.GetTileAt(point);
            this.tiles.Remove(toRemove);
        }

        public Tile GetTileAt(Point point) {
            foreach (Tile tile in this.tiles) {
                if (tile.Area.Contains(point)) {
                    return tile;
                }
            }

            return null;
        }

        public bool IsAreaOccupied(Point position, Size size) {
            Rectangle area = new Rectangle(position, size);
            foreach (Tile tile in this.tiles) {
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