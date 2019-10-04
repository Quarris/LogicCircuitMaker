using System;
using Microsoft.Xna.Framework.Input;

namespace LCM.Input {
    public class InputHandler {
        
        // Fields defining previous and current keyboard states
        private KeyboardState pkState;
        private KeyboardState ckState = Keyboard.GetState();

        // Fields defining previous and current mouse states
        private MouseState pmState;
        private MouseState cmState = Mouse.GetState();
        
        // Getters for the keyboard and mouse states
        public KeyboardState KeyboardState => this.ckState;
        public MouseState MouseState => this.cmState;

        public void Update(){
            this.pkState = this.ckState;
            this.pmState = this.cmState;
            
            this.ckState = Keyboard.GetState();
            this.cmState = Mouse.GetState();
        }

        public bool IsKeyPressed(Keys key){
            return !this.pkState.IsKeyDown(key) && this.ckState.IsKeyDown(key);
        }

        public bool IsMouseClicked(MouseButton button){
            switch (button) {
                case MouseButton.Left:
                    return this.pmState.LeftButton == ButtonState.Released &&
                           this.cmState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return this.pmState.RightButton == ButtonState.Released &&
                           this.cmState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return this.pmState.MiddleButton == ButtonState.Released &&
                           this.cmState.MiddleButton == ButtonState.Pressed;
                case MouseButton.X1:
                    return this.pmState.XButton1 == ButtonState.Released &&
                           this.cmState.XButton1== ButtonState.Pressed;
                case MouseButton.X2:
                    return this.pmState.XButton2 == ButtonState.Released &&
                           this.cmState.XButton2 == ButtonState.Pressed;
                default: return false;
            }
        }
    }

    public enum MouseButton {
        Left,
        Middle,
        Right,
        X1,
        X2
    }
}