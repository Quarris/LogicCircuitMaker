using Microsoft.Xna.Framework;
using MLEM.Misc;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class OrGate : Component {
        public OrGate() : base(2, new Size(1, 1), "OR Gate") {
            this.Inputs["A"] = tile => new Connector(tile, new Vector2(-0.5f, 0.3f), Direction2.Right);
            this.Inputs["B"] = tile => new Connector(tile, new Vector2(-0.5f, 0.7f), Direction2.Right);
            this.Outputs["Q"] = tile => new Connector(tile, new Vector2(1.5f, 0.5f), Direction2.Left);
        }

        public override void Operate(Tile tile) {
            tile.Outputs["Q"].LogicState = tile.Inputs["A"].LogicState & tile.Inputs["B"].LogicState;
        }

        public override TextureRegion GetTexture() {
            return LCMGame.Inst.TextureMap[2, 0];
        }
    }
}