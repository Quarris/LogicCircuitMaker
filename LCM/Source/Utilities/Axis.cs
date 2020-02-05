using MLEM.Misc;

namespace LCM.Utilities {
    public enum Axis {
        X, Y, Undefined
    }

    public static class AxisHelper {
        public static Axis GetAxis(Direction2 direction) {
            if (direction == Direction2.Left || direction == Direction2.Right) {
                return Axis.X;
            }
            if (direction == Direction2.Up || direction == Direction2.Down) {
                return Axis.Y;
            }
            return Axis.Undefined;
        }
    }
}