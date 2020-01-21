using LCM.Utilities;
using Microsoft.Xna.Framework;
using MLEM.Cameras;

namespace LCM.Extensions {
    public static class CameraExtensions {
        public static Point CameraToTilePos(this Camera camera, Vector2 cameraPos) {
            return (camera.ToWorldPos(cameraPos) / Constants.PixelsPerUnit).FloorToPoint();
        }
        
        public static Point WorldToTilePos(this Camera camera, Vector2 cameraPos) {
            return (cameraPos / Constants.PixelsPerUnit).FloorToPoint();
        }
    }
}