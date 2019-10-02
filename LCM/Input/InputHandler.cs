using Microsoft.Xna.Framework.Input;

namespace LCM.Input {
    public class InputHandler {
        private KeyboardState pState;
        private KeyboardState cState = Keyboard.GetState();
        
        public void Update(){
            this.pState = this.cState;
        }
        
    }
}