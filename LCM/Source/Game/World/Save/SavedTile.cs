using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LCM.Game {
    public abstract class SavedTile {
        public Point Position;
        public readonly Dictionary<string, bool> ConnectorActiveStates = new Dictionary<string, bool>();

        public Tile Load() {
            Tile tile = this.LoadInternal();
            foreach (KeyValuePair<string, bool> state in this.ConnectorActiveStates) {
                tile.Connectors.First(kv => kv.Key == state.Key).Value.IsActive = state.Value;
            }

            return tile;
        }

        protected abstract Tile LoadInternal();
    }

    public class SavedComponentTile : SavedTile {
        public string Component;

        protected override Tile LoadInternal() {
            Console.WriteLine($"Loading {this.Component}");
            return new ComponentTile(this.Position, Components.ComponentList[this.Component]);
        }
    }

    public class SavedPinTile : SavedTile {
        public bool IsInput;

        protected override Tile LoadInternal() {
            return new Pin(this.Position, this.IsInput);
        }
    }
}