using System;
using System.Collections.Generic;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LCM.Game {
    public class Wire : IInteractable {
        public int Layer => 20;
        public RectangleF InteractableArea { get; }

        public readonly Connector Start;
        public readonly Connector End;

        private LogicState logicState = LogicState.Undefined;
        public LogicState LogicState {
            get => this.logicState;
            // If the wire changes state,
            // update the states of the connectors it is attached to.
            set {
                if (this.logicState==value) {
                    return;
                }

                this.logicState = value;
                if (this.Start!=null) {
                    this.Start.LogicState = value;
                }

                if (this.End!=null) {
                    this.End.LogicState = value;
                }
            }
        }

        public readonly List<Vector2> WirePoints;

        public Wire(Connector start, Connector end, List<Vector2> points) {
            this.Start = start;
            this.End = end;
            this.WirePoints = points;

            this.WirePoints = points;

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

            const float size = Constants.PointRadius*2;
            this.InteractableArea = new RectangleF(minX-size, minY-size, maxX-minX+2*size, maxY-minY+2*size);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            for (int i = 1; i < this.WirePoints.Count; i++) {
                if (i < this.WirePoints.Count) {
                    sb.TiledDrawCircle(this.WirePoints[i], Constants.PointRadius, 10, Color.Multiply(this.logicState.Color(), 0.9f),
                        10);
                }

                sb.TiledDrawLine(this.WirePoints[i-1], this.WirePoints[i],this.logicState.Color(), Constants.WireWidth);
            }
        }

        public void OnRemoved() {
            if (this.Start!=null) {
                this.Start.Wire = null;
            }

            if (this.End!=null) {
                this.End.Wire = null;
            }
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime, Vector2 position) {
            for (int i = 0; i < this.WirePoints.Count; i++) {
                if (this.WirePoints[i].EqualsWithTolerence(position, Constants.PointRadius*2)) {
                    sb.TiledDrawCircle(this.WirePoints[i], Constants.PointRadius, 10, Color.Black, 3);
                    return;
                }

                if (i < this.WirePoints.Count-1) {
                    RectangleF line = Helper.RectFromCorners(this.WirePoints[i], this.WirePoints[i+1]);
                    const float inflate = Constants.WireWidth/Constants.PixelsPerUnit;
                    line.Inflate(inflate, inflate);
                    if (line.Contains(position)) {
                        sb.TiledDrawRectangle(line, Color.Black, 3);
                        return;
                    }
                }
            }
        }

        public bool CanInteract(InteractionManager manager, Vector2 position, InteractType type = InteractType.Any) {
            for (int i = 0; i < this.WirePoints.Count; i++) {
                if (this.WirePoints[i].EqualsWithTolerence(position, Constants.PointRadius*2)) {
                    return true;
                }

                if (i < this.WirePoints.Count-1) {
                    RectangleF line = Helper.RectFromCorners(this.WirePoints[i], this.WirePoints[i+1]);
                    const float inflate = Constants.WireWidth*3/Constants.PixelsPerUnit;
                    line.Inflate(inflate, inflate);
                    if (line.Contains(position)) {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Interact(InteractionManager manager, Vector2 position, InteractType type) {
        }
    }
}