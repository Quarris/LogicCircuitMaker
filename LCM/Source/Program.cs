using System;
using Microsoft.Xna.Framework;
using MLEM.Misc;

namespace LCM {
    public static class Program {
        [STAThread]
        static void Main(){
            using (LCMGame game = new LCMGame()) {
                TextInputWrapper.Current = new TextInputWrapper.DesktopGl<TextInputEventArgs>((w, c) => w.TextInput += c);
                game.Run();
            }
        }
    }
}