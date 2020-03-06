using System;
using LCM.Utilities;
using Microsoft.Xna.Framework;

namespace LCM.Game {
    [Flags]
    public enum LogicState {
        Undefined = -1,
        Off = 0,
        On = 1
    }

    public static class LogicStateHelper {
        public static Color Color(this LogicState state) {
            switch (state) {
                case LogicState.Undefined: return Constants.UndefinedLogicStateColor;
                case LogicState.On: return Constants.OnLogicStateColor;
                case LogicState.Off: return Constants.OffLogicStateColor;
                default: {
                    Console.WriteLine($"Undefined Color for state {state}");
                    return Microsoft.Xna.Framework.Color.Pink;
                }
            }
        }

        public static LogicState And(this LogicState state, LogicState other) {
            if (state == LogicState.Undefined || other == LogicState.Undefined) {
                return LogicState.Undefined;
            }

            return state & other;
        }

        public static LogicState Or(this LogicState state, LogicState other) {
            if (state == LogicState.Undefined || other == LogicState.Undefined) {
                return LogicState.Undefined;
            }

            return state | other;
        }

        public static LogicState Xor(this LogicState state, LogicState other) {
            if (state == LogicState.Undefined || other == LogicState.Undefined) {
                return LogicState.Undefined;
            }

            return state ^ other;
        }

        public static LogicState Not(this LogicState state) {
            if (state == LogicState.Undefined) {
                return LogicState.Undefined;
            }

            return 1-state;
        }

    }
}