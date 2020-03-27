using System.Collections.Generic;
using System.Linq;
using LCM.Extensions;
using LCM.Game.Save;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public class Wire : IInteractable {
        public int Layer => 10;

        private RectangleF interactableArea;
        public RectangleF InteractableArea => this.interactableArea;

        public readonly List<Vector2> WirePoints;
        public Output Start;
        public Input End;

        private LogicState logicState = LogicState.Undefined;
        public LogicState LogicState {
            get => this.logicState;
            // If the wire changes state,
            // update the states of the connectors it is attached to.
            set {
                if (this.logicState == value) {
                    return;
                }

                this.logicState = value;
                if (this.Start != null) {
                    this.Start.LogicState = value;
                }

                if (this.End != null) {
                    this.End.LogicState = value;
                }
            }
        }

        public Wire(List<Vector2> points, Output start = null, Input end = null) {
            this.Start = start;
            this.End = end;
            this.WirePoints = points;

            if (this.Start != null) {
                this.Start.Wire = this;
            }

            if (this.End != null) {
                this.End.Wire = this;
            }

            this.UpdateInteractableArea();
        }

        public void OnRemoved() {
            if (this.Start != null) {
                this.Start.Wire = null;
            }

            if (this.End != null) {
                this.End.Wire = null;
            }
        }

        public bool CanInteract(InteractionManager manager, Vector2 position, InteractType type = InteractType.Hover) {
            Vector2? hov = GetHoveredPoint(position);
            if (type == InteractType.StartDrag) {
                return hov != null && hov.Equals(this.WirePoints.Last());
            }

            if (type == InteractType.EndDrag) {
                IInteractable item = manager.GetInteractableItem(InteractType.EndDrag);
                if (item is Connector connector) {
                    return connector.Wire == null;
                }

                return item == null;
            }

            return IsHoveredOver(position);
        }

        public void Interact(InteractionManager manager, Vector2 position, InteractType type) {
            switch (type) {
                case InteractType.StartDrag:
                    Vector2? hov = GetHoveredPoint(position);
                    if (hov != null && hov == this.WirePoints.Last()) {
                        manager.WireSelectionContext.Activate(this);
                    }

                    break;
                case InteractType.EndDrag:
                    if (manager.WireSelectionContext.IsActive) {
                        IInteractable item = manager.GetInteractableItem(InteractType.LClickRelease);
                        if (item is Connector connector) {
                            manager.WireSelectionContext.Deactivate(manager, connector);
                        } else {
                            manager.WireSelectionContext.Deactivate(manager, position);
                        }
                    }

                    break;
                default: {
                    if (this.IsHoveredOver(position)) {
                        if (type == InteractType.RClickPress) {
                            LevelManager.RemoveWire(this);
                        }
                    }

                    break;
                }
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            for (int i = 1; i < this.WirePoints.Count; i++) {
                if (i < this.WirePoints.Count) {
                    sb.TiledDrawCircle(this.WirePoints[i], Constants.PointRadius, 10, Color.Multiply(this.logicState.Color(), 0.9f),
                        10);
                }

                sb.TiledDrawLine(this.WirePoints[i - 1], this.WirePoints[i], this.logicState.Color(), Constants.WireWidth);
            }
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime, Vector2 position) {
            Vector2? point = GetHoveredPoint(position);
            if (point != null) {
                sb.TiledDrawCircle(point.Value, Constants.PointRadius, 10, Color.Black, 3);
            } else {
                RectangleF? line = GetHoveredLine(position);
                if (line != null) {
                    sb.TiledDrawRectangle(line.Value, Color.Black, 3);
                }
            }
        }

        public bool IsHoveredOver(Vector2 position) {
            return GetHoveredPoint(position) != null || GetHoveredLine(position) != null;
        }

        public void Extend(Vector2 end) {
            List<Vector2> points = Helper.GetWirePointPositions(this.WirePoints.Last(), end);
            for (int i = 1; i < points.Count; i++) {
                this.WirePoints.Add(points[i]);
            }

            this.UpdateInteractableArea();
        }

        public void Extend(Connector end) {
            this.Extend(end.InteractableArea.Center);

            if (end is Input input) {
                this.End = input;
            } else if (end is Output output) {
                this.Start = output;
            }
        }

        private Vector2? GetHoveredPoint(Vector2 position) {
            for (int i = 0; i < this.WirePoints.Count; i++) {
                if (this.WirePoints[i].EqualsWithTolerence(position, Constants.PointRadius * 2)) {
                    return this.WirePoints[i];
                }
            }

            return null;
        }

        private RectangleF? GetHoveredLine(Vector2 position) {
            for (int i = 0; i < this.WirePoints.Count; i++) {
                if (i < this.WirePoints.Count - 1) {
                    RectangleF line = Helper.RectFromCorners(this.WirePoints[i], this.WirePoints[i + 1]);
                    const float inflate = Constants.WireWidth / Constants.PixelsPerUnit;
                    line.Inflate(inflate, inflate);
                    if (line.Contains(position)) {
                        return line;
                    }
                }
            }

            return null;
        }

        private void UpdateInteractableArea() {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            foreach (Vector2 point in this.WirePoints) {
                if (point.X <= minX) {
                    minX = point.X;
                }

                if (point.X >= maxX) {
                    maxX = point.X;
                }

                if (point.Y <= minY) {
                    minY = point.Y;
                }

                if (point.Y >= maxY) {
                    maxY = point.Y;
                }
            }

            const float size = Constants.PointRadius * 2;
            this.interactableArea = new RectangleF(minX - size, minY - size, maxX - minX + 2 * size, maxY - minY + 2 * size);
        }

        public SavedWire Save() {
            return new SavedWire {
                WirePoints = this.WirePoints,

                StartTile = this.Start.Tile.Position,
                StartConnector = this.Start.Tile.Outputs.First(kv => kv.Value.Equals(this.Start)).Key,

                EndTile = this.End.Tile.Position,
                EndConnector = this.End.Tile.Inputs.First(kv => kv.Value.Equals(this.End)).Key,
            };
        }
    }
}