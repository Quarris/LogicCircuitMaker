using System.Collections.Generic;

namespace LCM.Game.Save {
    public class SavedLevel {
        public List<SavedTile> Tiles;
        public List<SavedWire> Wires;

        public Level Load() {
            Level level = new Level();
            foreach (SavedTile tile in this.Tiles) {
                level.TryAddTile(tile.Position, tile.Load());
            }

            foreach (SavedWire wire in this.Wires) {
                level.AddWire(wire.Load(level));
            }

            return level;
        }
    }
}