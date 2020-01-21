using System;
using System.Collections.Generic;
using LCM.Extensions;
using LCM.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MLEM.Textures;
using MonoGame.Extended;

namespace LCM.Game {
    public class Tile {
        public Point Position { get; private set; }
        public Component Component;
        public Size Size => this.Component.Size;
        public Rectangle Area => new Rectangle(this.Position, this.Size);

        public Vector2 DrawPos => this.Position.ToVector2() * Constants.PixelsPerUnit;
        public Size2 DrawSize => this.Size.ToSize2() * Constants.PixelsPerUnit;
        public RectangleF DrawArea => new RectangleF(this.DrawPos, this.DrawSize);

        public Tile(Point position, Component component) {
            this.Position = position;
            this.Component = component;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime) {
            sb.DrawCenteredString(
                LCMGame.Inst.Font,
                this.Component.Name,
                this.DrawPos + new Vector2(Size.Width / 2f, -1) * Constants.PixelsPerUnit,
                0.35f,
                Color.Black
            );
            sb.FillRectangle(this.DrawArea, Color.GreenYellow);
            //this.DrawConnectors(sb, gameTime);
        }

        private void DrawConnectors(SpriteBatch sb, GameTime gameTime) {
            foreach (KeyValuePair<string, Connector> input in this.Component.Inputs) {
                sb.Draw(LCMGame.Inst.TextureMap[0, 0, 2, 3],
                    this.DrawPos + input.Value.Position * Constants.PixelsPerUnit,
                    Color.Azure,
                    0, //MathHelper.PiOver2,
                    new Vector2(0, 0),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}