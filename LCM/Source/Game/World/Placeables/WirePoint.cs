using System.Collections.Generic;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public class WirePoint : IInteractable {
        public int Layer { get; }
        public Vector2 Position;
        public readonly List<WireSegment> ConnectedWires;
        public readonly Connector Connection;

        public RectangleF InteractableArea { get; }

        public WirePoint(Vector2 position, Connector connector = null) {
            this.Position = position;
            this.Connection = connector;
            this.ConnectedWires = new List<WireSegment>();
            this.InteractableArea = Helper.RectFromCorners(position.Translate(-1 / 16f, -1 / 16f), position.Translate(1 / 16f, 1 / 16f));
            this.Layer = 30;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(
                new CircleF(this.Position * Constants.PixelsPerUnit,
                    Constants.PixelsPerUnit * this.InteractableArea.Width / 2f), 10, Color.Aqua, 5);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime, Vector2 position) {
            sb.DrawCircle(
                new CircleF(this.Position * Constants.PixelsPerUnit, Constants.PixelsPerUnit * this.InteractableArea.Width / 2f),
                10, Color.Black, 2.5f
            );
        }

        public bool CanInteract(InteractionManager manager, Vector2 position, InteractType type) {
            return true;
        }

        public void Interact(InteractionManager manager, Vector2 position, InteractType type) {
        }

        public override string ToString() {
            return this.Position.ToString();
        }
    }
}