using LCM.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM {
    public class GameState {
        public Level Level;
        public Robot Robot { get; }

        public GameState() {
            this.Robot = new Robot();
        }

        public void LoadLevel(Level level) {
            this.Level = level;
            this.Robot.Position = level.RobotStartPosition;
        }

        public void Update(LCMGame game, GameTime time) {
            // Update Level Logic
            // Update Robot Position
        }

        public void Draw(LCMGame game, GameTime time, GameWindow window, SpriteBatch sb, float scale) {
            // Render Level Layout (Non logic components)
            // Render Logic Components (Gates etc...)
            // Render Robot
            if (this.Level != null) {
                Point center = new Point(window.ClientBounds.Width / 2, window.ClientBounds.Height / 2);
                Size2 size = this.Level.Size;
                sb.DrawRectangle((center.ToVector2() / scale).Translate(-size.Width / 2 - 1, -size.Height / 2 - 1),
                    size.Add(2), Color.Azure);
                sb.FillRectangle(this.Robot.Position.Translate(-0.5f, -0.5f), new Size2(1, 1), Color.Aquamarine);
            }
        }
    }
}