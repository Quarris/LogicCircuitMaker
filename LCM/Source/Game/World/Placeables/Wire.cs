using System;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public class Wire : IInteractable {
        public readonly WirePoint Point1;
        public readonly WirePoint Point2;

        public bool IsVertical => Math.Abs(this.Point1.Position.X - this.Point2.Position.X) < 0.0001f;

        private readonly RectangleF interactableArea;
        public  RectangleF InteractableArea => this.interactableArea;

        private RectangleF DrawRect;

        public Wire(WirePoint point1, WirePoint point2) {
            this.Point1 = point1;
            this.Point2 = point2;
            this.interactableArea = Helper.RectFromCorners(point1.Position - new Vector2(1/16f), point2.Position + new Vector2(1/16f));
            this.DrawRect = Helper.RectFromCorners((Vector2) this.interactableArea.TopLeft * Constants.PixelsPerUnit,
                (Vector2) this.interactableArea.BottomRight * Constants.PixelsPerUnit);
            if (this.IsVertical) {
                this.interactableArea.X -= 0.1f;
                this.interactableArea.Width += 0.2f;
            } else {
                this.interactableArea.Y -= 0.1f;
                this.interactableArea.Height += 0.2f;
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.FillRectangle(Helper.RectFromCorners(
                    (this.Point1.Position - new Vector2(1/32f)) * Constants.PixelsPerUnit,
                    (this.Point2.Position + new Vector2(1/32f)) * Constants.PixelsPerUnit), Color.Red);
            this.Point1.Draw(sb, gameTime);
            this.Point2.Draw(sb, gameTime);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            RectangleF drawRect = Helper.RectFromCorners((Vector2)this.InteractableArea.TopLeft * Constants.PixelsPerUnit, (Vector2)this.InteractableArea.BottomRight * Constants.PixelsPerUnit);
            sb.DrawRectangle(this.DrawRect, Color.Black, 4);
        }

        public void Interact(InteractionManager manager, InteractType type) {

        }
    }

    public class WirePoint: IInteractable {
        public readonly Vector2 Position;
        public RectangleF InteractableArea { get; }

        public WirePoint(Vector2 position) {
            this.Position = position;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {

        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.Position * Constants.PixelsPerUnit, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f), 10, Color.Aqua, 10);

        }

        public void Interact(InteractionManager manager, InteractType type) {

        }
    }
}