using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Cameras;
using MLEM.Extensions;
using MLEM.Input;
using MLEM.Startup;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class InteractionManager {
        public InputHandler Input => MlemGame.Input;
        public Level Level => LCMGame.Inst.GameState.Level;
        public Camera Camera => LCMGame.Inst.GameState.Camera;
        public Vector2 MouseTilePosition => Camera.ToWorldPos(Input.MousePosition.ToVector2()) / Constants.PixelsPerUnit;

        public readonly DraggingContext DraggingContext = new DraggingContext();
        public Vector2 ClickedPosition;
        public IInteractable HoveredItem;
        public bool IsSelecting;
        public bool IsWireSelected;
        public int SelectedComponent { get; private set; }
        public Connector SelectedConnector;

        public GameState GameState { get; }

        public InteractionManager(GameState gameState) {
            this.GameState = gameState;
        }

        public void Update(GameTime gameTime) {
            this.HoveredItem = this.GetHoveredItem();

            /** Keys **/
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

            if (Input.IsKeyPressed(Keys.I)) {
                this.IsWireSelected = !this.IsWireSelected;
            }

            for (int i = (int) Keys.D1; i < (int) Keys.D9; i++) {
                if (Input.IsKeyPressed((Keys) i)) {
                    int index = i - (int) Keys.D1;
                    if (index >= 0 && index < Components.ComponentList.Count) {
                        this.SelectedComponent = i - (int) Keys.D1;
                    }
                }
            }

            /** Scroll **/
            int scrollDelta = Input.ScrollWheel - Input.LastScrollWheel;

            if (scrollDelta > 0) {
                Camera.Zoom(0.1f, Input.MousePosition.ToVector2());
            } else if (scrollDelta < 0) {
                Camera.Zoom(-0.1f, Input.MousePosition.ToVector2());
            }

            /** Buttons **/
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                this.ClickedPosition = this.MouseTilePosition;
                if (this.HoveredItem != null) {
                    this.HoveredItem.Interact(this, InteractType.LClickPress);
                } else {
                    LevelManager.TryAddTile(MouseTilePosition.FloorToPoint(), Components.ComponentList[this.SelectedComponent]);
                }
            }

            if (Input.IsMouseButtonReleased(MouseButton.Left)) {
                this.HoveredItem?.Interact(this, InteractType.LClickRelease);
                this.IsSelecting = false;
                if (this.DraggingContext.Button == MouseButton.Left) {
                    this.DraggingContext.Deactivate();
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.Right)) {
                this.ClickedPosition = this.MouseTilePosition;
                this.HoveredItem?.Interact(this, InteractType.RClickPress);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Right)) {
                this.HoveredItem?.Interact(this, InteractType.RClickRelease);
                if (this.DraggingContext.Button == MouseButton.Right) {
                    this.DraggingContext.Deactivate();
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.Middle)) {
                this.HoveredItem?.Interact(this, InteractType.MClickPress);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Middle)) {
                this.HoveredItem?.Interact(this, InteractType.MClickRelease);
                if (this.DraggingContext.Button == MouseButton.Middle) {
                    this.DraggingContext.Deactivate();
                }
            }

            this.SetDraggingContext();
            if (this.DraggingContext.IsActive && this.DraggingContext.Item != null && this.DraggingContext.Item.CanInteract(InteractType.Drag)) {
                this.DraggingContext.Item.Interact(this, InteractType.Drag);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            if (this.HoveredItem != null && this.HoveredItem.CanInteract()) {
                this.HoveredItem.DrawOutline(sb, gameTime);
                if (this.HoveredItem is Tile tile) {
                    sb.DrawCenteredString(
                        LCMGame.Inst.Font,
                        tile.Component.Name,
                        tile.Position.ToVector2() * Constants.PixelsPerUnit + new Vector2(tile.Size.Width / 2f, -1) * Constants.PixelsPerUnit,
                        0.35f,
                        Color.Black
                    );
                }
            }

            if (this.IsSelecting) { // Render wire preview
                IList<Vector2> points = Helper.GetWirePointPositions(this.ClickedPosition, this.MouseTilePosition);
                for (int i = 0; i < points.Count-1; i++) {
                    sb.DrawLine(points[i] * Constants.PixelsPerUnit, points[i+1] * Constants.PixelsPerUnit, Color.Red, 6);
                }
            } else if (this.HoveredItem == null) { // Render tile preview
                Component component = Components.ComponentList[this.SelectedComponent];
                if (!LevelManager.Level.IsAreaOccupied(MouseTilePosition.FloorToPoint(), component.Size)) {
                    sb.Draw(component.GetTexture(LCMGame.Inst.TextureMap), this.MouseTilePosition.Floor() * Constants.PixelsPerUnit, Color.Multiply(Constants.ComponentColor, 0.5f));
                }
            }
        }

        private void SetDraggingContext() {
            if (Input.IsMouseButtonDown(MouseButton.Left) && !this.ClickedPosition.EqualsWithTolerence(this.MouseTilePosition, 0.1f)) {
                if (!this.DraggingContext.IsActive) {
                    this.DraggingContext.Activate(MouseButton.Left, this.ClickedPosition, this.HoveredItem);
                }
            } else if (Input.IsMouseButtonDown(MouseButton.Right) && !this.ClickedPosition.EqualsWithTolerence(this.MouseTilePosition, 0.1f)) {
                if (!this.DraggingContext.IsActive) {
                    this.DraggingContext.Activate(MouseButton.Right, this.ClickedPosition, this.HoveredItem);
                }
            } else if (Input.IsMouseButtonDown(MouseButton.Middle) && !this.ClickedPosition.EqualsWithTolerence(this.MouseTilePosition, 0.1f)) {
                if (!this.DraggingContext.IsActive) {
                    this.DraggingContext.Activate(MouseButton.Middle, this.ClickedPosition, this.HoveredItem);
                }
            }
        }

        private IInteractable GetHoveredItem() {
            return this.Level?.Interactables
                .Where(item => item.InteractableArea.Contains(this.MouseTilePosition))
                .OrderByDescending(item => item.Layer)
                .FirstOrDefault();
        }
    }
}