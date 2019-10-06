using System;
using LCM.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LCM {
    public class LCMGame : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sb;
        private float scale;
        private Level level;
        private Robot robot;
        public InputHandler InputHandler { get; }

        public LCMGame(){
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.InputHandler = new InputHandler();
            this.scale = 10;
            this.level = new Level(new Size2(10, 10));
            this.robot = new Robot();
        }

        protected override void Initialize(){
            // TODO: Add your initialization logic here.

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

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime){
            GraphicsDevice.Clear(Color.Firebrick);
            Matrix mat = Matrix.CreateScale(this.scale);
            this.sb.Begin(transformMatrix:mat);
            Point center = new Point(this.graphics.PreferredBackBufferWidth/2, this.graphics.PreferredBackBufferHeight/2);
            Size2 size = this.level.Size;
            this.sb.DrawRectangle((center.ToVector2()/this.scale).Translate(-size.Width/2 - 1, -size.Height/2 - 1), size.Add(2), Color.Azure);
            this.sb.FillRectangle(this.robot.Position.Translate(-0.5f, -0.5f), new Size2(1, 1), Color.Aquamarine);
            this.sb.End();
            
            base.Draw(gameTime);
        }
    }
}