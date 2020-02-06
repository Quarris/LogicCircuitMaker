using Microsoft.Xna.Framework;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class OrGate : Component {
        public OrGate() : base(2, new Size(3, 2), "OR Gate") {
            this.Inputs["A"] = pos => new Connector(pos, new Vector2(-0.5f, 0.5f), Direction2.Right);
            this.Inputs["B"] = pos => new Connector(pos, new Vector2(-0.5f, 1.5f), Direction2.Right);
            this.Outputs["Q"] = pos => new Connector(pos, new Vector2(3.5f, 1.0f), Direction2.Left);
        }
    }
}