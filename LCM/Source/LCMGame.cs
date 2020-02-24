using LCM.Game;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;
using MLEM.Textures;

namespace LCM {
    public class LCMGame : MlemGame {
        public static LCMGame Inst { get; private set; }
        private SpriteBatch sb;
        public SpriteFont Font;
        public UniformTextureAtlas TextureMap;

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
            LCM.Game.Components.LoadComponents();
            this.Font = this.Content.Load<SpriteFont>("Fonts/Default");
            this.TextureMap = new UniformTextureAtlas(this.Content.Load<Texture2D>("Textures/TextureMap"), 8, 8);
        }

        protected override void DoUpdate(GameTime gameTime) {
            base.DoUpdate(gameTime);
            this.GameState.Update(gameTime);
        }

        protected override void DoDraw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Constants.BackgroundColor);
            this.GameState.Draw(gameTime, this.sb);
        }
    }
}