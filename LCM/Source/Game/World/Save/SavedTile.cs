using Microsoft.Xna.Framework;

namespace LCM.Game.Save {
    public abstract class SavedTile {
        public Point Position;

        public abstract Tile Load();
    }

    public class SavedComponentTile : SavedTile {
        public string Component;

        public override Tile Load() {
            return new ComponentTile(this.Position, Components.ComponentList[this.Component]);
        }
    }

    public class SavedPinTile : SavedTile {
        public bool IsInput;

        public override Tile Load() {
            return new Pin(this.Position, this.IsInput);
        }
    }
}