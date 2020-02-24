using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Cameras;
using MLEM.Input;
using MonoGame.Extended;

namespace LCM.Game {
    public class GameState {
        public static GameState Get => LCMGame.Inst.GameState;
        public Level Level { get; }
        public Camera Camera { get; }
        private InputHandler Input => LCMGame.Input;
        private readonly InteractionManager interactionManager;

        public GameState() {
            this.Level = new Level();
            this.Camera = new Camera(LCMGame.Inst.GraphicsDevice) {
                AutoScaleWithScreen = true,
                MinScale = 32.0f/Constants.PixelsPerUnit,
                MaxScale = 64.0f/Constants.PixelsPerUnit,
                Scale = 24.0f/Constants.PixelsPerUnit
            };
            this.Camera.LookingPosition = Vector2.Zero;
            this.interactionManager = new InteractionManager(this);
        }

        public void Update(GameTime gameTime) {
            // Update User Interactions
            this.interactionManager.Update(gameTime);

            // Update Level Logic
            this.Level.Update(gameTime);

            // Update Robot Position

        }

        public void Draw(GameTime gameTime, SpriteBatch sb) {
            sb.Begin(transformMatrix: this.Camera.ViewMatrix, samplerState: SamplerState.PointClamp);
            sb.FillRectangle(float.MinValue, -1, float.MaxValue * 16, 2 * 16, Color.Black);
            sb.FillRectangle(-1, float.MinValue, 2 * 16, float.MaxValue * 16, Color.Black);
            // Render Logic Components (Gates etc...)
            this.Level.Draw(sb, gameTime);
            // Render Robot
            // Robot.Draw();
            this.interactionManager.Draw(sb, gameTime);
            sb.End();
            sb.Begin(samplerState:SamplerState.PointClamp);
            // Selected Gate
            sb.DrawString(LCMGame.Inst.Font, Components.ComponentList[this.interactionManager.SelectedComponent].Name, new Vector2(10), Color.Black, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
            sb.End();
        }
    }
}