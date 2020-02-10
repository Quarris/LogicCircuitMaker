using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LCM.Game {
    public class LevelManager {

        public static Level Level => LCMGame.Inst.GameState.Level;

        public static bool TryAddTile(Point position, Component component) {
            return Level.TryAddTile(position, component);
        }

        public static void RemoveTile(Point point) {
            Level.RemoveTile(point);
        }

        public static (WirePoint, WirePoint) CreateWire(Vector2 start, Vector2 end, Connector startConnector, Connector endConnector) {
            WirePoint wireStart = new WirePoint(start, startConnector);

            WirePoint mid1 = new WirePoint(new Vector2(start.X + (end.X - start.X)/2f, start.Y));
            WirePoint mid2 = new WirePoint(new Vector2(start.X + (end.X - start.X)/2f, end.Y));

            WirePoint wireEnd = new WirePoint(end, endConnector);

            Level.AddWire(wireStart, mid1);
            Level.AddWire(mid1, mid2);
            Level.AddWire(mid2, wireEnd);

            return (wireStart, wireEnd);
            /*
                Vector2 mid1 = new Vector2(this.ClickedPosition.X + (this.MouseTilePosition.X-this.ClickedPosition.X)/2, this.ClickedPosition.Y) * Constants.PixelsPerUnit;
                Vector2 mid2 = new Vector2(this.ClickedPosition.X + (this.MouseTilePosition.X-this.ClickedPosition.X)/2, this.MouseTilePosition.Y) * Constants.PixelsPerUnit;
                sb.DrawLine(this.ClickedPosition * Constants.PixelsPerUnit, mid1, Color.Black, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid1, mid2, Color.Black, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid2, this.MouseTilePosition * Constants.PixelsPerUnit, Color.Black, Constants.PixelsPerUnit/16f);

             */
        }

        public static void RemoveWire(Wire wire) {
            Level.RemoveWire(wire);
        }
    }
}