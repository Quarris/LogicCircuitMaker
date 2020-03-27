using Microsoft.Xna.Framework;
using MLEM.Input;

namespace LCM.Game {
    public class DraggingContext {
        public MouseButton Button { get; private set; }
        public bool IsActive { get; private set; }
        public IInteractable Item { get; private set; }
        public Vector2 StartPosition { get; private set; }

        public void Activate(InteractionManager manager, MouseButton button, Vector2 position, IInteractable item) {
            this.Button = button;
            this.IsActive = true;
            this.Item = item;
            this.StartPosition = position;

            if (this.Item != null && this.Item.CanInteract(manager, position, InteractType.StartDrag)) {
                this.Item.Interact(manager, position, InteractType.StartDrag);
            }
        }

        public void Deactivate(InteractionManager manager, Vector2 position) {
            if (!this.IsActive) {
                return;
            }
            this.IsActive = false;
            if (this.Item != null && this.Item.CanInteract(manager, position, InteractType.EndDrag)) {
                this.Item.Interact(manager, position, InteractType.EndDrag);
            }
        }
    }
}