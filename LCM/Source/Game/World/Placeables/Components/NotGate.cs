using Microsoft.Xna.Framework;
using MLEM.Misc;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class NotGate : Component {
        public NotGate() : base(0, new Size(1, 1), "NOT Gate") {
            this.Inputs["A"] = tile => new Connector(tile, new Vector2(-0.5f, 0.5f), Direction2.Right);
            this.Outputs["Q"] = tile => new Connector(tile, new Vector2(1.5f, 0.5f), Direction2.Left);
        }

        public override void Operate(Tile tile) {
            tile.Outputs["Q"].LogicState = 1 - tile.Inputs["A"].LogicState;
        }

        public override TextureRegion GetTexture() {
            return LCMGame.Inst.TextureMap[0, 0];
        }
    }
}