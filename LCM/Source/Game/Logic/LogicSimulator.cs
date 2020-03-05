using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Startup;

namespace LCM.Game {
    public class LogicSimulator {

        private Level level;
        private readonly Queue<Tile> queue = new Queue<Tile>();
        private int step;
        private Tile workedOn;

        public LogicSimulator(Level level) {
            this.level = level;
        }

        public void Update(GameTime gameTime) {
            if (MlemGame.Input.IsKeyPressed(Keys.Space)) {
                this.Step();
                this.step++;
            }

            if (MlemGame.Input.IsKeyPressed(Keys.R)) {
                this.Reset();
            }
        }

        private void Step() {
            if (this.step == 0) {
                this.CollectInputs();
            }
            if (this.queue.Count != 0) {
                bool deadlock = true;
                int count = this.queue.Count;
                for (int i = 0; i < count; i++) {
                    this.workedOn = this.queue.Dequeue();
                    if (this.workedOn.Operate()) {
                        this.queue.Enqueue(this.workedOn);
                    } else {
                        deadlock = false;
                        break;
                    }
                }

                if (deadlock) {
                    Console.WriteLine($"Deadlocked at step { this.step }");
                    return;
                }

                IEnumerable<Tile> tiles = this.workedOn.Outputs
                    .Where(kv => kv.Value.LogicState != LogicState.Undefined)
                    .Select(kv => kv.Value)
                    .Where(conn => conn.Wire != null)
                    .Select(conn => conn.Wire.Connector1 == conn ? conn.Wire.Connector2.Tile : conn.Wire.Connector1.Tile);

                foreach (Tile tile in tiles) {
                    if (!this.queue.Contains(tile)) {
                        this.queue.Enqueue(tile);
                    }
                }
            }

            if (this.queue.Count == 0) {
                Console.WriteLine("Finished Logic");
            }
        }

        private void Reset() {
            this.step = 0;
            this.queue.Clear();
            foreach (Tile tile in this.level.Tiles) {
                tile.Reset();
            }
        }

        private void CollectInputs() {
            foreach (Tile tile in this.level.Tiles.Where(tile => tile is Pin pin && pin.IsInput)) {
                this.queue.Enqueue(tile);
            }
        }

        public void Draw(SpriteBatch batch, GameTime gameTime) {

        }
    }
}