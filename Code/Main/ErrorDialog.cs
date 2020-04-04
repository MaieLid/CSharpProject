using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class ErrorDialog : Form
    {
        string message;
        string more;

        public ErrorDialog(string message, string more)
        {
            this.message = message;
            this.more = more;
            InitializeComponent();
            msg.Text = message;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            msg.Text = message + "\r\n" + more;
        }
    }
}
