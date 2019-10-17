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
        private static List<UiComponent> uiComponents = new List<UiComponent>();

        public static void Draw(GameTime gameTime, GameWindow window) {
            foreach (UiComponent component in uiComponents)
                component.Draw(gameTime);
        }
    }
}