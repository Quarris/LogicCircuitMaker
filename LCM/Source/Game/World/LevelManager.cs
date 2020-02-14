using System;
using System.Collections.Generic;
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

        public static Wire CreateWire(Connector startConnector, Connector endConnector) {
            Vector2 start = startConnector.InteractableArea.Center;
            Vector2 end = endConnector.InteractableArea.Center;

            List<Vector2> points = new List<Vector2> {
                start,
                new Vector2(start.X + (end.X - start.X) / 2f, start.Y),
                new Vector2(start.X + (end.X - start.X) / 2f, end.Y),
                end
            };

            Wire wire = new Wire(startConnector, endConnector, points);

            Level.AddWire(wire);

            return wire;
        }

        public static void RemoveWire(Wire wire) {
            Level.RemoveWire(wire);
        }
    }
}