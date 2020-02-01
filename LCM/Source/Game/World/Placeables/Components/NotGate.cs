using Microsoft.Xna.Framework;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class NotGate : Component {
        public NotGate() : base(0, new Size(1, 1), "NOT Gate") {
            this.Inputs["A"] = pos => new Connector(pos, new Vector2(0, 0.5f), Direction2.Left);
            this.Outputs["Q"] = pos => new Connector(pos, new Vector2(1.0f, 0.5f), Direction2.Right);
        }
    }
}