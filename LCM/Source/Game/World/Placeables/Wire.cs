using System;
using System.Collections.Generic;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public class Wire : IInteractable {
        public int Layer { get; }
        public readonly WirePoint Point1;
        public readonly WirePoint Point2;

        public Axis Axis { get; }
        public bool IsVertical => Math.Abs(this.Point1.Position.X - this.Point2.Position.X) < 0.0001f;

        private readonly RectangleF interactableArea;
        public RectangleF InteractableArea => this.interactableArea;

        private RectangleF DrawRect;

        public Wire(WirePoint point1, WirePoint point2) {
            this.Point1 = point1;
            this.Point2 = point2;
            point1.ConnectedWires.Add(this);
            point2.ConnectedWires.Add(this);
            this.interactableArea = Helper.RectFromCorners(point1.Position - new Vector2(1/16f), point2.Position + new Vector2(1/16f));
            this.DrawRect = Helper.RectFromCorners((Vector2) this.interactableArea.TopLeft * Constants.PixelsPerUnit,
                (Vector2) this.interactableArea.BottomRight * Constants.PixelsPerUnit);
            this.Axis = Math.Abs(this.Point1.Position.X - this.Point2.Position.X) < 0.0001f ? Axis.X : Axis.Y;
            if (this.Axis == Axis.Y) {
                this.interactableArea.X -= 0.1f;
                this.interactableArea.Width += 0.2f;
            } else if (this.Axis == Axis.X) {
                this.interactableArea.Y -= 0.1f;
                this.interactableArea.Height += 0.2f;
            }

            this.Layer = 20;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.FillRectangle(Helper.RectFromCorners(
                    (this.Point1.Position - new Vector2(1/32f)) * Constants.PixelsPerUnit,
                    (this.Point2.Position + new Vector2(1/32f)) * Constants.PixelsPerUnit), Color.Red);
            this.Point1.Draw(sb, gameTime);
            this.Point2.Draw(sb, gameTime);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawRectangle(this.DrawRect, Color.Black, 4);
        }

        public void Interact(InteractionManager manager, InteractType type) {
            switch (type) {
                case InteractType.RClickDown: {
                    LevelManager.RemoveWire(this);
                    break;
                }
            }
        }
    }

    public class WirePoint: IInteractable {
        public int Layer { get; }
        public readonly Vector2 Position;
        public readonly List<Wire> ConnectedWires;

        public RectangleF InteractableArea { get; }

        public WirePoint(Vector2 position) {
            this.Position = position;
            this.ConnectedWires = new List<Wire>();
            this.InteractableArea = Helper.RectFromCorners(position.Translate(-1/16f, -1/16f), position.Translate(1/16f, 1/16f));
            this.Layer = 30;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.Position * Constants.PixelsPerUnit, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f), 10, Color.Aqua, 5);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.Position * Constants.PixelsPerUnit, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f), 10, Color.Black, 2.5f);
        }

        public void Interact(InteractionManager manager, InteractType type) {

        }
    }
}