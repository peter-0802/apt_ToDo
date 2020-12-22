using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace APT_TaskR
{
    public partial class comment : Form
    {
        private task_isolate dash = null;
        public comment(Form dashForm)
        {
            dash = dashForm as task_isolate;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox3.Text == "" || this.richTextBox1.Text == "" || (this.textBox3.Text == null || this.richTextBox1.Text == null))
            {
                MessageBox.Show("Oops! Missing Something?");
            }
            else
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                    {
                        string qry = @"insert into task_comment (task_id, comment, commit_by, timestamp) values ((select id from todo where code = @code), @comment, @commit_by, CURRENT_TIMESTAMP)";
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                        cmd.Parameters.AddWithValue("@code", this.label4.Text);
                        cmd.Parameters.AddWithValue("@comment", this.richTextBox1.Text);
                        cmd.Parameters.AddWithValue("@commit_by", this.textBox3.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Comment Added!");
                        this.richTextBox1.Text = string.Empty;
                        this.textBox3.Text = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void comment_Load(object sender, EventArgs e)
        {
            //this.label4.Text = this.dash.txtName.Text;
        }
    }
}
