using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;

namespace LCM {
    public class LCMGame : MlemGame {
        public static LCMGame Inst { get; private set; }
        private SpriteBatch sb;

        public GameState GameState;

        protected override void Initialize() {
            Inst = this;
            this.GameState = new GameState();
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
            this.sb = new SpriteBatch(this.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
            this.GameState.Update(gameTime);
        }

        protected override void DoDraw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.Firebrick);
            this.GameState.Draw(gameTime, this.sb);
        }
    }
}