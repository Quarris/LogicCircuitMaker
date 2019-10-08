using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LCM {
    public class Level {
        public Size2 Size { get; }
        public Vector2 RobotStartPosition;

        public Level(Size2 size, Vector2 robotStartPosition){
            this.Size = size;
            this.RobotStartPosition = robotStartPosition;
        }
    }
}