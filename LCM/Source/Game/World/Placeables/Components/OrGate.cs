using Microsoft.Xna.Framework;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Game {
    public class OrGate : Component {
        public OrGate() : base(2, new Size(3, 2), "OR Gate") {
            this.Inputs["A"] = new Connector(new Vector2(0, 0.5f), Direction2.Left);
            this.Inputs["B"] = new Connector(new Vector2(0, 1.5f), Direction2.Left);
            this.Outputs["Q"] = new Connector(new Vector2(3.0f, 1.0f), Direction2.Right);
        }
    }
}