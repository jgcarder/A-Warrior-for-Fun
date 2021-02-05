using System;

namespace A_Worrior_For_Fun
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new WorriorGame())
                game.Run();
        }
    }
}
