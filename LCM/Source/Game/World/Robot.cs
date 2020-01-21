using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Color = Microsoft.Xna.Framework.Color;

namespace LCM.Game {
    public class Robot {
        public Vector2 Position;

        public Robot(Vector2 position) {
            this.Position = position;
        }

        public void Update(GameTime gameTime) {

        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.FillRectangle(this.Position * 16, new Size2(2, 2) * 16, Color.LawnGreen);
        }
    }
}