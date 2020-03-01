using System;
using System.Linq;

namespace LCM.Game {
    public class Parser {
        public static Token Parse(string input) {
            string toParse = input.Replace(" ", "");
            if (!toParse.Any()) {
                return null;
            }

            int counter = 0;
            return ParseInput(toParse, ref counter);
        }

        private static Token ParseInput(string input, ref int index) {
            bool startBracket = false;
            bool endBracket = false;
            bool seenExpression = false;
            bool seenOperation = false;
            bool seenNegation = false;
            if (index > 0) {
                startBracket = true;
            }

            Token root = null;
            for (; index < input.Length; index++) {
                char c = input[index];
                if (c == '(') {
                    index++;
                    Token token = ParseInput(input, ref index);
                    token.Type = TokenType.Expression;
                    if (seenNegation) {
                        token = new Token(TokenType.Expression, "!") {
                            Left = token
                        };
                    }
                    seenExpression = true;
                    seenOperation = false;
                    if (root == null) {
                        root = token;
                    } else {
                        Token right = FindRightMostOperation(root);
                        right.Right = token;
                    }
                } else if (c == ')') {
                    endBracket = true;
                    break;
                } else if (c == '!') {
                    if (seenExpression && !seenOperation) {
                        throw new ParserException($"Illegal position for Negation at '{index}' for input '{input}'");
                    }

                    seenNegation = true;
                } else if (c == '&' || c == '|' || c == '^') {
                    if (!seenExpression) {
                        throw new ParserException($"Operation '{c}' with no left value at '{index}' for input '{input}'");
                    }

                    if (seenOperation || seenNegation) {
                        throw new ParserException($"Two operations in a row at '{index}' for input '{input}'");
                    }

                    seenOperation = true;

                    if (root.Type != TokenType.Operation) {
                        root = new Token(TokenType.Operation, c.ToString()) {Left = root};
                    } else {
                        Token right = FindRightMostOperation(root);
                        right.Right = new Token(TokenType.Operation, c.ToString()) {
                            Left = right.Right
                        };
                    }
                } else if (IsSymbolChar(c)) {
                    int start = index;
                    while (index < input.Length && IsSymbolChar(input[index])) {
                        index++;
                    }

                    if (seenOperation) {
                        seenOperation = false;
                    } else {
                        seenExpression = true;
                    }

                    string symbol = input.Substring(start, index - start);
                    Token token = new Token(TokenType.Symbol, symbol);
                    if (seenNegation) {
                        token = new Token(TokenType.Expression, "!") {
                            Left = token
                        };
                    }
                    if (root == null) {
                        root = token;
                    } else {
                        Token right = FindRightMostOperation(root);
                        right.Right = token;
                    }

                    index--;
                } else {
                    throw new ParserException($"Invalid character '{c}' at '{index}' for input '{input}'");
                }
            }

            if (startBracket != endBracket) {
                throw new ParserException($"Uneven brackets at '{index}' for input '{input}'");
            }

            if (seenOperation) {
                throw new ParserException($"Uneven expression on the right of an operation at '{index}' for input '{input}'");
            }

            if (seenNegation && !seenExpression) {
                throw new ParserException("Invalid negation without an expression");
            }

            return root;
        }

        private static bool IsSymbolChar(char c) {
            return char.IsDigit(c) || char.IsLetter(c) || c == '_';
        }

        private static Token FindRightMostOperation(Token token) {
            Console.WriteLine($"Right: {token.Right}");
            if (token.Right == null) return token;
            while (token.Right != null && token.Right.Type == TokenType.Operation) {
                token = token.Right;
            }

            return token;
        }
    }
}