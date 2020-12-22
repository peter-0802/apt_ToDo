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
using MaterialSkin.Controls;
using MaterialSkin.Animations;
using MaterialSkin;
namespace APT_TaskR
{
    public partial class addTask : MaterialForm
    {
        public addTask()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.LightBlue600, Primary.LightBlue700, Primary.Red700, Accent.DeepPurple200, TextShade.WHITE);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }
        
        private void addTask_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void addTask_Load(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    string qry = @"insert into todo (code, title, description, assign_to, done_flag) values
                                 ((select
                                    CASE
                                        WHEN count(id) <= 0
                                        THEN 'Task - 1'
                                        ELSE 'Task - ' || (max(id) + 1)
                                    END CODE
                                 from todo), @title, @desc, @assignee, @flag)";
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@title", this.materialSingleLineTextField1.Text);
                    cmd.Parameters.AddWithValue("@desc", this.richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@assignee", this.materialSingleLineTextField2.Text);
                    cmd.Parameters.AddWithValue("@flag", "Pending");
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Task Added");
                    this.materialSingleLineTextField1.Text = string.Empty;
                    this.richTextBox1.Text = string.Empty;
                    this.materialSingleLineTextField2.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
