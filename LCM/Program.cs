using System;

namespace LCM {
    public static class Program {
        [STAThread]
        static void Main(){
            using (var game = new LCMGame())
                game.Run();
        }
    }
}