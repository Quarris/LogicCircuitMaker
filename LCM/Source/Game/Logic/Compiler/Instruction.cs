namespace LCM.Game {
    public abstract class Instruction {
        public abstract LogicState Operate(Tile tile);
    }

    public class And : Instruction {
        public readonly Instruction Left, Right;

        public And(Instruction left, Instruction right) {
            this.Left = left;
            this.Right = right;
        }

        public override LogicState Operate(Tile tile) {
            return this.Left.Operate(tile).And(this.Right.Operate(tile));
        }
    }

    public class Or : Instruction {
        public readonly Instruction Left, Right;

        public Or(Instruction left, Instruction right) {
            this.Left = left;
            this.Right = right;
        }

        public override LogicState Operate(Tile tile) {
            return this.Left.Operate(tile).Or(this.Right.Operate(tile));
        }
    }

    public class Xor : Instruction {
        public readonly Instruction Left, Right;

        public Xor(Instruction left, Instruction right) {
            this.Left = left;
            this.Right = right;
        }

        public override LogicState Operate(Tile tile) {
            return this.Left.Operate(tile).Xor(this.Right.Operate(tile));
        }
    }

    public class Not : Instruction {
        public readonly Instruction Next;

        public Not(Instruction next) {
            this.Next = next;
        }

        public override LogicState Operate(Tile tile) {
            return this.Next.Operate(tile).Not();
        }
    }

    public class NoOp : Instruction {
        public readonly string Input;

        public NoOp(string input) {
            this.Input = input;
        }

        public override LogicState Operate(Tile tile) {
            return tile.Inputs[this.Input];
        }
    }
}