using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public interface IInteractable {

        int Layer { get; }
        RectangleF InteractableArea { get; }

        void DrawOutline(SpriteBatch sb, GameTime gameTime);

        bool CanInteract();

        void Interact(InteractionManager manager, InteractType type, object[] data = null);
    }

    public enum InteractType {
        LClickRelease,
        LClickPress,
        RClickRelease,
        RClickPress,
        MClickRelease,
        MClickPress,
        LDrag,
        RDrag
    }
}