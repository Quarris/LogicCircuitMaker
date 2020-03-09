using LCM.Game;
using LCM.Ui;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Startup;

namespace LCM {
    public class LCMGame : MlemGame {
        public static LCMGame Inst { get; private set; }
        private SpriteBatch sb;
        public SpriteFont Font;

        public GameState GameState;
        public UiManager UiManager;

        protected override void Initialize() {
            Inst = this;
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
            this.sb = new SpriteBatch(this.GraphicsDevice);
            this.Content.AddJsonConverter(new SizeJsonConverter());
            this.Content.AddJsonConverter(new TokenJsonConverter());
            LCM.Game.Components.LoadComponents(this.Content);
            this.Font = this.Content.Load<SpriteFont>("Fonts/Default");
            this.GameState = new GameState();
            this.UiManager = new UiManager(this.UiSystem, this.Content, this.SpriteBatch);
        }

        protected override void DoUpdate(GameTime gameTime) {
            this.GameState.Update(gameTime);
            base.DoUpdate(gameTime);
        }

        protected override void DoDraw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Constants.BackgroundColor);
            this.GameState.Draw(gameTime, this.sb);
        }
    }
}