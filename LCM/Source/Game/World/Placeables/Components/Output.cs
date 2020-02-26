using Microsoft.Xna.Framework;
using MLEM.Misc;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class Output : Component {
        public Output() : base(4, new Size(1, 1), "Output") {
            this.Inputs.Add("A", tile => new Connector(tile, new Vector2(-0.5f, 0.5f), Direction2.Right));
        }

        public override void Operate(Tile tile) {

        }

        public override TextureRegion GetTexture(UniformTextureAtlas atlas) {
            return atlas[1, 1];
        }
    }
}