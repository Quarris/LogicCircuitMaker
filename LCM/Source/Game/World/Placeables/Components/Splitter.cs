using Microsoft.Xna.Framework;
using MLEM.Misc;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class Splitter : Component {

        public Splitter() : base(6, new Size(1, 1), "Splitter") {
            this.Inputs.Add("In", tile => new Connector(tile, new Vector2(-0.5f, 0.5f), Direction2.Right));
            this.Outputs.Add("Out1", tile => new Connector(tile, new Vector2(0.5f, -0.5f), Direction2.Down));
            this.Outputs.Add("Out2", tile => new Connector(tile, new Vector2(1.5f, 0.5f), Direction2.Left));
            this.Outputs.Add("Out3", tile => new Connector(tile, new Vector2(0.5f, 1.5f), Direction2.Up));
        }

        public override void Operate(Tile tile) {
            foreach (Connector output in tile.Outputs.Values) {
                output.LogicState = tile.Inputs["In"].LogicState;
            }
        }

        public override TextureRegion GetTexture(UniformTextureAtlas atlas) {
            return atlas[2, 1];
        }
    }
}