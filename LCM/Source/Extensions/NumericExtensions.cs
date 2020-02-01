using System;
using Microsoft.Xna.Framework;
using MLEM.Extensions;
using MonoGame.Extended;

namespace LCM.Extensions {
    public static class TransformExtensions {
        public static Size2 Add(this Size2 size, int amount){
            return new Size2(size.Width + amount, size.Height + amount);
        }

        public static Point RoundToPoint(this Vector2 vector) {
            return vector.Round().ToPoint();
        }

        public static Point FloorToPoint(this Vector2 vector) {
            return vector.Floor().ToPoint();
        }

        public static Rectangle Scale(this Rectangle rectangle, int size) {
            Rectangle newRect = new Rectangle(rectangle.Location, rectangle.Size);
            newRect.Width *= size;
            newRect.Height *= size;
            return newRect;
        }

        public static Size2 ToSize2(this Size size) {
            return new Size2(size.Width, size.Height);
        }
    }
}