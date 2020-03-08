using System;
using LCM.Game.Save;
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
                this.Outputs.Add("Output", new Output(this, new Vector2(1, 0.5f), Direction2.Right, false, 0.3f, null));
            } else {
                this.Inputs.Add("Input", new Input(this, new Vector2(0, 0.5f), Direction2.Left, false, 0.3f));
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

        protected override Texture2D GetTexture() {
            return texture;
        }

        protected override SavedTile SaveInternal() {
            return new SavedPinTile {
                Position = this.Position,
                IsInput = this.IsInput
            };
        }
    }
}