using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Configuration;

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
            ConfigLoad();

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

        string steamPath;
        string oculusPath;
        private void ConfigLoad()
        {
            var test = Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess;
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 438100");
            if (rkey != null)
                steamPath = (string)rkey.GetValue("InstallLocation") + @"\VRChat.exe";
            else
                steamPath = ConfigurationManager.AppSettings["steamPath"];

            oculusPath = ConfigurationManager.AppSettings["oculusPath"];

            if (steamPath == "" && oculusPath == "")
                MessageBox.Show("Steam版VRChatのインストール場所の読み込みに失敗しました。\n設定からSteam版かOculus版のVRChat.exeのパスを設定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (steamPath != "")
                    radioSteam.Enabled = true;
                if (oculusPath != "")
                    radioOculus.Enabled = true;
            }

            var radioSelected = ConfigurationManager.AppSettings["selected"];
            if (radioSelected == "1")
                radioSteam.Checked = true;
            else if (radioSelected == "2")
                radioOculus.Checked = true;
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
            //bool result = RunVRChat(false);
            //if (result)
            //    Application.Exit();
            MessageBox.Show(steamPath + "\n" + oculusPath);
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

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            Form2 fs2 = new Form2();
            fs2.ShowDialog(this);
            ConfigLoad();
        }
    }
}
