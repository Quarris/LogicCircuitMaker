using System.Collections.Generic;
using LCM.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;

namespace LCM.Ui {
    public class UiManager {

        private Texture2D uiTextureMap;
        private readonly UiSystem ui;

        public UiManager(UiSystem ui, ContentManager content) {
            this.uiTextureMap = content.Load<Texture2D>("Textures/Ui/UiTextureMap");
            this.ui = ui;

            this.ui.Style = new UiStyle {
                PanelTexture = new NinePatch(new TextureRegion(this.uiTextureMap, 0, 0, 11, 11), 5)
            };

            Panel componentSelect = new Panel(Anchor.AutoRight, new Vector2(0.2f, 1), Vector2.Zero, scrollOverflow: true, scrollerSize: new Point(10, 1));
            foreach (KeyValuePair<string,Component> pair in Components.ComponentList) {
                //componentSelect.AddChild(new Button(Anchor.AutoInline, new Vector2(0.9f, 64), pair.Key));
            }
            //this.ui.Add("ComponentSelect", componentSelect);
        }

    }
}