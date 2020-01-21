using Microsoft.Xna.Framework;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class AndGate : Component {
        public AndGate() : base(1, new Size(2, 2), "AND Gate") {
            this.Inputs["A"] = new Connector(new Vector2(0, 0.5f), Direction2.Left);
            this.Inputs["B"] = new Connector(new Vector2(0, 1.5f), Direction2.Left);
            this.Outputs["Q"] = new Connector(new Vector2(2.0f, 1.0f), Direction2.Right);
        }
    }
}