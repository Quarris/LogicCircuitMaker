using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Input;

namespace LCM {
    public class Level {
        private readonly List<LogicComponent> components;
        private Robot robot;

        public Level() {
            this.components = new List<LogicComponent>();
            this.robot = new Robot(Vector2.Zero);
        }

        public bool AddComponent(Point point, LogicComponent component) {
            LogicComponent current = this.GetComponentAt(component.Area);
            if (current != null) return false;
            component.Position = point;
            this.components.Add(component);
            return true;
        }

        public LogicComponent GetComponentAt(Rectangle area) {
            return this.components.Find(comp => comp.Area.Intersects(area));
        }

        public void Update(GameTime gameTime) {
            GameState state = LCMGame.Inst.GameState;
            if (LCMGame.Input.IsMouseButtonPressed(MouseButton.Left)) {
                Vector2 click = state.Camera.ToWorldPos(LCMGame.Input.MousePosition.ToVector2())/16f;
                this.robot.Position = click;
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            foreach (LogicComponent component in this.components) {
                component.Draw(sb, gameTime);
            }
            this.robot.Draw(sb, gameTime);
        }
    }
}