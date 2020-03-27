using System;
using System.Collections.Generic;
using System.Linq;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public class WireSelectionContext {
        public bool IsActive;

        public WireSelectionType Type;
        public Connector SelectedConnector;
        public Wire SelectedWire;

        public void Activate(Wire wire) {
            this.SelectedWire = wire;
            this.Type = WireSelectionType.Wire;
            this.Activate();
        }

        public void Activate(Connector connector) {
            this.SelectedConnector = connector;
            this.Type = WireSelectionType.Connector;
            this.Activate();
        }

        private void Activate() {
            this.IsActive = true;
        }

        public void Deactivate(InteractionManager manager, Vector2 end) {
            this.Deactivate();
            if (this.Type == WireSelectionType.Connector) {
                if (this.SelectedConnector != null) {
                    Wire wire;
                    if (this.SelectedConnector is Input input) {
                        wire = LevelManager.CreateWire(end, this.SelectedConnector.InteractableArea.Center);
                        wire.End = input;
                    } else {
                        wire = LevelManager.CreateWire(this.SelectedConnector.InteractableArea.Center, end);
                        wire.Start = (Output) this.SelectedConnector;
                    }

                    this.SelectedConnector.Wire = wire;
                }
            } else if (this.Type == WireSelectionType.Wire) {
                this.SelectedWire.Extend(end, false);
            }
        }

        public void Deactivate(InteractionManager manager, Connector connector) {
            this.Deactivate();
            if (this.Type == WireSelectionType.Connector) {
                Vector2 start = this.SelectedConnector.InteractableArea.Center;
                Vector2 end = connector.InteractableArea.Center;

                if (!start.EqualsWithTolerence(end)) {
                    Output output = this.SelectedConnector is Output outputConn ? outputConn : (Output) connector;
                    Input input = this.SelectedConnector is Input inputConn ? inputConn : (Input) connector;
                    LevelManager.CreateWire(output, input);
                }
            } else if (this.Type == WireSelectionType.Wire) {
                this.SelectedWire.Extend(connector);
            }
        }

        public void Deactivate(InteractionManager manager, Vector2 position, Wire wire) {
            this.Deactivate();

            Vector2? hov = wire.GetHoveredPoint(position);
            if (hov == null || !hov.Equals(wire.WirePoints.Last()) && !hov.Equals(wire.WirePoints.First())) {
                return;
            }

            if (this.Type == WireSelectionType.Connector) {
                wire.Extend(this.SelectedConnector);
            } else if (this.Type == WireSelectionType.Wire) {
                if (wire != this.SelectedWire) {
                    this.SelectedWire.Extend(wire);
                }
            }
        }

        private void Deactivate() {
            this.IsActive = false;
        }

        public void DrawWirePreview(SpriteBatch sb, Vector2 position) {
            Vector2 start = Vector2.Zero;
            Vector2 end = Vector2.Zero;

            if (this.Type == WireSelectionType.Connector) {
                if (this.SelectedConnector is Output) {
                    start = this.SelectedConnector.InteractableArea.Center;
                    end = position;
                } else {
                    start = position;
                    end = this.SelectedConnector.InteractableArea.Center;
                }
            } else if (this.Type == WireSelectionType.Wire) {
                if (this.SelectedWire.End == null) {
                    start = this.SelectedWire.WirePoints.Last();
                    end = position;
                } else if (this.SelectedWire.Start == null) {
                    start = position;
                    end = this.SelectedWire.WirePoints.First();
                }
            }
            IList<Vector2> points = Helper.GetWirePointPositions(start, end);
            for (int i = 0; i < points.Count - 1; i++) {
                sb.DrawLine(points[i] * Constants.PixelsPerUnit, points[i + 1] * Constants.PixelsPerUnit, Color.Red, 6);
            }
        }
    }

    public enum WireSelectionType {
        Wire, Connector
    }
}