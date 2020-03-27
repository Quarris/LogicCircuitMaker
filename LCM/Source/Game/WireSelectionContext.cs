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
        public Vector2 Start;

        public WireSelectionType Type;
        public Connector SelectedConnector;
        public Wire SelectedWire;

        public void Activate(Wire wire) {
            this.SelectedWire = wire;
            this.Type = WireSelectionType.Wire;
            this.Activate(wire.WirePoints.Last());
        }

        public void Activate(Connector connector) {
            this.SelectedConnector = connector;
            this.Type = WireSelectionType.Connector;
            this.Activate(connector.InteractableArea.Center);
        }

        private void Activate(Vector2 position) {
            this.IsActive = true;
            this.Start = position;
        }

        public void Deactivate(InteractionManager manager, Vector2 end) {
            if (this.Type == WireSelectionType.Connector) {
                if (this.SelectedConnector != null) {
                    Wire wire = LevelManager.CreateWire(this.Start, end);
                    if (this.SelectedConnector is Input input) {
                        wire.End = input;
                    } else {
                        wire.Start = (Output) this.SelectedConnector;
                    }

                    this.SelectedConnector.Wire = wire;
                }
            } else if (this.Type == WireSelectionType.Wire) {
                this.SelectedWire.Extend(end);
            }

            this.Deactivate();
        }

        public void Deactivate(InteractionManager manager, Connector connector) {
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

            this.Deactivate();
        }

        private void Deactivate() {
            this.IsActive = false;
        }

        public void DrawWirePreview(SpriteBatch sb, Vector2 position) {
            Vector2 start = Vector2.Zero;
            Vector2 end = Vector2.Zero;

            if (this.Type == WireSelectionType.Connector) {
                if (this.SelectedConnector is Output) {
                    start = this.SelectedConnector.Position;
                    end = position;
                } else {
                    start = position;
                    end = this.SelectedConnector.Position;
                }
            } else if (this.Type == WireSelectionType.Wire) {
                start = this.SelectedWire.WirePoints.Last();
                end = position;
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