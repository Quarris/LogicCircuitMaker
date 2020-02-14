using Microsoft.Xna.Framework;
using MLEM.Input;

namespace LCM.Game {
    public class DraggingContext {
        public MouseButton Button { get; private set; }
        public bool IsActive { get; private set; }
        public IInteractable Item { get; private set; }
        public Vector2 StartPosition { get; private set; }

        public void Activate(MouseButton button, Vector2 pos, IInteractable item) {
            this.Button = button;
            this.IsActive = true;
            this.Item = item;
            this.StartPosition = pos;
        }

        public void Deactivate() {
            this.IsActive = false;
        }
    }
}