using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APT_TaskR
{
    public partial class alterTask : Form
    {
        private Dash dash = null;
        public alterTask(Form dashForm)
        {
            dash = dashForm as Dash;
            InitializeComponent();
        }
        
        private void alterTask_Load(object sender, EventArgs e)
        {
            this.label4.Text = this.dash.label2.Text;
        }
        public void loadTask()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    string qry = @"SELECT CODE `CODE`, TITLE `TITLE`, DESCRIPTION `DESCRIPTION`, ASSIGN_TO `ASSIGNEE` FROM TODO WHERE code = @code";
                    SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@code", this.label4.Text);
                    SQLiteDataReader myreader;
                    conn.Open();
                    myreader = cmd.ExecuteReader();
                    while (myreader.Read())
                    {
                        this.textBox1.Text = myreader.GetString(myreader.GetOrdinal("TITLE"));
                        this.richTextBox1.Text = myreader.GetString(myreader.GetOrdinal("DESCRIPTION"));
                        this.textBox3.Text = myreader.GetString(myreader.GetOrdinal("ASSIGNEE"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void label4_TextChanged(object sender, EventArgs e)
        {
            loadTask();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    string qry = @"update todo set title = @title, description = @desc, assign_to = @assignee where code = @code";
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@title", this.textBox1.Text);
                    cmd.Parameters.AddWithValue("@desc", this.richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@assignee", this.textBox3.Text);
                    cmd.Parameters.AddWithValue("@code", this.label4.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Task Altered!");
                    this.textBox1.Text = string.Empty;
                    this.richTextBox1.Text = string.Empty;
                    this.textBox3.Text = string.Empty;
                    this.label4.Text = "*";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
