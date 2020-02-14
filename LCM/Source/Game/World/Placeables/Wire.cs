using System.Collections.Generic;

namespace LCM.Game {
    public class Wire {
        public readonly Connector Connector1;
        public readonly Connector Connector2;
        public readonly List<WireSegment> Segments;
    }
}