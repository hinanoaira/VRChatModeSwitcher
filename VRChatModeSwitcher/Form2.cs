using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace VRChatModeSwitcher
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textSteamPath.Text = ConfigurationManager.AppSettings["steamPath"];
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 438100");
            if (rkey != null && textSteamPath.Text == "")
            {
                textSteamPath.Text = (string)rkey.GetValue("InstallLocation") + @"\VRChat.exe";
                button2.Enabled = false;
            }
            textOculusPath.Text = ConfigurationManager.AppSettings["oculusPath"];
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            if (settings["steamPath"] == null)
                settings.Add("steamPath", "");
            settings["steamPath"].Value = textSteamPath.Text;

            if (settings["oculusPath"] == null)
                settings.Add("oculusPath", "");
            settings["oculusPath"].Value = textOculusPath.Text;

            if (settings["Arguments"] == null)
                settings.Add("Arguments", "");
            settings["Arguments"].Value = textArguments.Text;

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

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(@"VRChat\shell\open\command");
            if (rkey != null)
            {
                DialogResult result = MessageBox.Show("起動リンクに紐づけしますか？",
                "質問",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    ProcessStartInfo psi = new ProcessStartInfo("LinkInstaller.exe");
                    Process.Start(psi);
                }
            }
            else
                MessageBox.Show("先にVRChatを起動して起動リンクを仕込んでおいてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        int eggCount = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            eggCount++;
            if (eggCount >= 10)
            {
                Form3 fs3 = new Form3();
                fs3.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 fs4 = new Form4();
            fs4.ShowDialog();
        }
    }
}
