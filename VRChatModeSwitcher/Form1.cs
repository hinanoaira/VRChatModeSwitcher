using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Configuration;
using System.IO;
using System.Reflection;

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
        string arguments;
        private void ConfigLoad()
        {
            var test = Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess;
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 438100");
            if (rkey != null && textSteamPath.Text == "")
                steamPath = (string)rkey.GetValue("InstallLocation") + @"\VRChat.exe";
            else
                steamPath = ConfigurationManager.AppSettings["steamPath"];

            oculusPath = ConfigurationManager.AppSettings["oculusPath"];
            arguments = ConfigurationManager.AppSettings["Arguments"];

            if (steamPath == "" && oculusPath == "")
            {
                MessageBox.Show("Steam版VRChatのインストール場所の読み込みに失敗しました。\n設定からSteam版かOculus版のVRChat.exeのパスを設定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonSelectDesktop.Enabled = false;
                buttonSelectVR.Enabled = false;
                radioSteam.Enabled = false;
                radioOculus.Enabled = false;
            }
            else
            {
                var radioSelected = ConfigurationManager.AppSettings["selected"];
                if (radioSelected == "1")
                    radioSteam.Checked = true;
                else if (radioSelected == "2")
                    radioOculus.Checked = true;

                radioSteam.Enabled = (steamPath != "");
                radioOculus.Enabled = (oculusPath != "");
                buttonSelectDesktop.Enabled = (radioOculus.Checked || radioSteam.Checked);
                buttonSelectVR.Enabled = (radioOculus.Checked || radioSteam.Checked);
            }

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
            if (arguments != "")
                outArg = $"{arguments} {outArg}";
            if (!VRMode)
                outArg = $"--no-vr {outArg}";

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "VRChat.exe";
            psi.Arguments = outArg;
            string path = "";

            if (radioSteam.Checked)
                path = Path.GetDirectoryName(steamPath);
            else if (radioOculus.Checked)
                path = Path.GetDirectoryName(oculusPath);
            try
            {
                Environment.CurrentDirectory = path;
                for (int i = 0; i < intboxParallel.Value; i++)
                {
                    Process p = Process.Start(psi);
                    if (VRMode && i == 0) psi.Arguments = $"--no-vr {outArg}";
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
            Assembly myAssembly = Assembly.GetEntryAssembly();
            path = Path.GetDirectoryName(myAssembly.Location);
            Environment.CurrentDirectory = path;
            return false;
        }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            Form2 fs2 = new Form2();
            fs2.ShowDialog(this);
            ConfigLoad();
        }

        private void radioSteam_CheckedChanged(object sender, EventArgs e)
        {
            buttonSelectDesktop.Enabled = true;
            buttonSelectVR.Enabled = true;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (radioSteam.Checked)
                config.AppSettings.Settings["Selected"].Value = "1";
            else
                config.AppSettings.Settings["Selected"].Value = "2";

            config.Save();
        }
    }
}
