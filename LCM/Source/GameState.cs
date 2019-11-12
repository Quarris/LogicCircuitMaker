using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Cameras;
using MLEM.Input;
using MonoGame.Extended;

namespace LCM {
    public class GameState {
        private Level level;
        public Camera Camera;
        private InputHandler Input => LCMGame.Input;

        public GameState() {
            this.level = new Level();
            this.Camera = new Camera(LCMGame.Inst.GraphicsDevice) {
                AutoScaleWithScreen = true,
                Scale = 0.35f
            };
            this.Camera.LookingPosition = Vector2.Zero;
            this.level.AddComponent(new Point(10, 10), new LogicNOT());
        }

        public void Update(GameTime gameTime) {
            if (Input.IsDown(Keys.S)) {
                this.Camera.Position += new Vector2(0, 10);
            }

            if (Input.IsDown(Keys.W)) {
                this.Camera.Position += new Vector2(0, -10);
            }

            if (Input.IsDown(Keys.A)) {
                this.Camera.Position += new Vector2(-10, 0);
            }

            if (Input.IsDown(Keys.D)) {
                this.Camera.Position += new Vector2(10, 0);
            }

            int scrollDelta = Input.ScrollWheel - Input.LastScrollWheel;

            if (scrollDelta > 0) {
                this.Camera.Zoom(0.1f);
            } else if (scrollDelta < 0) {
                this.Camera.Zoom(-0.1f);
            }

            // Update Level Logic
            this.level.Update(gameTime);
            // Update Robot Position
        }

        public void Draw(GameTime gameTime, SpriteBatch sb) {
            sb.Begin(transformMatrix: this.Camera.ViewMatrix, samplerState: SamplerState.PointClamp);
            sb.FillRectangle(float.MinValue, -1, float.MaxValue * 16, 2 * 16, Color.Black);
            sb.FillRectangle(-1, float.MinValue, 2 * 16, float.MaxValue * 16, Color.Black);
            // Render Logic Components (Gates etc...)
            this.level.Draw(sb, gameTime);
            // Render Robot
            // Robot.Draw();
            sb.End();
        }
    }
}