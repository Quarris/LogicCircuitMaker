using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LCM.Utilities {
    public class Helper {
        public static RectangleF RectFromCorners(Vector2 corner1, Vector2 corner2) {
            float minX = Math.Min(corner1.X, corner2.X);
            float minY = Math.Min(corner1.Y, corner2.Y);
            float maxX = Math.Max(corner1.X, corner2.X);
            float maxY = Math.Max(corner1.Y, corner2.Y);
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }
    }
}