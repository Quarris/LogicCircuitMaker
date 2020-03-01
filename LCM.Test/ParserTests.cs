using System;
using LCM.Game;
using NUnit.Framework;

namespace LCM.Test {
    public class ParserTests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestSingleInputs() {
            Assert.AreEqual(
                new Token(TokenType.Symbol, "a"),
                Parser.Parse("a")
            );
        }

        [Test]
        public void TestExpressionAndOperationEquality() {
            Assert.AreEqual(
                new Token(TokenType.Expression, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Symbol, "b")
                },
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Symbol, "b")
                }
            );
        }

        [Test]
        public void TestSingleOperations() {
            Assert.AreEqual(
                new Token(TokenType.Operation, "&") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Symbol, "b")
                },
                Parser.Parse("a&b")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "^") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Symbol, "b")
                },
                Parser.Parse("a^b")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Symbol, "b")
                },
                Parser.Parse("a|b")
            );
            Assert.AreEqual(
                new Token(TokenType.Expression, "!") {
                    Left = new Token(TokenType.Symbol, "a"),
                },
                Parser.Parse("!a")
            );
        }

        [Test]
        public void TestMultipleOperations() {
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Expression, "!") {
                        Left = new Token(TokenType.Symbol, "b")
                    }
                },
                Parser.Parse("a|!b")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Expression, "&") {
                        Left = new Token(TokenType.Symbol, "a"),
                        Right = new Token(TokenType.Symbol, "b")
                    },
                    Right = new Token(TokenType.Expression, "!") {
                        Left = new Token(TokenType.Expression, "|") {
                            Left = new Token(TokenType.Symbol, "a"),
                            Right = new Token(TokenType.Symbol, "b")
                        }
                    }
                },
                Parser.Parse("(a&b) | !(a|b)")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Operation, "&") {
                        Left = new Token(TokenType.Symbol, "b"),
                        Right = new Token(TokenType.Symbol, "c")
                    }
                },
                Parser.Parse("a|b&c")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Operation, "&") {
                        Left = new Token(TokenType.Symbol, "b"),
                        Right = new Token(TokenType.Operation, "^") {
                            Left = new Token(TokenType.Symbol, "c"),
                            Right = new Token(TokenType.Symbol, "d")
                        }
                    }
                },
                Parser.Parse("a|b&c^d")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "a"),
                    Right = new Token(TokenType.Operation, "&") {
                        Left = new Token(TokenType.Symbol, "b"),
                        Right = new Token(TokenType.Operation, "&") {
                            Left = new Token(TokenType.Symbol, "c"),
                            Right = new Token(TokenType.Operation, "|") {
                                Left = new Token(TokenType.Symbol, "d"),
                                Right = new Token(TokenType.Symbol, "d")
                            }
                        }
                    }
                },
                Parser.Parse("a|b&c&d|d")
            );
        }

        [Test]
        public void TestParenthesis() {
            Assert.AreEqual(
                new Token(TokenType.Operation, "&") {
                    Left = new Token(TokenType.Expression, "|") {
                        Left = new Token(TokenType.Symbol, "a"),
                        Right = new Token(TokenType.Symbol, "b")
                    },
                    Right = new Token(TokenType.Symbol, "c")
                },
                Parser.Parse("(a|b)&c")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "&") {
                    Left = new Token(TokenType.Expression, "|") {
                        Left = new Token(TokenType.Symbol, "a"),
                        Right = new Token(TokenType.Symbol, "b")
                    },
                    Right = new Token(TokenType.Operation, "|") {
                        Left = new Token(TokenType.Symbol, "c"),
                        Right = new Token(TokenType.Operation, "^") {
                            Left = new Token(TokenType.Expression, "&") {
                                Left = new Token(TokenType.Symbol, "a"),
                                Right = new Token(TokenType.Symbol, "d")
                            },
                            Right = new Token(TokenType.Symbol, "w")
                        }
                    }
                },
                Parser.Parse("(a|b)&c|(a&d)^w")
            );
        }

        [Test]
        public void TestNestedParentheses() {
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Expression, "&") {
                        Left = new Token(TokenType.Symbol, "a"),
                        Right = new Token(TokenType.Symbol, "b")
                    },
                    Right = new Token(TokenType.Expression, "&") {
                        Left = new Token(TokenType.Expression, "^") {
                            Left = new Token(TokenType.Symbol, "a"),
                            Right = new Token(TokenType.Symbol, "b")
                        },
                        Right = new Token(TokenType.Symbol, "c")
                    }
                },
                Parser.Parse("(a&b)|((a^b)&c)")
            );
        }

        [Test]
        public void TestLongSymbols() {
            Assert.AreEqual(
                new Token(TokenType.Symbol, "This_IsAn_Input"),
                Parser.Parse("This_IsAn_Input")
            );
            Assert.AreEqual(
                new Token(TokenType.Operation, "|") {
                    Left = new Token(TokenType.Symbol, "SomeLeft_Input"),
                    Right = new Token(TokenType.Symbol, "Some_RightInput")
                },
                Parser.Parse("SomeLeft_Input|Some_Right Input")
            );
        }

        [Test]
        public void TestInvalidInputs() {
            Assert.IsNull(Parser.Parse(""), "Empty input");
            Assert.IsTrue(this.ParserThrowsException(")a&b"), "Illegal closing bracket 1");
            Assert.IsTrue(this.ParserThrowsException("a&b)"), "Illegal closing bracket 2");
            Assert.IsTrue(this.ParserThrowsException("a&b("), "Illegal opening bracket 1");
            Assert.IsTrue(this.ParserThrowsException("(a&b"), "Non closing expression");
            Assert.IsTrue(this.ParserThrowsException("^"), "Single operation statement");
            Assert.IsTrue(this.ParserThrowsException("!"), "Single negation statement");
            Assert.IsTrue(this.ParserThrowsException("a!^b"), "Illegal negation statement");
            Assert.IsTrue(this.ParserThrowsException("$"), "Invalid symbol");
            Assert.IsTrue(this.ParserThrowsException("q^"), "Non even operation on right");
            Assert.IsTrue(this.ParserThrowsException("^t"), "Non even operation on left");
            Assert.IsTrue(this.ParserThrowsException("a|&t"), "Multiple Expression in a row");
        }

        private bool ParserThrowsException(string input) {
            try {
                Parser.Parse(input);
                return false;
            }
            catch (ParserException) {
                return true;
            }
        }
    }
}