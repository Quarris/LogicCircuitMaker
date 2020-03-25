using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCM.Extensions;
using LCM.Game.Save;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Cameras;
using MLEM.Data;
using MLEM.Extended.Extensions;
using MLEM.Extensions;
using MLEM.Input;
using MLEM.Misc;
using MLEM.Startup;
using MLEM.Textures;
using MonoGame.Extended;
using Newtonsoft.Json;

namespace LCM.Game {
    public class InteractionManager {
        public static InputHandler Input => MlemGame.Input;
        public static Level Level => LCMGame.Inst.GameState.Level;
        public static Camera Camera => LCMGame.Inst.GameState.Camera;
        public static Vector2 MouseTilePosition => Camera.ToWorldPos(Input.MousePosition.ToVector2()) / Constants.PixelsPerUnit;

        public GameState GameState { get; }

        public readonly DraggingContext DraggingContext = new DraggingContext();
        public readonly WireSelectionContext WireSelectionContext = new WireSelectionContext();
        public Vector2 ClickedPosition;
        public IEnumerable<IInteractable> HoveredItems = new List<IInteractable>();

        public string SelectedComponent = "";
        public bool IsSelectedPin;

        public string LastSave;

        public InteractionManager(GameState gameState) {
            this.GameState = gameState;
            Input.UpdateOrder = 100;
        }

        public void Update(GameTime gameTime) {
            if (Input.IsDown(Keys.S)) {
                Camera.Position += new Vector2(0, Constants.PixelsPerUnit / 16f * 10f);
            }

            if (Input.IsDown(Keys.W)) {
                Camera.Position += new Vector2(0, -Constants.PixelsPerUnit / 16f * 10f);
            }

            if (Input.IsDown(Keys.A)) {
                Camera.Position += new Vector2(-Constants.PixelsPerUnit / 16f * 10f, 0);
            }

            if (Input.IsDown(Keys.D)) {
                Camera.Position += new Vector2(Constants.PixelsPerUnit / 16f * 10f, 0);
            }

            if (LCMGame.Inst.UiSystem.Controls.GetElementUnderPos(Input.MousePosition.ToVector2()) != null)
                return;

            this.HoveredItems = this.GetHoveredItems();

            /** Keys **/
            if (Input.IsKeyPressed(Keys.F5)) {
                JsonSerializer serializer = new JsonSerializer {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, Level.Save());
                this.LastSave = writer.ToString();
                Console.WriteLine($"Saved level to {this.LastSave}");
            }

            if (Input.IsKeyPressed(Keys.F6)) {
                JsonSerializer serializer = new JsonSerializer {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                SavedLevel level = serializer.Deserialize<SavedLevel>(new JsonTextReader(new StringReader(this.LastSave)));
                Console.WriteLine($"Loaded level from {this.LastSave}");
                LevelManager.LoadLevel(level.Load());
            }

            /** Scroll **/
            int scrollDelta = Input.ScrollWheel - Input.LastScrollWheel;

            if (scrollDelta > 0) {
                Camera.Zoom(0.1f, Input.MousePosition.ToVector2());
            } else if (scrollDelta < 0) {
                Camera.Zoom(-0.1f, Input.MousePosition.ToVector2());
            }

            /** Mouse **/
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                this.ClickedPosition = MouseTilePosition;
                IInteractable item = this.GetInteractableItem(InteractType.LClickPress);
                if (item != null) {
                    item.Interact(this, MouseTilePosition, InteractType.LClickPress);
                } else if (this.SelectedComponent.Length > 0) {
                    Point pos = MouseTilePosition.FloorToPoint();
                    Tile tile;
                    if (this.IsSelectedPin) {
                        tile = new Pin(pos, this.SelectedComponent.Equals("input"));
                    } else {
                        tile = new ComponentTile(pos, Components.ComponentList[this.SelectedComponent]);
                    }

                    LevelManager.TryAddTile(pos, tile);
                }
            }

            if (Input.IsMouseButtonReleased(MouseButton.Left)) {
                this.GetInteractableItem(InteractType.LClickRelease)?.Interact(this, MouseTilePosition, InteractType.LClickRelease);
                if (this.WireSelectionContext.IsActive) {
                    this.WireSelectionContext.Deactivate(this, MouseTilePosition);
                }

                if (this.DraggingContext.Button == MouseButton.Left) {
                    this.DraggingContext.Deactivate(this, MouseTilePosition);
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.Right)) {
                this.ClickedPosition = MouseTilePosition;
                this.GetInteractableItem(InteractType.RClickPress)?.Interact(this, MouseTilePosition, InteractType.RClickPress);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Right)) {
                this.GetInteractableItem(InteractType.RClickRelease)?.Interact(this, MouseTilePosition, InteractType.RClickRelease);
                if (this.DraggingContext.Button == MouseButton.Right) {
                    this.DraggingContext.Deactivate(this, MouseTilePosition);
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.Middle)) {
                this.GetInteractableItem(InteractType.MClickPress)?.Interact(this, MouseTilePosition, InteractType.MClickPress);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Middle)) {
                this.GetInteractableItem(InteractType.MClickRelease)?.Interact(this, MouseTilePosition, InteractType.MClickRelease);
                if (this.DraggingContext.Button == MouseButton.Middle) {
                    this.DraggingContext.Deactivate(this, MouseTilePosition);
                }
            }

            if (!this.DraggingContext.IsActive) {
                this.SetDraggingContext(this.GetInteractableItem(InteractType.StartDrag));
            } else if (this.DraggingContext.Item != null && this.DraggingContext.Item.CanInteract(this, MouseTilePosition, InteractType.Drag)) {
                this.DraggingContext.Item.Interact(this, MouseTilePosition, InteractType.Drag);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            // Render item outline
            IInteractable item = this.GetInteractableItem();
            if (item != null) {
                item.DrawOutline(sb, gameTime, MouseTilePosition);
                switch (item) {
                    case Tile tile:
                        tile.DrawName(sb, LCMGame.Inst.Font, 0.35f, Color.Black);
                        break;
                    case Connector connector:
                        sb.DrawCenteredString(
                            LCMGame.Inst.Font,
                            connector.Name,
                            connector.Position * Constants.PixelsPerUnit,
                            0.2f,
                            Color.Black
                        );
                        break;
                }
            }
            // Render wire preview
            if (this.WireSelectionContext.IsActive) {
                this.WireSelectionContext.DrawWirePreview(sb, MouseTilePosition);
            } else if (this.GetInteractableItem() == null && this.SelectedComponent?.Length > 0) { // Render tile preview
                Texture2D texture;
                Size size;
                Point pos = MouseTilePosition.FloorToPoint();

                if (this.IsSelectedPin) {
                    texture = Pin.Texture;
                    size = new Size(1, 1);
                } else {
                    Component component = Components.ComponentList[this.SelectedComponent];
                    texture = component.Texture;
                    size = component.Size;
                }

                if (!LevelManager.Level.IsAreaOccupied(pos, size)) {
                    sb.Draw(texture, pos.ToVector2() * Constants.PixelsPerUnit, Color.Multiply(Constants.ComponentColor, 0.5f));
                }
            }
        }

        private void SetDraggingContext(IInteractable item) {
            if (Input.IsMouseButtonDown(MouseButton.Left) && !this.ClickedPosition.EqualsWithTolerence(MouseTilePosition, 0.1f)) {
                this.DraggingContext.Activate(this, MouseButton.Left, this.ClickedPosition, item);
            } else if (Input.IsMouseButtonDown(MouseButton.Right) && !this.ClickedPosition.EqualsWithTolerence(MouseTilePosition, 0.1f)) {
                this.DraggingContext.Activate(this, MouseButton.Right, this.ClickedPosition, item);
            } else if (Input.IsMouseButtonDown(MouseButton.Middle) && !this.ClickedPosition.EqualsWithTolerence(MouseTilePosition, 0.1f)) {
                this.DraggingContext.Activate(this, MouseButton.Middle, this.ClickedPosition, item);
            }
        }

        private IInteractable GetInteractableItem(InteractType type = InteractType.Hover) {
            return this.HoveredItems
                .FirstOrDefault(item => item.CanInteract(this, MouseTilePosition, type));
        }

        private IEnumerable<IInteractable> GetHoveredItems() {
            return Level?.Interactables
                .OrderByDescending(item => item.Layer)
                .Where(item => item.InteractableArea.Contains(MouseTilePosition) && item.CanInteract(this, MouseTilePosition));
        }

        public void SelectComponent(string name, bool isPin) {
            this.SelectedComponent = name;
            this.IsSelectedPin = isPin;
        }
    }
}