using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Startup;

namespace LCM.Game {
    public class LogicSimulator {

        private readonly Level level;
        private readonly Queue<Tile> queue = new Queue<Tile>();
        private readonly List<Tile> workedOn;
        private int step;

        public LogicSimulator(Level level) {
            this.level = level;
            this.workedOn = new List<Tile>();
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
                    Tile tile = this.queue.Dequeue();
                    if (!tile.Operate()) {
                        this.workedOn.Add(tile);
                        deadlock = false;
                    }
                }

                if (deadlock) {
                    Console.WriteLine($"Deadlocked at step { this.step }");
                    return;
                }

                foreach (Tile lastTile in this.workedOn) {
                    IEnumerable<Tile> tiles = lastTile.Outputs
                        .Where(kv => kv.Value.LogicState != LogicState.Undefined)
                        .Select(kv => kv.Value)
                        .Where(conn => conn.Wire != null)
                        .Select(conn => conn.Wire.End.Tile);

                    foreach (Tile tile in tiles) {
                        if (!this.queue.Contains(tile)) {
                            bool enqueue = true;
                            foreach (Input input in tile.Inputs.Values) {
                                if (input.LogicState == LogicState.Undefined) {
                                    enqueue = false;
                                    break;
                                }
                            }

                            if (enqueue) {
                                this.queue.Enqueue(tile);
                            }
                        }
                    }
                }
                this.workedOn.Clear();

                Console.WriteLine(string.Join(" ", this.queue));
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