using MonoGame.Extended;

namespace LCM.Extensions {
    public static class TransformExtensions {
        public static Size2 Add(this Size2 size, int amount){
            return new Size2(size.Width + amount, size.Height + amount);
        }
    }
}