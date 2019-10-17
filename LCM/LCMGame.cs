using System;
using LCM.Extensions;
using LCM.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LCM {
    public class LCMGame : Game {
        public readonly GraphicsDeviceManager Graphics;
        private SpriteBatch sb;
        private float scale;
        private InputHandler InputHandler { get; }

        private readonly GameState gameState;

        private static LCMGame instance;
        public static LCMGame Instance => instance;

        public LCMGame() {
            instance = this;
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.InputHandler = new InputHandler();
            this.gameState = new GameState();
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            this.scale = 10;
            this.gameState.LoadLevel(new Level(new Size2(10, 10), Vector2.Zero));
            base.Initialize();
        }

        protected override void LoadContent() {
            this.sb = new SpriteBatch(this.GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            this.InputHandler.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            int scroll = this.InputHandler.ScrollWheelDelta;
            if (scroll > 0)
                this.scale += 0.5f;
            else if (scroll < 0) this.scale -= 0.5f;
            // TODO: Add your update logic here

            this.gameState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.Firebrick);
            Matrix mat = Matrix.CreateScale(this.scale);
            this.sb.Begin(transformMatrix: mat);
            this.gameState.Draw(gameTime, this.Window, this.sb, this.scale);
            this.sb.End();
            this.sb.Begin();
            Ui.Draw(gameTime, this.Window);
            this.sb.End();
        }
    }
}