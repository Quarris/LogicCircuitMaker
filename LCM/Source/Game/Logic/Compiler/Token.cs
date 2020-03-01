using System.Text;

namespace LCM.Game {
    public class Token {
        public TokenType Type;
        public Token Left;
        public Token Right;
        public readonly string Value;

        public Token(TokenType type, string value) {
            this.Type = type;
            this.Value = value;
        }

        public override bool Equals(object obj) {
            if (!(obj is Token token)) {
                return false;
            }

            if (this.Type == TokenType.Symbol && token.Type == TokenType.Symbol) {
                return this.Value.Equals(token.Value);
            }

            if (this.Type == TokenType.Expression && this.Value.Equals("!")) {
                return token.Type == TokenType.Expression && token.Value.Equals("!") && this.Left.Equals(token.Left);
            }

            return this.Value.Equals(token.Value) && this.Left.Equals(token.Left) && this.Right.Equals(token.Right);
        }

        public override string ToString() {
            return "\n" + PrintToken(this, 0);
        }

        public static string PrintToken(Token token, int indent) {
            if (token == null) {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            if (token.Type != TokenType.Symbol) {
                builder.Append(PrintToken(token.Right, indent + 1));
            }

            for (int i = 0; i < indent; i++) {
                builder.Append("  ");
            }

            builder.AppendLine(token.Value);

            if (token.Type != TokenType.Symbol) {
                builder.Append(PrintToken(token.Left, indent + 1));
            }

            return builder.ToString();
        }
    }

    public enum TokenType {
        Symbol,
        Operation,
        Expression
    }
}