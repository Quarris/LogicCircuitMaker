using Microsoft.Xna.Framework;
using MLEM.Cameras;

namespace LCM {
    public static class CameraExtensions {

        public static void Zoom(this Camera camera, float zoom) {
            float newScale = camera.Scale + zoom;
            if (newScale <= 0) return;
            Vector2 lookPos = camera.LookingPosition;
            camera.Scale = newScale;
            camera.LookingPosition = lookPos;
        }
    }
}