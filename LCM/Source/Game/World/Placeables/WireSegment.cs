using System;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extended.Extensions;
using MLEM.Input;
using MonoGame.Extended;
using MlemRect = MLEM.Misc.RectangleF;

namespace LCM.Game {
    public class WireSegment : IInteractable {
        public int Layer { get; }

        public readonly Wire Wire;

        public readonly WirePoint Point1;
        public readonly WirePoint Point2;

        public Axis Axis { get; }

        private RectangleF interactableArea;
        public RectangleF InteractableArea {
            get => this.interactableArea;
            set {
                this.interactableArea = value;
                this.drawRect = Helper.RectFromCorners((Vector2) value.TopLeft * Constants.PixelsPerUnit, (Vector2) value.BottomRight * Constants.PixelsPerUnit);
            }
        }

        private RectangleF drawRect;
        private RectangleF DrawRect => this.drawRect;

        public WireSegment(Wire wire, WirePoint point1, WirePoint point2) {
            this.Wire = wire;
            this.Point1 = point1;
            this.Point2 = point2;
            point1.ConnectedWires.Add(this);
            point2.ConnectedWires.Add(this);

            this.interactableArea = MlemRect.FromCorners(point1.Position - new Vector2(1 / 16f), point2.Position + new Vector2(1 / 16f)).ToExtended();
            this.Axis = Math.Abs(this.Point1.Position.X - this.Point2.Position.X) < 0.0001f ? Axis.Y : Axis.X;
            switch (this.Axis) {
                case Axis.Y:
                    this.interactableArea.X -= 0.1f;
                    this.interactableArea.Width += 0.2f;
                    break;
                case Axis.X:
                    this.interactableArea.Y -= 0.1f;
                    this.interactableArea.Height += 0.2f;
                    break;
            }

            this.Layer = 20;
        }

        public void Move(Vector2 target) {
            switch (this.Axis) {
                case Axis.Y: // If is vertical
                    // Move horizontally
                    this.Point1.Position.X = target.X;
                    this.Point2.Position.X = target.X;
                    this.interactableArea.X = target.X - this.interactableArea.Width / 2;
                    break;
                case Axis.X: // Else if horizontal
                    // Move vertically
                    this.Point1.Position.Y = target.Y;
                    this.Point2.Position.Y = target.Y;
                    this.interactableArea.Y = target.Y - this.interactableArea.Height / 2f;
                    break;
            }
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            RectangleF rectangle = MlemRect.FromCorners(this.Point1.Position.Translate(-1/12f, -1/12f), this.Point2.Position.Translate(1/12f, 1/12f)).ToExtended();
            sb.TiledDrawRectangle(rectangle, Color.Black, 4);
        }

        public bool CanInteract(InteractType type) {
            if (type == InteractType.Drag) {
                return this.Point1.Connection == null && this.Point2.Connection == null;
            }

            return true;
        }

        public void Interact(InteractionManager manager, InteractType type) {
            switch (type) {
                case InteractType.RClickPress:
                    LevelManager.RemoveWire(this.Wire);
                    break;
                case InteractType.Drag:
                    if (manager.DraggingContext.Button == MouseButton.Left)
                        this.Move(manager.MouseTilePosition);
                    break;
                }
        }
    }
}