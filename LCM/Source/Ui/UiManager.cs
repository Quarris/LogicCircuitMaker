using System.Collections.Generic;
using System.IO;
using LCM.Game;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Startup;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;

namespace LCM.Ui {
    public class UiManager {
        private Texture2D uiTextureMap;
        private readonly UiSystem ui;

        public UiManager(UiSystem ui, ContentManager content, SpriteBatch sb) {
            this.uiTextureMap = content.Load<Texture2D>("Textures/Ui/UiTextureMap");
            this.ui = ui;
            this.ui.GlobalScale = 5f;
            this.ui.UpdateOrder = 0;
            this.ui.Controls.HandleKeyboard = false;
            this.ui.Style = new UntexturedStyle(sb) {
                Font = new GenericSpriteFont(LCMGame.Inst.Font),
                TextScale = 0.01f
            };

            Panel root = new Panel(Anchor.AutoRight, new Vector2(0.2f, 1), Vector2.Zero) {
                ChildPadding = Vector2.Zero
            };

            // Saving
            root.AddChild(new Button(Anchor.TopLeft, new Vector2(0.5f, 10), "Save") {
                OnPressed = save => {
                    Group group = new Group(Anchor.TopLeft, Vector2.One, false) {
                        CanBeMoused = true
                    };
                    Panel panel = group.AddChild(new Panel(Anchor.Center, new Vector2(100, 40), Vector2.Zero) {
                        OnUpdated = (p, gameTime) => {
                            if (MlemGame.Input.IsKeyPressed(Keys.Escape)) {
                                p.System.Remove(p.Root.Name);
                            }
                        }
                    });
                    TextField textField = panel.AddChild(new TextField(Anchor.TopCenter, new Vector2(1, 0.5f)));
                    panel.AddChild(new Button(Anchor.BottomLeft, new Vector2(0.5f, 0.5f), "Back") {
                        OnPressed = saveBack => saveBack.System.Remove(saveBack.Root.Name)
                    });
                    panel.AddChild(new Button(Anchor.BottomRight, new Vector2(0.5f, 0.5f), "Save") {
                        OnPressed = saveSave => {
                            string text = textField.Text;
                            FileManager.SaveLevel(LevelManager.Level, text, true);
                        },
                        OnUpdated = (e, time) => ((Button) e).IsDisabled = textField.Text.Length == 0
                    });
                    this.ui.Add("SavePopup", group);
                }
            });

            // Loading
            root.AddChild(new Button(Anchor.TopRight, new Vector2(0.5f, 10), "Load") {
                OnPressed = load => {
                    Group group = new Group(Anchor.TopLeft, Vector2.One, false) {
                        CanBeMoused = true
                    };
                    Panel panel = group.AddChild(new Panel(Anchor.Center, new Vector2(0.5f, 0.5f), Vector2.Zero) {
                        OnUpdated = (p, gameTime) => {
                            if (MlemGame.Input.IsKeyPressed(Keys.Escape)) {
                                p.System.Remove(p.Root.Name);
                            }
                        },
                        ChildPadding = new Vector2(2)
                    });

                    Panel levelSelect = group.AddChild(new Panel(Anchor.TopCenter, new Vector2(1, 0.7f), Vector2.Zero, true, false, new Point(5, 5)) {
                        ChildPadding = new Vector2(2)

                    });
                    foreach (FileInfo file in FileManager.ListLevels()) {
                        string name = file.Name.Replace(".json", "");
                        levelSelect.AddChild(new Button(Anchor.AutoCenter, new Vector2(1, 20), name) {
                            OnPressed = e => {
                                if (FileManager.LoadLevel(name, out Level level)) {
                                    LevelManager.LoadLevel(level);
                                    e.System.Remove(e.Root.Name);
                                }
                            },
                            Padding = new Vector2(0, 0.5f)
                        });
                    }

                    panel.AddChild(levelSelect);
                    panel.AddChild(new Button(Anchor.BottomCenter, new Vector2(1, 0.15f), "Back") {
                        OnPressed = saveBack => saveBack.System.Remove(saveBack.Root.Name)
                    });
                    this.ui.Add("LoadPopup", group);
                }
            });

            // Components
            Panel componentSelect = root.AddChild(new Panel(Anchor.AutoInline, new Vector2(1, 1), Vector2.Zero, scrollOverflow: true, scrollerSize: new Point(5, 5)));

            // Input
            Button inputButton = componentSelect.AddChild(new Button(Anchor.AutoInline, new Vector2(1, 20), "Input") {
                Padding = new Vector2(0, 0.5f)
            });
            inputButton.Text.PositionOffset += new Vector2(15 / 2f, 0);
            inputButton.AddChild(new Image(Anchor.CenterLeft, new Vector2(15), new TextureRegion(Pin.Texture)) {
                Padding = new Vector2(2, 0)
            }, 0);
            inputButton.OnPressed += e => {
                GameState.Get.InteractionManager.SelectComponent("input", true);
            };

            // Output
            Button outputButton = componentSelect.AddChild(new Button(Anchor.AutoInline, new Vector2(1, 20), "Output") {
                Padding = new Vector2(0, 0.5f)
            });
            outputButton.Text.PositionOffset += new Vector2(15 / 2f, 0);
            outputButton.AddChild(new Image(Anchor.CenterLeft, new Vector2(15), new TextureRegion(Pin.Texture)) {
                Padding = new Vector2(2, 0)
            }, 0);
            outputButton.OnPressed += e => {
                GameState.Get.InteractionManager.SelectComponent("output", true);
            };

            // Loaded
            foreach (KeyValuePair<string, Component> pair in Components.ComponentList) {
                Button button = componentSelect.AddChild(new Button(Anchor.AutoInline, new Vector2(1, 20), pair.Value.Name) {
                    Padding = new Vector2(0, 0.5f)
                });
                button.Text.PositionOffset += new Vector2(15 / 2f, 0);
                button.AddChild(new Image(Anchor.CenterLeft, new Vector2(15), new TextureRegion(pair.Value.Texture)) {
                    Padding = new Vector2(2, 0)
                }, 0);
                button.OnPressed += e => {
                    GameState.Get.InteractionManager.SelectComponent(pair.Key, false);
                };
            }



            this.ui.Add("Root", root);
        }
    }
}