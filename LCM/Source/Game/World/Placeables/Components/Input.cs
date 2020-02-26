using Microsoft.Xna.Framework;
using MLEM.Misc;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class Input : Component {
        public Input() : base(3, new Size(1, 1), "Input") {
            this.Outputs.Add("Q", tile => new Connector(tile, new Vector2(1.5f, 0.5f), Direction2.Left));
        }

        public override void Operate(Tile tile) {
            tile.Outputs["Q"].LogicState = LogicState.On;
        }

        public override TextureRegion GetTexture() {
            return LCMGame.Inst.TextureMap[0, 1];
        }
    }
}