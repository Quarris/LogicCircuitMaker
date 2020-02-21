using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using MLEM.Extensions;
using MLEM.Misc;
using MonoGame.Extended;

namespace LCM.Extensions {
    public static class TransformExtensions {
        public static double PiOver8 = Math.PI / 8;

        public static Size2 Add(this Size2 size, int amount) {
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

        public static Direction2 CardinalTo(this Vector2 origin, Vector2 position) {
            float angle = position.Translate(-origin.X, -origin.Y).ToAngle();
            if (angle > -MathHelper.PiOver4 && angle <= MathHelper.PiOver4) {
                return Direction2.Up;
            }

            if (angle > MathHelper.PiOver4 && angle <= MathHelper.PiOver2 + MathHelper.PiOver4) {
                return Direction2.Right;
            }

            if (angle > MathHelper.PiOver2 + MathHelper.PiOver4 || angle <= -MathHelper.PiOver2 - MathHelper.PiOver4) {
                return Direction2.Down;
            }

            if (angle > -MathHelper.PiOver2 - MathHelper.PiOver4 && angle <= -MathHelper.PiOver4) {
                return Direction2.Left;
            }

            return Direction2.None;
        }

        public static Direction2 DirectionTo(this Vector2 origin, Vector2 position) {
            float angle = position.Translate(-origin.X, -origin.Y).ToAngle();
            if (angle >= -PiOver8 && angle < PiOver8) {
                return Direction2.Up;
            }

            if (angle >= PiOver8 && angle < MathHelper.PiOver2 - PiOver8) {
                return Direction2.UpRight;
            }

            if (angle >= MathHelper.PiOver2 - PiOver8 && angle < MathHelper.PiOver2 + PiOver8) {
                return Direction2.Right;
            }

            if (angle >= MathHelper.PiOver2 + PiOver8 && angle < MathHelper.Pi - PiOver8) {
                return Direction2.DownRight;
            }

            if (angle >= MathHelper.Pi - PiOver8 || angle < -MathHelper.Pi + PiOver8) {
                return Direction2.Down;
            }

            if (angle >= -MathHelper.Pi + PiOver8 && angle < -MathHelper.PiOver2 - PiOver8) {
                return Direction2.DownLeft;
            }

            if (angle >= -MathHelper.PiOver2 - PiOver8 && angle < -MathHelper.PiOver2 + PiOver8) {
                return Direction2.Left;
            }

            if (angle >= -MathHelper.PiOver2 + PiOver8 && angle < -PiOver8) {
                return Direction2.UpLeft;
            }

            throw new Exception($"I don't know how you broke this honestly...\n {origin} DirectionTo {position} with angle {angle}");
        }
    }
}