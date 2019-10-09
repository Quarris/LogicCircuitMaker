using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LCM.UI {
    public class Component {

        private List<Component> children;
        private Component parent;

        public Component(){
            
        }

        public void Draw(GameTime gameTime){
            
        }

        public void Destroy(){
            foreach (var child in this.children)
                child.Destroy();
            this.children.Clear();
        }
    }
}