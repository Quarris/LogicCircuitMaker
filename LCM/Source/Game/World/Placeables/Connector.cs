using System;
using System.Collections.Generic;
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
        public readonly Direction2 Direction;
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

        public Connector(Tile tile, Vector2 position, Direction2 direction) {
            this.Tile = tile;
            this.Position = tile.Position.ToVector2() + position;
            this.Direction = direction;
            Vector2 size = new Vector2(1 / 6f);
            this.InteractableArea = new RectangleF(this.Position - size, size * 2);
            this.Layer = 10;
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.TiledDrawCircle(this.Position, 1/12f, 10, Color.Aqua, 3);
        }

        public bool CanInteract(InteractType type) {
            return this.Wire == null;
        }

        public void Interact(InteractionManager manager, InteractType type) {
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

                        Wire wire = LevelManager.CreateWire(manager.SelectedConnector, this);


                        manager.SelectedConnector.Wire = wire;
                        this.Wire = wire;
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
    }

    public class Output : Connector {

        private readonly Instruction instructions;

        public Output(Tile tile, Vector2 position, Direction2 direction, Instruction instructions) : base(tile, position, direction) {
            this.instructions = instructions;
        }

        public void Operate(Tile tile) {
            this.LogicState = this.instructions.Operate(tile);
        }
    }
}