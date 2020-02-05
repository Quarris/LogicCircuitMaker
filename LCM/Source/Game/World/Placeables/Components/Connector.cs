using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    public class Connector : IInteractable {
        public int Layer { get; }
        public Vector2 Position { get; }
        public Direction2 Direction;
        public WirePoint Connection;

        public RectangleF InteractableArea { get; }

        public Vector2 DrawPos { get; }

        public Connector(Point tilePos, Vector2 position, Direction2 direction) {
            this.Position = position;
            this.Direction = direction;
            this.DrawPos = (tilePos.ToVector2() + position) * Constants.PixelsPerUnit;
            Vector2 size = new Vector2(1 / 6f);
            this.InteractableArea = new RectangleF(tilePos.ToVector2() + position - size / 2, size);
            this.Layer = 10;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.DrawPos, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f), 10,
                Color.Aqua, 10);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.DrawPos, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f + 1.5f), 10,
                Color.Black, 3);
        }

        public void Interact(InteractionManager manager, InteractType type) {
            switch (type) {
                case InteractType.LClickDown:
                    manager.IsSelecting = true;
                    manager.SelectedConnector = this;
                    manager.ClickedPosition = this.InteractableArea.Center;
                    break;
                case InteractType.LClickUp:
                    if (manager.IsSelecting) {
                        manager.IsSelecting = false;

                        Vector2 start = manager.ClickedPosition;
                        Vector2 end = this.InteractableArea.Center;

                        if (start.EqualsWithTolerence(end)) {
                            break;
                        }

                        (WirePoint wireStart, WirePoint wireEnd) = LevelManager.CreateWire(start, end);

                        manager.SelectedConnector.Connection = wireStart;
                        this.Connection = wireEnd;
                    }
                    break;
            }
        }
    }
}