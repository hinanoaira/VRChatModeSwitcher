using Microsoft.Win32;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace VRChatModeSwitcher
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 438100");
            if (rkey != null)
            {
                textSteamPath.ReadOnly = true;
                textSteamPath.Text = (string)rkey.GetValue("InstallLocation") + @"\VRChat.exe";
                button2.Enabled = false;
            }
            else
                textSteamPath.Text = ConfigurationManager.AppSettings["steamPath"];
            textOculusPath.Text = ConfigurationManager.AppSettings["oculusPath"];
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 438100");
            if (rkey == null)
                config.AppSettings.Settings["steamPath"].Value = textSteamPath.Text;
            config.AppSettings.Settings["oculusPath"].Value = textOculusPath.Text;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "実行可能ファイル|*.exe|すべてのファイル|*.*";
                dlg.FileName = "VRChat.exe";
                //ダイアログを開く
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.textSteamPath.Text = dlg.FileName;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "実行可能ファイル|*.exe|すべてのファイル|*.*";
                dlg.FileName = "VRChat.exe";
                //ダイアログを開く
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.textOculusPath.Text = dlg.FileName;
                }
            }
        }
    }
}
