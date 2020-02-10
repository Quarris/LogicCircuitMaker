using Microsoft.Xna.Framework;
using MLEM.Input;

namespace LCM.Game {
    public class DraggingContext {
        public MouseButton Button;
        public bool IsActive { get; private set; }
        public Vector2 StartPosition;

        public void Activate(MouseButton button, Vector2 pos) {
            this.Button = button;
            this.StartPosition = pos;
            this.IsActive = true;
        }

        public void Deactivate() {
            this.IsActive = false;
        }
    }
}