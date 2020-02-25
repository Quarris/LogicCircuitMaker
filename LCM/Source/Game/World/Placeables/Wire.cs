using System.Collections.Generic;
using System.Linq;
using LCM.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;

namespace LCM.Game {
    public class Wire {
        public static readonly TextureRegion Texture = LCMGame.Inst.TextureMap[0, 2];

        public readonly Connector Connector1;
        public readonly Connector Connector2;

        public LogicState state = LogicState.Invalid;

        public WirePoint Point1 => this.Segments.First().Point1;
        public WirePoint Point2 => this.Segments.Last().Point2;
        public readonly IList<WireSegment> Segments;

        public Color Color => this.state == LogicState.Invalid ? Color.DimGray : this.state == LogicState.Off ? Color.DarkRed : Color.Red;

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
                sb.TiledDrawLine(segment.Point1.Position, segment.Point2.Position, this.Color, 6);
            }

            foreach (WireSegment segment in this.Segments) {
                if (segment.Point1 != this.Point1) {
                    sb.TiledDrawCircle(segment.Point1.Position, 1/12f, 10, Color.Aqua, 10);
                }
            }
        }
    }
}