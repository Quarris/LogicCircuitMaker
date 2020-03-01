using System.Collections.Generic;

namespace LCM.Game {
    public static class Compiler {

        public static Instruction Compile(ICollection<string> inputs, Token token) {
            string input = token.Value;
            if (token.Type == TokenType.Symbol) {
                if (!inputs.Contains(input)) {
                    throw new CompilerException($"The component contains unknown input {input}");
                }
                return new NoOp(input);
            }

            if (input.Equals("!")) {
                return new Not(Compile(inputs, token.Left));
            }

            if (input.Equals("&")) {
                return new And(Compile(inputs, token.Left), Compile(inputs, token.Right));
            }

            if (input.Equals("|")) {
                return new Or(Compile(inputs, token.Left), Compile(inputs, token.Right));
            }

            if (input.Equals("^")) {
                return new Xor(Compile(inputs, token.Left), Compile(inputs, token.Right));
            }

            throw new CompilerException("End of compile without an instruction.");
        }
    }
}