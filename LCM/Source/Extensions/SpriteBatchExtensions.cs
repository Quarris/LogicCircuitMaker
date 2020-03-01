using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extended.Extensions;
using MLEM.Textures;
using MonoGame.Extended;
using MlemRect = MLEM.Misc.RectangleF;

namespace LCM.Extensions {
    public static class SpriteBatchExtensions {
        public static void TiledDraw(this SpriteBatch batch, Texture2D texture, Vector2 position, Color color) {
            batch.Draw(texture, position * Constants.PixelsPerUnit, color);
        }

        public static void TiledDrawLine(this SpriteBatch batch, Vector2 start, Vector2 end, Color color, float thickness = 1F, float layerDepth = 0F) {
            batch.DrawLine(start * Constants.PixelsPerUnit, end * Constants.PixelsPerUnit, color, thickness, (int) layerDepth);
        }

        public static void TiledDrawRectangle(this SpriteBatch batch, RectangleF rectangle, Color color, float thickness = 1F, float layerDepth = 0F) {
            RectangleF rect = MlemRect.FromCorners((Vector2) rectangle.TopLeft * Constants.PixelsPerUnit, (Vector2) rectangle.BottomRight * Constants.PixelsPerUnit).ToExtended();
            batch.DrawRectangle(rect, color, thickness, (int) layerDepth);
        }

        public static void TiledDrawCircle(this SpriteBatch batch, CircleF circle, int sides, Color color, float thickness = 1f, float layerDepth = 0F) {
            Point2 center = new Point2(circle.Center.X * Constants.PixelsPerUnit, circle.Center.Y * Constants.PixelsPerUnit);
            batch.DrawCircle(new CircleF(center, circle.Radius * Constants.PixelsPerUnit), sides, color, thickness, (int)layerDepth);
        }

        public static void TiledDrawCircle(this SpriteBatch batch, Vector2 position, float radius, int sides, Color color, float thickness, float layerDepth = 0F) {
            batch.TiledDrawCircle(new CircleF(position, radius), sides, color, thickness, layerDepth);
        }

    }
}