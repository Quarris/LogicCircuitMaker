using System;
using System.Collections.Generic;
using LCM.Extensions;
using Microsoft.Xna.Framework;
using MLEM.Misc;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Utilities {
    public class Helper {
        public static RectangleF RectFromCorners(Vector2 corner1, Vector2 corner2) {
            float minX = Math.Min(corner1.X, corner2.X);
            float minY = Math.Min(corner1.Y, corner2.Y);
            float maxX = Math.Max(corner1.X, corner2.X);
            float maxY = Math.Max(corner1.Y, corner2.Y);
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public static int Round(double value) {
            int ret = (int) Math.Truncate(value);
            ret += value - ret < 0.5 ? 0 : 1;
            return ret;
        }

        private static float ClampWireValue(double value) {
            value *= 10;
            int rounded = Round(value);
            if (Math.Abs(value - rounded) < 0.001f) {
                return rounded / 10f;
            }
            if (rounded % 2 == 0) {
                double rem = value - rounded;
                if (rem > 0 && rem < 0.5f) rounded += 1;
                else                       rounded -= 1;
            }
            return rounded / 10f;
        }

        public static Vector2 ClampWire(Vector2 position) {
            float x = ClampWireValue(position.X);
            float y = ClampWireValue(position.Y);
            return new Vector2(x, y);
        }

        public static IList<Vector2> GetWirePointPositions(Vector2 start, Vector2 end, bool clamp = true, bool straight = false) {
            List<Vector2> points = new List<Vector2>();
            if (straight) {
                points.Add(start);
                Direction2 cardinal = start.CardinalTo(end);
                if (cardinal.GetAxis() == Axis.X) {    // If Horizontal
                    points.Add(new Vector2(end.X, start.Y));
                } else {    // If Vertical
                    points.Add(new Vector2(start.X, end.Y));
                }
            } else if (clamp) {
                return GetWirePointPositions(ClampWire(start), ClampWire(end), false);
            } else {
                Direction2 cardinal = start.CardinalTo(end);
                int dx = Math.Abs(cardinal.Offset().X);
                int dy = Math.Abs(cardinal.Offset().Y);

                float mx = (1 - dx) * start.X + dx * end.X;
                float my = (1 - dy) * start.Y + dy * end.Y;

                points.Add(start);
                points.Add(new Vector2(mx, my));
                points.Add(end);
            }

            return points;
        }

        // -1, 0, 3
        public static int Wrap(int value, int min, int max) {
            if (value >= min && value <= max) {
                return value;
            }

            return min + Mod(value - min, max - min + 1);
        }

        private static int Mod(int a, int b) {
            return (a < 0) ? (a % b + b) : (a % b);
        }
    }
}