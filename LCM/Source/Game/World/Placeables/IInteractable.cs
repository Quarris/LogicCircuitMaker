using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public interface IInteractable {

        RectangleF InteractableArea { get; }

        void DrawOutline(SpriteBatch sb, GameTime gameTime);

        void Interact(InteractionManager manager, InteractType type);
    }

    public enum InteractType {
        LClickUp,
        LClickDown,
        RClickUp,
        RClickDown,
        MClickUp,
        MClickDown
    }
}