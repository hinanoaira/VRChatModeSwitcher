using System;
using System.Windows.Forms;

namespace VRChatModeSwitcher
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VRChatModeSwitcher(args));
        }
    }
}
