using Microsoft.Xna.Framework;
using MLEM.Misc;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class OrGate : Component {
        public OrGate() : base(2, new Size(1, 1), "OR Gate") {
            this.Inputs["A"] = pos => new Connector(pos, new Vector2(-0.5f, 0.3f), Direction2.Right);
            this.Inputs["B"] = pos => new Connector(pos, new Vector2(-0.5f, 0.7f), Direction2.Right);
            this.Outputs["Q"] = pos => new Connector(pos, new Vector2(1.5f, 0.5f), Direction2.Left);
        }

        public override TextureRegion GetTexture() {
            return LCMGame.Inst.TextureMap[2, 0, 1, 1];
        }
    }
}