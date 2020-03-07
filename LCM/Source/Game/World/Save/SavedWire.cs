using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LCM.Game.Save {
    public class SavedWire {
        public List<Vector2> WirePoints;

        public Point StartTile;
        public string StartConnector;

        public Point EndTile;
        public string EndConnector;

        public Wire Load(Level level) {
            Output output = level.Tiles.First(tile => tile.Position == this.StartTile).Outputs[this.StartConnector];
            Input input = level.Tiles.First(tile => tile.Position == this.EndTile).Inputs[this.EndConnector];

            return new Wire(output, input, this.WirePoints);
        }
    }
}