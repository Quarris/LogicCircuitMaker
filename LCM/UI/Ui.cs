using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LCM.UI {
    public class Ui {
        
        private static float scale = 1;
        public static float Scale {
            get => scale;
            set {
                scale = value;
                if (scale <= 0.001f)
                    scale = 0.001f;
            }
        }

        //TODO Add ordering to components.
        private static List<Component> uiComponents = new List<Component>();

        public static void Draw(GameTime gameTime){
            foreach (var component in uiComponents)
                component.Draw(gameTime);
        }
    }
}