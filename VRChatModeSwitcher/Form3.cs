using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRChatModeSwitcher
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(ClientSize.Width, ClientSize.Height - 42);
            label1.Location = new Point((ClientSize.Width / 2) - (label1.Size.Width / 2), pictureBox1.Size.Height + 9);
        }

        private void Form3_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(ClientSize.Width, ClientSize.Height - 42);
            label1.Location = new Point((ClientSize.Width / 2) - (label1.Size.Width / 2), pictureBox1.Size.Height + 9);
        }
    }
}
