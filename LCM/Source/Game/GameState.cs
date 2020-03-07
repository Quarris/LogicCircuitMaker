using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Cameras;
using MLEM.Input;

namespace LCM.Game {
    public class GameState {
        public static GameState Get => LCMGame.Inst.GameState;
        public Level Level { get; private set; }
        public Camera Camera { get; }
        private InputHandler Input => LCMGame.Input;
        private readonly InteractionManager interactionManager;

        public GameState() {
            this.Camera = new Camera(LCMGame.Inst.GraphicsDevice) {
                AutoScaleWithScreen = true,
                MinScale = 48.0f / Constants.PixelsPerUnit,
                MaxScale = 64.0f / Constants.PixelsPerUnit,
                Scale = 24.0f / Constants.PixelsPerUnit,
                LookingPosition = Vector2.Zero
            };
            this.interactionManager = new InteractionManager(this);
        }

        public void Update(GameTime gameTime) {
            // Update User Interactions
            this.interactionManager.Update(gameTime);

            // Update Level Logic
            this.Level.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb) {
            sb.Begin(transformMatrix: this.Camera.ViewMatrix, samplerState: SamplerState.PointClamp);
            // Render Logic Components (Gates etc...)
            this.Level.Draw(sb, gameTime);
            this.interactionManager.Draw(sb, gameTime);
            sb.End();

            sb.Begin(samplerState: SamplerState.PointClamp);
            // Selected Gate
            //sb.DrawString(LCMGame.Inst.Font, Components.ComponentList[this.interactionManager.SelectedComponent].Name, new Vector2(10), Color.Black, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);

            sb.DrawString(LCMGame.Inst.Font, $"{this.interactionManager.MouseTilePosition.FloorToPoint().X} : {this.interactionManager.MouseTilePosition.FloorToPoint().Y}", new Vector2(10, LCMGame.Inst.GraphicsDevice.Viewport.Height - LCMGame.Inst.Font.MeasureString("R").Y * 0.1f - 10), Color.Black, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
            sb.End();
        }

        public void LoadLevel(Level level) {
            this.Level = level;
        }

        public void LoadLevel(string fileName) {
            this.LoadLevel(FileManager.LoadLevel(fileName, out Level level) ? level : new Level());
        }
    }
}