using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;
using MonoGame.Extended;

namespace LCM.Game {
    public class Pin : Tile {
        private static readonly Texture2D texture = MlemGame.LoadContent<Texture2D>("Textures/Components/pin");

        public readonly bool IsInput;

        public Pin(Point position, bool isInput) : base(position, new Size(1, 1)) {
            this.IsInput = isInput;
        }

        public override string Name => this.IsInput ? "Input" : "Output";
        public override Texture2D GetTexture() {
            return texture;
        }
    }
}