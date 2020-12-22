using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APT_TaskR
{
    public partial class validate : Form
    {
        public validate()
        {
            InitializeComponent();
        }

        public string upflag;
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void validate_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dash a = new Dash();
            if (this.textBox1.Text != "P@ss")
            {
                upflag = "0";
            }
            else
            {
                upflag = "1";
            }
        }

        private void validate_Load(object sender, EventArgs e)
        {

        }
    }
}
