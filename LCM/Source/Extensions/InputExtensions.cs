using MLEM.Input;

namespace LCM.Extensions {
    public static class InputExtensions {

        public static bool IsMouseButtonReleased(this InputHandler inputHandler, MouseButton mouseButton) {
            return inputHandler.WasMouseButtonDown(mouseButton) && inputHandler.IsMouseButtonUp(mouseButton);
        }

    }
}