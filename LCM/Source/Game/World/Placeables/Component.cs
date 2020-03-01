using System;
using System.Collections.Generic;
using MLEM.Textures;
using MonoGame.Extended;
using Newtonsoft.Json.Serialization;

namespace LCM.Game {
    public class Component {
        public string RegistryName;
        public string DisplayName;
        public Size Size;
        public readonly Dictionary<string, Func<Tile, Connector>> Inputs;
        public readonly Dictionary<string, Func<Tile, Connector>> Outputs;

        public Component(JsonObjectContract json) {

        }

        public virtual bool CanOperate(Tile tile) {
            foreach (Connector connector in tile.Inputs.Values) {
                if (connector == LogicState.Undefined) {
                    return false;
                }
            }

            return true;
        }

        public void Operate(Tile tile) {

        }

        public TextureRegion GetTexture(UniformTextureAtlas atlas) {
            return null;
        }
    }
}