using System;
using LCM.Extensions;
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
        public readonly Direction2 Direction;
        public WirePoint Connection;

        public RectangleF InteractableArea { get; }

        public Connector(Point tilePos, Vector2 position, Direction2 direction) {
            this.Position = tilePos.ToVector2() + position;
            this.Direction = direction;
            Vector2 size = new Vector2(1 / 6f);
            this.InteractableArea = new RectangleF(this.Position - size, size * 2);
            this.Layer = 10;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDrawLine(this.Position, this.Position + this.Direction.Offset().ToVector2()/2f, Color.Black, 6);
            sb.TiledDrawCircle(this.Position, 1/12f, 10, Color.Aqua, 10);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDrawCircle(this.Position, 1/12f, 10, Color.Aqua, 3);
        }

        public bool CanInteract(InteractType type) {
            return this.Connection == null;
        }

        public void Interact(InteractionManager manager, InteractType type) {
            switch (type) {
                case InteractType.LClickPress:
                    manager.IsSelecting = true;
                    manager.SelectedConnector = this;
                    manager.ClickedPosition = this.InteractableArea.Center;
                    break;
                case InteractType.LClickRelease:
                    if (manager.IsSelecting) {
                        manager.IsSelecting = false;

                        Vector2 start = manager.ClickedPosition;
                        Vector2 end = this.InteractableArea.Center;

                        if (start.EqualsWithTolerence(end)) {
                            break;
                        }

                        Wire wire = LevelManager.CreateWire(manager.SelectedConnector, this);


                        manager.SelectedConnector.Connection = wire.Point1;
                        this.Connection = wire.Point2;
                    }
                    break;
            }
        }
        public override string ToString() {
            return this.Position.ToString();
        }
    }
}