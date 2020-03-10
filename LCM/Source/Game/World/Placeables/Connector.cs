using System;
using LCM.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    public class Connector : IInteractable {
        public int Layer => 20;

        private readonly RectangleF activeInteractableArea;
        private readonly RectangleF inactiveInteractableArea;
        public RectangleF InteractableArea => this.IsActive ? this.activeInteractableArea : this.inactiveInteractableArea;

        public readonly string Name;
        public Vector2 OffsetPosition;
        public Vector2 InsetPosition;
        public Vector2 Position => this.IsActive ? this.OffsetPosition : this.InsetPosition;
        public readonly bool IsOptional;
        public bool IsActive;
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

        protected Connector(string name, Tile tile, Vector2 position, Direction2 direction, bool optional, float length) {
            this.Name = name;
            this.Tile = tile;
            this.Direction = direction;
            this.IsOptional = optional;
            this.IsActive = !optional;
            this.Length = length;
            this.InsetPosition = tile.Position.ToVector2() + position;
            this.OffsetPosition = this.InsetPosition + this.Direction.Offset().ToVector2() * this.Length;
            Vector2 size = new Vector2(1 / 6f);
            this.inactiveInteractableArea = new RectangleF(this.InsetPosition - size, size * 2);
            this.activeInteractableArea = new RectangleF(this.OffsetPosition - size, size * 2);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime, Vector2 position) {
            sb.TiledDrawCircle(this.Position, 1 / 12f, 10, Color.Black, 3);
        }

        public bool CanInteract(InteractionManager manager, Vector2 position, InteractType type) {
            if (type == InteractType.Hover) {
                return true;
            }
            return this.Wire == null || manager.SelectedConnector?.GetType() == this.GetType();
        }

        public void Interact(InteractionManager manager, Vector2 position, InteractType type) {
            switch (type) {
                case InteractType.LClickPress:
                    if (this.IsActive) {
                        manager.IsSelecting = true;
                        manager.SelectedConnector = this;
                        manager.ClickedPosition = this.InteractableArea.Center;
                    } else if (this.IsOptional) {
                        this.IsActive = true;
                    }

                    break;
                case InteractType.LClickRelease:
                    if (this.IsActive && manager.IsSelecting) {
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
                case InteractType.RClickPress:
                    if (this.IsOptional && this.IsActive) {
                        this.IsActive = false;
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

        public Output(string name, Tile tile, Vector2 position, Direction2 direction, bool optional, float length, Instruction instructions)
            : base(name, tile, position, direction, optional, length) {
            this.instructions = instructions;
        }

        public void Operate(Tile tile) {
            this.LogicState = this.instructions.Operate(tile);
        }
    }

    public class Input : Connector {
        public Input(string name, Tile tile, Vector2 position, Direction2 direction, bool optional, float length)
            : base(name, tile, position, direction, optional, length) {
        }
    }
}