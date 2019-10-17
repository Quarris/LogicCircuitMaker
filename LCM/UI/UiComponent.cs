using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LCM.UI {
    public class UiComponent {
        /**
         * Absolute position of this component (relative to the screen)
         */
        private Vector2 position;
        public Vector2 Position {
            get => this.position;
            set { this.position = value; }
        }
        private List<UiComponent> children;
        private UiComponent parent;

        public UiComponent() {
        }

        public void Draw(GameTime gameTime) {
            this.DrawBackground(gameTime);
            foreach (UiComponent child in this.children) child.Draw(gameTime);
        }

        public virtual void DrawBackground(GameTime gameTime) {
            
        }

        public void Destroy() {
            foreach (UiComponent child in this.children)
                child.Destroy();
            this.children.Clear();
        }
    }
}