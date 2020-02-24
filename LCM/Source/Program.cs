using System;

namespace LCM {
    public static class Program {
        [STAThread]
        static void Main(){
            using (LCMGame game = new LCMGame())
                game.Run();
        }
    }
}