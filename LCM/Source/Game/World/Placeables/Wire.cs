using System;
using System.Collections.Generic;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Input;
using MonoGame.Extended;

namespace LCM.Game {
    public class Wire : IInteractable {
        public int Layer { get; }
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

        public Wire(WirePoint point1, WirePoint point2) {
            this.Point1 = point1;
            this.Point2 = point2;
            point1.ConnectedWires.Add(this);
            point2.ConnectedWires.Add(this);
            this.interactableArea = Helper.RectFromCorners(point1.Position - new Vector2(1 / 16f), point2.Position + new Vector2(1 / 16f));
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

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.FillRectangle(Helper.RectFromCorners(
                (this.Point1.Position - new Vector2(1 / 32f)) * Constants.PixelsPerUnit,
                (this.Point2.Position + new Vector2(1 / 32f)) * Constants.PixelsPerUnit), Color.Red);
            this.Point1.Draw(sb, gameTime);
            this.Point2.Draw(sb, gameTime);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawRectangle(this.DrawRect, Color.Black, 4);
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
                    LevelManager.RemoveWire(this);
                    break;
                case InteractType.Drag:
                    if (manager.DraggingContext.Button == MouseButton.Left)
                        this.Move(manager.MouseTilePosition);
                    break;
                }
        }
    }

    public class WirePoint : IInteractable {
        public int Layer { get; }
        public Vector2 Position;
        public readonly List<Wire> ConnectedWires;
        public readonly Connector Connection;

        public RectangleF InteractableArea { get; }

        public WirePoint(Vector2 position, Connector connector = null) {
            this.Position = position;
            this.Connection = connector;
            this.ConnectedWires = new List<Wire>();
            this.InteractableArea = Helper.RectFromCorners(position.Translate(-1 / 16f, -1 / 16f), position.Translate(1 / 16f, 1 / 16f));
            this.Layer = 30;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(
                new CircleF(this.Position * Constants.PixelsPerUnit,
                    Constants.PixelsPerUnit * this.InteractableArea.Width / 2f), 10, Color.Aqua, 5);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(
                new CircleF(this.Position * Constants.PixelsPerUnit, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f),
                10, Color.Black, 2.5f
            );
        }

        public bool CanInteract(InteractType type) {
            return true;
        }

        public void Interact(InteractionManager manager, InteractType type) {
        }
    }
}