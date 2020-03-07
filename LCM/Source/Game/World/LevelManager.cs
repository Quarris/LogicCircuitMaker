using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LCM.Game {
    public class LevelManager {
        public static Level Level => LCMGame.Inst.GameState.Level;

        public static bool TryAddTile(Point position, Tile tile) {
            return Level.TryAddTile(position, tile);
        }

        public static void RemoveTile(Point point) {
            Level.RemoveTile(point);
        }

        public static Wire CreateWire(Output startConnector, Input endConnector) {
            Vector2 start = startConnector.InteractableArea.Center;
            Vector2 end = endConnector.InteractableArea.Center;

            Wire wire = new Wire(startConnector, endConnector, Helper.GetWirePointPositions(start, end));

            Level.AddWire(wire);

            return wire;
        }

        public static void RemoveWire(Wire wire) {
            Level.RemoveWire(wire);
        }

        public static void LoadLevel(Level level) {
            GameState.Get.LoadLevel(level);
        }
    }
}