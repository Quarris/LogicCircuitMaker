using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM {
    public abstract class LogicComponent {
        public Point Position;
        public Rectangle Area => new Rectangle(this.Position, this.GetSize());

        private readonly Connector[] inputs;
        private readonly Connector[] outputs;

        public LogicComponent() {
            this.inputs = new Connector[this.GetInputSize()];
            this.outputs = new Connector[this.GetOutputSize()];
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.FillRectangle(this.Position.ToVector2() * -Vector2.UnitY * 16, this.GetSize().ToVector2() * 16, Color.Gray);
            foreach (Connector input in this.inputs) {
                sb.FillRectangle((this.Position + input.Position).ToVector2() * 16, new Size2(2, 4) * 16, Color.LightBlue);
                sb.FillRectangle((this.Position + input.Position + new Point(-1, 4)).ToVector2() * 16, new Size2(4, 4) * 16, Color.LightBlue);
            }
            foreach (Connector output in this.outputs) {
                sb.FillRectangle((this.Position + output.Position).ToVector2() * 16, new Size2(2, 4) * 16, Color.Orange);
                sb.FillRectangle((this.Position + output.Position + new Point(-1, 4)).ToVector2() * 16, new Size2(4, 4) * 16, Color.Orange);
            }
        }

        public void AddInput(int index, Point position, string name) {
            if (index < 0 || index > this.GetInputSize()) {
                throw new ArgumentOutOfRangeException(
                    $"Tried adding input {index} for component {this.GetName()} with size {this.GetInputSize()}");
            }

            Connector connector = new Connector(name, this, position, Connector.Type.INPUT);
            Rectangle size = new Rectangle(Point.Zero, this.GetSize());
            if (!size.Contains(connector.Area)) {
                throw new ArgumentOutOfRangeException(
                    $"Tried adding input {index} for component {this.GetName()} with outside its area.");
            }

            this.inputs[index] = connector;
        }

        public void AddOutput(int index, Point position, string name) {
            if (index < 0 || index > this.GetOutputSize()) {
                throw new ArgumentOutOfRangeException(
                    $"Tried getting output {index} for component {this.GetName()} with size {this.GetOutputSize()}");
            }

            Connector connector = new Connector(name, this, position, Connector.Type.OUTPUT);
            Rectangle size = new Rectangle(Point.Zero, this.GetSize());
            if (!size.Contains(connector.Area)) {
                throw new ArgumentOutOfRangeException(
                    $"Tried adding output {index} for component {this.GetName()} with outside its area.");
            }

            this.outputs[index] = connector;
        }

        public bool IsConnectorAt(Point point) {
            return this.GetConnectorAt(point) == null;
        }

        public Connector GetConnectorAt(Point point) {
            foreach (Connector input in this.inputs) {
                if (input.Area.Contains(point)) {
                    return input;
                }
            }
            foreach (Connector output in this.outputs) {
                if (output.Area.Contains(point)) {
                    return output;
                }
            }
            return null;
        }

        public Connector GetInput(int index) {
            if (index < 0 || index > this.GetInputSize()) {
                throw new ArgumentOutOfRangeException(
                    $"Tried getting input {index} for component {this.GetName()} with size {this.GetInputSize()}");
            }

            return this.inputs[index];
        }

        public Connector GetOutput(int index) {
            if (index < 0 || index > this.GetOutputSize()) {
                throw new ArgumentOutOfRangeException(
                    $"Tried getting output {index} for component {this.GetName()} with size {this.GetOutputSize()}");
            }

            return this.outputs[index];
        }

        public abstract string GetName();
        public abstract Point GetSize();
        public abstract int GetInputSize();
        public abstract int GetOutputSize();
        public abstract string GetDescription();
    }
}