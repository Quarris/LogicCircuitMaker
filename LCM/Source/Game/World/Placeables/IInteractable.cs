using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public interface IInteractable {

        int Layer { get; }
        RectangleF InteractableArea { get; }

        void DrawOutline(SpriteBatch sb, GameTime gameTime, Vector2 position);

        // TODO Add the position of the interaction. Allows to more precisely define the shape for non-rectangular gates

        bool CanInteract(InteractionManager manager, Vector2 position, InteractType type = InteractType.Any);

        void Interact(InteractionManager manager, Vector2 position, InteractType type);
    }

    public enum InteractType {
        Any,
        LClickRelease,
        LClickPress,
        RClickRelease,
        RClickPress,
        MClickRelease,
        MClickPress,
        Drag
    }
}