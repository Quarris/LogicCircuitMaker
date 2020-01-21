using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LCM.Game {
    public class LevelManager {

        public static Level Level => LCMGame.Inst.GameState.Level;

        public static bool TryAddTile(Point position, Component component) {
            return Level.TryAddTile(position, component);
        }

        public static void RemoveTile(Point point) {
            Level.RemoveTile(point);
        }
    }
}