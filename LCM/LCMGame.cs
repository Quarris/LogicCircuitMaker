using LCM.Extensions;
using LCM.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LCM {
    public class LCMGame : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sb;
        private float scale;
        public InputHandler InputHandler { get; }

        public readonly GameState GameState;

        public LCMGame(){
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.InputHandler = new InputHandler();
            this.GameState = new GameState();
        }

        protected override void Initialize(){
            this.scale = 10;
            this.GameState.LoadLevel(new Level(new Size2(10, 10), Vector2.Zero));
            base.Initialize();
        }

        protected override void LoadContent(){
            this.sb = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime){
            this.InputHandler.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int scroll = InputHandler.ScrollWheelDelta;
            if (scroll > 0) {
                scale += 0.5f;
            }
            else if (scroll < 0) {
                scale -= 0.5f;
            }
            // TODO: Add your update logic here

            this.GameState.Update(gameTime);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime){
            GraphicsDevice.Clear(Color.Firebrick);
            Matrix mat = Matrix.CreateScale(this.scale);
            this.sb.Begin(transformMatrix:mat);
            this.GameState.Draw(gameTime, this.graphics, this.sb, this.scale);
            this.sb.End();
            this.sb.Begin();
            Ui.Draw(gameTime);
            this.sb.End();
        }
    }
}