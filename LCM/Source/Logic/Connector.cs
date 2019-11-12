using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MLEM.Startup;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM {
    public class Connector {
        //private static readonly Texture2D CONNECTOR_TEXTURE = MlemGame.LoadContent<Texture2D>("Textures/Connector");

        public string Name { get; }
        public Point Position;
        private LogicComponent component;
        private Type type;

        public Rectangle Area => new Rectangle(this.Position, new Point(2));

        public bool On;

        public Connector(string name, LogicComponent component, Point position, Type type) {
            this.Name = name;
            this.component = component;
            this.Position = position;
            this.type = type;

            this.On = false;
        }

        public void Draw(SpriteBatch sb) {
            Rectangle source = new Rectangle((int)this.type * 4, 0, 4, 8);
            LogicComponent parent = this.component;
        }

        public enum Type {
            INPUT,
            OUTPUT,
            RELAY
        }


    }
}