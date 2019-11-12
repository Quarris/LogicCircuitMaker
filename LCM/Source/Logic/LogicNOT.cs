using Microsoft.Xna.Framework;

namespace LCM {
    public class LogicNOT : LogicComponent {
        public LogicNOT() {
            this.AddInput(0, new Point(1, 4), "I");
            this.AddOutput(0, new Point(7, 4), "O");
        }

        public override string GetName() {
            return "NOT Gate";
        }

        public override Point GetSize() {
            return new Point(10);
        }

        public override int GetInputSize() {
            return 1;
        }

        public override int GetOutputSize() {
            return 1;
        }

        public override string GetDescription() {
            return "Inverts the input.";
        }
    }
}