using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRChatModeSwitcher
{
    public partial class Form4 : Form
    {
        Dictionary<string, string> profiles;
        public Form4()
        {
            InitializeComponent();
            ActiveControl = buttonOk;
            string profilesJson = ConfigurationManager.AppSettings["Profiles"];
            if (profilesJson == "" || profilesJson == null)
                profilesJson = @"{""0"":""Default""}";
            profiles = JsonConvert.DeserializeObject<Dictionary<string,string>>(profilesJson);
            foreach (var item in profiles)
            {
                listView1.Items.Add(new ListViewItem(new String[] { item.Value, item.Key }));
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int key = 0;
            var keys = profiles.Keys.Select(int.Parse);
            if (keys.Count() == 0)
                key = 0;
            else
            {
                key = keys.Where(x => !keys.Contains(x + 1))
                    .Select(x => x + 1)
                    .Min();
            }
            listView1.Items.Add(new ListViewItem(new string[] { "Name", key.ToString() }));
            profiles.Add(key.ToString(), "Name");
            listView1.Items[listView1.Items.Count - 1].BeginEdit();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            if(listView1.Items.Count == 1)
            {
                MessageBox.Show("プロファイルを全て消すことはできません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string key = listView1.SelectedItems[0].SubItems[1].Text;
            listView1.Items.Remove(listView1.SelectedItems[0]);
            profiles.Remove(key);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["Profiles"] == null)
                config.AppSettings.Settings.Add("Profiles", @"{}");

            profiles = profiles.OrderBy(o => int.Parse(o.Key)).ToDictionary(x => x.Key, x => x.Value);
            var profilesJson = JsonConvert.SerializeObject(profiles);
            config.AppSettings.Settings["Profiles"].Value = profilesJson;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                listView1.SelectedItems[0].BeginEdit();
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            profiles[listView1.SelectedItems[0].SubItems[1].Text] = e.Label;
        }
    }
}
