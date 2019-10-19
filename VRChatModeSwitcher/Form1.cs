using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;

namespace VRChatModeSwitcher
{
    public partial class formVRChatModeSwitcher : Form
    {
        // 初期化
        private readonly string[] args;
        private readonly string arg;
        public formVRChatModeSwitcher(string[] inArgs)
        {
            args = inArgs;
            arg = "";
            foreach (var item in args)
            {
                arg += item + " ";
            }
            InitializeComponent();

            if (arg == "")
                textBoxLink.Text = "-no arguments-";
            else
            {
                textBoxLink.Text = arg;
                if (arg.Contains("BuildAndRun"))
                    EnableParalellLaunch();
            }
        }
        private void EnableParalellLaunch()
        {
            labelParallel.Visible = true;
            intboxParallel.Visible = true;
        }

        // クリックイベント

        private void ButtonSelectVR_Click(object sender, EventArgs e)
        {
            bool result = RunVRChat(true);
            if (result)
                Application.Exit();
        }

        private void ButtonSelectDesktop_Click(object sender, EventArgs e)
        {
            bool result = RunVRChat(false);
            if (result)
                Application.Exit();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool RunVRChat(bool VRMode)
        {
            string outArg = arg;
            if (!VRMode)
                outArg = "--no-vr " + outArg;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "VRChat.exe";
            psi.Arguments = outArg;

            try
            {
                for (int i = 0; i < intboxParallel.Value; i++)
                {
                    Process p = Process.Start(psi);
                    if (VRMode)
                        if(i == 0) psi.Arguments = "--no-vr " + outArg;
                }
                return true;
            }
            catch(Win32Exception ex)
            {
                if (ex.NativeErrorCode == 2)
                    MessageBox.Show("VRChatの起動に失敗しました。\nVRChat.exeが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("VRChatの起動に失敗しました。\n未知のエラーが発生しました。\nエラーコード : " + ex.NativeErrorCode + "\nエラーメッセージ : " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("VRChatの起動に失敗しました。\n未知のエラーが発生しました。\nエラーメッセージ : " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        int eggCount = 0;
        private void labelLink_Click(object sender, EventArgs e)
        {
            eggCount++;
            if (eggCount == 10)
                this.FormBorderStyle = FormBorderStyle.Sizable;
        }
    }
}
