using System;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Cameras;
using MLEM.Input;
using MLEM.Startup;
using MonoGame.Extended;

namespace LCM.Game {
    public class InteractionManager {
        public InputHandler Input => MlemGame.Input;
        public Camera Camera => LCMGame.Inst.GameState.Camera;
        public Vector2 MouseCameraPosition => Input.MousePosition.ToVector2();
        public Vector2 MouseWorldPosition => Camera.ToWorldPos(Input.MousePosition.ToVector2());
        public Vector2 ClickedPosition;
        public bool IsSelecting;
        public bool IsWireSelected;
        public int SelectedComponent { get; private set; }

        private GameState gameState;

        public InteractionManager(GameState gameState) {
            this.gameState = gameState;
        }

        public void Update(GameTime gameTime) {
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
                Camera.Zoom(0.1f, this.MouseCameraPosition);
                Console.WriteLine(Camera.ActualScale);
            } else if (scrollDelta < 0) {
                Camera.Zoom(-0.1f, this.MouseCameraPosition);
                Console.WriteLine(Camera.ActualScale);
            }

            /** Buttons **/
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                if (this.IsWireSelected) {
                    this.IsSelecting = true;
                    this.ClickedPosition = this.MouseWorldPosition;
                } else {
                    LevelManager.TryAddTile(Camera.WorldToTilePos(this.MouseWorldPosition), Components.ComponentList[this.SelectedComponent]);
                }
            }

            if (Input.IsMouseButtonUp(MouseButton.Left)) {
                this.IsSelecting = false;
            }

            if (Input.IsMouseButtonPressed(MouseButton.Right)) {
                if (this.IsWireSelected) {

                } else {
                    LevelManager.RemoveTile(Camera.WorldToTilePos(this.MouseWorldPosition));
                }
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            Point tilePos = Camera.WorldToTilePos(this.MouseWorldPosition);
            sb.DrawRectangle(tilePos.ToVector2() * Constants.PixelsPerUnit, new Vector2(Constants.PixelsPerUnit), Color.Black, Constants.PixelsPerUnit/16f);
            sb.DrawString(LCMGame.Inst.Font, Components.ComponentList[this.SelectedComponent].Name, Camera.Position, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            if (this.IsSelecting) {
                Vector2 mid1 = new Vector2(this.ClickedPosition.X + (this.MouseWorldPosition.X-this.ClickedPosition.X)/2, this.ClickedPosition.Y);
                Vector2 mid2 = new Vector2(this.ClickedPosition.X + (this.MouseWorldPosition.X-this.ClickedPosition.X)/2, this.MouseWorldPosition.Y);
                sb.DrawLine(this.ClickedPosition, mid1, Color.Black, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid1, mid2, Color.Black, Constants.PixelsPerUnit/16f);
                sb.DrawLine(mid2, this.MouseWorldPosition, Color.Black, Constants.PixelsPerUnit/16f);
            }
        }
    }
}