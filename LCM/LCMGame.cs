using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;
using MonoGame.Extended;

namespace LCM {
    public class LCMGame : MlemGame {
        private SpriteBatch sb;
        private float scale;
        
        private GameState gameState;

        protected override void Initialize() {
            this.gameState = new GameState();
            this.IsMouseVisible = true;
            this.scale = 10;
            this.gameState.LoadLevel(new Level(new Size2(10, 10), Vector2.Zero));
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
            this.sb = new SpriteBatch(this.GraphicsDevice);
            
        }

        protected override void Update(GameTime gameTime){
            base.Update(gameTime);
            this.gameState.Update(this, gameTime);

            int scrollDelta = this.InputHandler.LastScrollWheel - this.InputHandler.ScrollWheel;
            if (scrollDelta > 0) {
                this.scale++;
            }
            else if (scrollDelta < 0) {
                this.scale--;
            }
        }

        protected override void DoDraw(GameTime gameTime){
            this.GraphicsDevice.Clear(Color.Firebrick);
            Matrix mat = Matrix.CreateScale(this.scale);
            this.sb.Begin(transformMatrix: mat);
            this.gameState.Draw(this, gameTime, this.Window, this.sb, this.scale);
            this.sb.End();
        }
    }
}