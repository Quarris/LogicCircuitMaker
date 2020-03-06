using Microsoft.Xna.Framework;

namespace LCM.Utilities {
    public static class Constants {
        public const int PixelsPerUnit = 128;
        public const float PointRadius = 1/12f;
        public const float WireWidth = 6f;

        public static readonly Color LineColor = new Color(173, 225, 244);
        public static readonly Color BackgroundColor = Color.White;
        public static readonly Color ComponentColor = Color.Black;

        public static readonly Color OnLogicStateColor = Color.Green;
        public static readonly Color OffLogicStateColor = Color.Red;
        public static readonly Color UndefinedLogicStateColor = Color.Gray;
    }
    // Dark Theme
    // Line - 207, 117, 32
    // Background - 43, 43, 43
    // Component - 220, 220, 220

    // Light Theme
    // Line - 173, 225, 244
    // Background - Color.White
    // Component - Color.Black
}