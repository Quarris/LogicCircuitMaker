using System;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MLEM.Startup;
using MonoGame.Extended;

namespace LCM.Game {
    public class Pin : Tile {
        public static readonly Texture2D texture = MlemGame.LoadContent<Texture2D>("Textures/Components/pin");

        public readonly bool IsInput;

        public Pin(Point position, bool isInput) : base(position, new Size(1, 1)) {
            this.IsInput = isInput;

            if (isInput) {
                this.Outputs.Add("Output", new Output(this, new Vector2(1, 0.5f), Direction2.Right, 0.3f, null));
            } else {
                this.Inputs.Add("Input", new Connector(this, new Vector2(0, 0.5f), Direction2.Left, 0.3f));
            }
        }

        public override bool Operate() {
            if (this.IsInput) {
                this.Outputs["Output"].LogicState = (LogicState) Helper.Random.Next(2);
            } else {
                Console.WriteLine($"Pin at {Position} finished with state {this.Inputs["Input"].LogicState.ToString()}");
            }

            return false;
        }

        public override string Name => this.IsInput ? "Input" : "Output";
        public override Texture2D GetTexture() {
            return texture;
        }
    }
}