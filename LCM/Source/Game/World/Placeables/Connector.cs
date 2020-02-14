using System;
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
        public CircleF DrawArea { get; }

        public Connector(Point tilePos, Vector2 position, Direction2 direction) {
            this.Position = position;
            this.Direction = direction;
            this.DrawPos = (tilePos.ToVector2() + position) * Constants.PixelsPerUnit;
            Vector2 size = new Vector2(1 / 6f);
            this.DrawArea = new CircleF(this.DrawPos, Constants.PixelsPerUnit * size.X / 2f);
            this.InteractableArea = new RectangleF(tilePos.ToVector2() + position - size, size * 2);
            this.Layer = 10;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(this.DrawArea, 10, Color.Aqua, 10);
            sb.DrawLine(this.DrawPos, this.DrawPos + (this.Direction.Offset().ToVector2()/2f) * Constants.PixelsPerUnit, Color.Black, 4);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(this.DrawArea, 10, Color.Black, 3);
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