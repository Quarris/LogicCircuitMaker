using System;
using System.Linq;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Cameras;
using MLEM.Input;
using MLEM.Startup;
using MonoGame.Extended;
using RectangleF = MLEM.Misc.RectangleF;

namespace LCM.Game {
    public class InteractionManager {
        public static InputHandler Input => MlemGame.Input;
        public static Level Level => LCMGame.Inst.GameState.Level;
        public static Camera Camera => LCMGame.Inst.GameState.Camera;
        public static Vector2 MouseTilePosition => Camera.ToWorldPos(Input.MousePosition.ToVector2()) / Constants.PixelsPerUnit;

        public Vector2 ClickedPosition;
        public Vector2 DragPosition;
        public bool IsDragging;
        public IInteractable HoveredItem;
        public bool IsSelecting;
        public int SelectedComponent { get; private set; }
        public Connector SelectedConnector;

        private GameState gameState;

        public InteractionManager(GameState gameState) {
            this.gameState = gameState;
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

            if (Input.IsKeyPressed(Keys.D1)) {
                Console.WriteLine($"Loading component {Components.ComponentList[0].Name}");
                this.SelectedComponent = 0;
            }

            if (Input.IsKeyPressed(Keys.D2)) {
                Console.WriteLine($"Loading component {Components.ComponentList[1].Name}");
                this.SelectedComponent = 1;
            }

            if (Input.IsKeyPressed(Keys.D3)) {
                Console.WriteLine($"Loading component {Components.ComponentList[2].Name}");
                this.SelectedComponent = 2;
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
                this.ClickedPosition = MouseTilePosition;
                if (this.HoveredItem != null) {
                    this.HoveredItem.Interact(this, InteractType.LClickPress);
                } else {
                    LevelManager.TryAddTile(MouseTilePosition.FloorToPoint(), Components.ComponentList[this.SelectedComponent]);
                }
            }

            if (Input.IsMouseButtonReleased(MouseButton.Left)) {
                this.HoveredItem?.Interact(this, InteractType.LClickRelease);
                this.IsSelecting = false;
            }

            if (Input.IsMouseButtonPressed(MouseButton.Right)) {
                this.HoveredItem?.Interact(this, InteractType.RClickPress);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Right)) {
                this.HoveredItem?.Interact(this, InteractType.RClickRelease);
            }

            if (Input.IsMouseButtonPressed(MouseButton.Middle)) {
                this.HoveredItem?.Interact(this, InteractType.MClickPress);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Middle)) {
                this.HoveredItem?.Interact(this, InteractType.MClickRelease);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            Point tilePos = MouseTilePosition.FloorToPoint();

            if (this.HoveredItem != null && this.HoveredItem.CanInteract()) {
                this.HoveredItem.DrawOutline(sb, gameTime);
            } else {
                sb.DrawRectangle(tilePos.ToVector2() * Constants.PixelsPerUnit, new Vector2(Constants.PixelsPerUnit), Color.Black, Constants.PixelsPerUnit/16f);
            }

            // Draw Wire

            if (this.IsSelecting) {
                Vector2 mid1 = new Vector2(this.ClickedPosition.X + (MouseTilePosition.X-this.ClickedPosition.X)/2, this.ClickedPosition.Y) * Constants.PixelsPerUnit;
                Vector2 mid2 = new Vector2(this.ClickedPosition.X + (MouseTilePosition.X-this.ClickedPosition.X)/2, MouseTilePosition.Y) * Constants.PixelsPerUnit;
                sb.DrawLine(this.ClickedPosition * Constants.PixelsPerUnit, mid1, Color.Red, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid1, mid2, Color.Red, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid2, MouseTilePosition * Constants.PixelsPerUnit, Color.Red, Constants.PixelsPerUnit/16f);
            }

        }

        private IInteractable GetHoveredItem() {
            return Level?.Interactables
                .Where(item => item.InteractableArea.Contains(MouseTilePosition))
                .OrderByDescending(item => item.Layer)
                .FirstOrDefault();
        }
    }
}