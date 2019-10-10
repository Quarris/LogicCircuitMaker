using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LCM.UI {
    public class Component {
        private List<Component> children;
        private Component parent;

        public Component() {
        }

        public void Draw(GameTime gameTime) {
            foreach (Component child in this.children) child.Draw(gameTime);
        }

        public void Destroy() {
            foreach (Component child in this.children)
                child.Destroy();
            this.children.Clear();
        }
    }
}