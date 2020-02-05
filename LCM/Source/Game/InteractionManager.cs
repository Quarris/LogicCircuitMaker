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
        public InputHandler Input => MlemGame.Input;
        public Level Level => LCMGame.Inst.GameState.Level;
        public Camera Camera => LCMGame.Inst.GameState.Camera;
        public Vector2 MouseTilePosition => Camera.ToWorldPos(Input.MousePosition.ToVector2()) / Constants.PixelsPerUnit;
        public Vector2 ClickedPosition;
        public IInteractable HoveredItem;
        public bool IsSelecting;
        public bool IsWireSelected;
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

            if (Input.IsKeyPressed(Keys.I)) {
                this.IsWireSelected = !this.IsWireSelected;
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
                if (this.HoveredItem != null) {
                    this.HoveredItem.Interact(this, InteractType.LClickDown);
                } else {
                    LevelManager.TryAddTile(MouseTilePosition.FloorToPoint(), Components.ComponentList[this.SelectedComponent]);
                }
            }

            if (Input.IsMouseButtonReleased(MouseButton.Left)) {
                this.HoveredItem?.Interact(this, InteractType.LClickUp);
                this.IsSelecting = false;
            }

            if (Input.IsMouseButtonPressed(MouseButton.Right)) {
                this.HoveredItem?.Interact(this, InteractType.RClickDown);
            }

            if (Input.IsMouseButtonReleased(MouseButton.Right)) {
                this.HoveredItem?.Interact(this, InteractType.RClickUp);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            Point tilePos = this.MouseTilePosition.FloorToPoint();

            if (this.HoveredItem != null) {
                this.HoveredItem.DrawOutline(sb, gameTime);
            } else if (!this.IsWireSelected) {
                sb.DrawRectangle(tilePos.ToVector2() * Constants.PixelsPerUnit, new Vector2(Constants.PixelsPerUnit), Color.Black, Constants.PixelsPerUnit/16f);
            }

            // Draw Wire

            if (this.IsSelecting) {
                Vector2 mid1 = new Vector2(this.ClickedPosition.X + (this.MouseTilePosition.X-this.ClickedPosition.X)/2, this.ClickedPosition.Y) * Constants.PixelsPerUnit;
                Vector2 mid2 = new Vector2(this.ClickedPosition.X + (this.MouseTilePosition.X-this.ClickedPosition.X)/2, this.MouseTilePosition.Y) * Constants.PixelsPerUnit;
                sb.DrawLine(this.ClickedPosition * Constants.PixelsPerUnit, mid1, Color.Red, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid1, mid2, Color.Red, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid2, this.MouseTilePosition * Constants.PixelsPerUnit, Color.Red, Constants.PixelsPerUnit/16f);
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