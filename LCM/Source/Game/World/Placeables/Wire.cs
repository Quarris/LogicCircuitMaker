using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LCM.Game {
    public class Wire {
        public readonly Connector Connector1;
        public readonly Connector Connector2;

        public WirePoint Point1 => this.Segments.First().Point1;
        public WirePoint Point2 => this.Segments.Last().Point2;

        public readonly IList<WireSegment> Segments;

        public Wire(Connector start, Connector end, IList<Vector2> points) {
            this.Connector1 = start;
            this.Connector2 = end;

            this.Segments = new List<WireSegment>();

            for (int i = 0; i < points.Count-1; i++) {
                WirePoint startPoint = new WirePoint(points[i], i == 0 ? start : null);
                WirePoint endPoint = new WirePoint(points[i+1], i == points.Count - 2 ? end : null);

                this.Segments.Add(new WireSegment(this, startPoint, endPoint));
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            foreach (WireSegment segment in this.Segments) {
                segment.Draw(sb, gameTime);
            }
        }
    }
}