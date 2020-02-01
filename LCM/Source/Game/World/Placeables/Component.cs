using System;
using System.Collections.Generic;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Misc;
using MonoGame.Extended;
using RectangleF = MonoGame.Extended.RectangleF;

namespace LCM.Game {
    public class Component {
        public Size Size;
        public readonly string Name;
        public readonly int Id;
        public readonly Dictionary<string, Func<Point, Connector>> Inputs;
        public readonly Dictionary<string, Func<Point, Connector>> Outputs;

        public Component(int id, Size size, string name) {
            this.Size = size;
            this.Name = name;
            this.Id = id;
            this.Inputs = new Dictionary<string, Func<Point, Connector>>();
            this.Outputs = new Dictionary<string, Func<Point, Connector>>();
            Components.ComponentList.Insert(this.Id, this);
        }
    }

    public class Connector : IInteractable {
        public Vector2 Position { get; }
        public Direction2 Direction;

        public RectangleF HoveredArea { get; }

        public Vector2 DrawPos { get; }

        public Connector(Point tilePos, Vector2 position, Direction2 direction) {
            this.Position = position;
            this.Direction = direction;
            this.DrawPos = (tilePos.ToVector2() + position) * Constants.PixelsPerUnit;
            Vector2 size = new Vector2(1/6f);
            this.HoveredArea = new RectangleF(tilePos.ToVector2() + position - size/2, size);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.DrawPos, Constants.PixelsPerUnit * this.HoveredArea.Width/2f), 10, Color.Aqua, 10);
        }

        public void DrawOutline(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCircle(new CircleF(this.DrawPos, Constants.PixelsPerUnit * this.HoveredArea.Width/2f + 1.5f), 10, Color.Black, 3);
        }

        public void Interact(InteractionManager manager, InteractType type) {
            if (type == InteractType.LClick) {
                manager.IsSelecting = true;
                manager.ClickedPosition = manager.MouseTilePosition;
            }
        }
    }
}