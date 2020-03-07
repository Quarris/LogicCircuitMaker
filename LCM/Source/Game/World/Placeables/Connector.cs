using LCM.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    public class Connector : IInteractable {
        public int Layer { get; }
        public RectangleF InteractableArea { get; }

        public Vector2 Position { get; }
        public Vector2 OffsetPosition;
        public readonly Direction2 Direction;
        public float Length { get; }
        public readonly Tile Tile;
        public Wire Wire;

        private LogicState logicState = LogicState.Undefined;
        public LogicState LogicState {
            get => this.logicState;
            set {
                if (this.logicState == value) {
                    return;
                }
                this.logicState = value;
                if (this.Wire != null) {
                    this.Wire.LogicState = value;
                }
            }
        }

        public Connector(Tile tile, Vector2 position, Direction2 direction, float length) {
            this.Tile = tile;
            this.Position = tile.Position.ToVector2() + position;
            this.Direction = direction;
            this.Length = length;
            this.OffsetPosition = this.Position + this.Direction.Offset().ToVector2() * this.Length;
            Vector2 size = new Vector2(1 / 6f);
            this.InteractableArea = new RectangleF(this.OffsetPosition - size, size * 2);
            this.Layer = 10;
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime, Vector2 position) {
            sb.TiledDrawCircle(this.OffsetPosition, 1/12f, 10, Color.Black, 3);
        }

        public bool CanInteract(InteractionManager manager, Vector2 position, InteractType type) {
            return this.Wire == null || manager.SelectedConnector?.GetType() == this.GetType();
        }

        public void Interact(InteractionManager manager, Vector2 position, InteractType type) {
            switch (type) {
                case InteractType.LClickPress:
                    manager.IsSelecting = true;
                    manager.SelectedConnector = this;
                    manager.ClickedPosition = this.InteractableArea.Center;
                    break;
                case InteractType.LClickRelease:
                    if (manager.IsSelecting) {
                        manager.IsSelecting = false;

                        Vector2 start = manager.ClickedPosition;
                        Vector2 end = this.InteractableArea.Center;

                        if (start.EqualsWithTolerence(end)) {
                            break;
                        }

                        Output output = manager.SelectedConnector is Output outputConn ? outputConn : (Output) this;
                        Input input = manager.SelectedConnector is Input inputConn ? inputConn : (Input) this;
                        LevelManager.CreateWire(output, input);
                    }
                    break;
            }
        }

        public static implicit operator LogicState(Connector connector) {
            return connector.LogicState;
        }

        public override string ToString() {
            return this.Position.ToString();
        }

        public void OnRemoved() {
            if (this.Wire != null) {
                LevelManager.RemoveWire(this.Wire);
            }
        }
    }

    public class Output : Connector {

        private readonly Instruction instructions;

        public Output(Tile tile, Vector2 position, Direction2 direction, float length, Instruction instructions) : base(tile, position, direction, length) {
            this.instructions = instructions;
        }

        public void Operate(Tile tile) {
            this.LogicState = this.instructions.Operate(tile);
        }
    }

    public class Input : Connector {
        public Input(Tile tile, Vector2 position, Direction2 direction, float length) : base(tile, position, direction, length) {}
    }
}