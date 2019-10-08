using Microsoft.Xna.Framework;

namespace LCM {
    public class GameState {
        public Level Level;
        public Robot Robot { get; }

        public GameState(){
            Robot = new Robot();
        }

        public void LoadLevel(Level level){
            this.Level = level;
            this.Robot.Position = level.RobotStartPosition;
        }

        public void Update(GameTime gameTime){
            // Update Level Logic
            // Update Robot Position
        }

        public void Render(){
            // Render Level Layout
            // Render Components (Gates etc...)
            // Render Robot
        }
    }
}