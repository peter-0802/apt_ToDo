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
    
    public partial class task_isolate : Form
    {
        private Dash2 dash = null;
        public task_isolate(Form dashForm)
        {
            dash = dashForm as Dash2;
            InitializeComponent();
        }
        public void loadComments()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    string qry = @"select
                                    timestamp `TIMESTAMP`, 
                                    comment `COMMENT`,
                                    commit_by `COMMENT BY`
                                    from todo
                                    inner join task_comment on task_comment.task_id = todo.id
                                    where code = @code
                                    order by timestamp desc";
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@code", this.txtName.Text);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = dataTable;
                    dataGridView1.DataSource = bindingSource;
                    adapter.Update(dataTable);
                    this.dataGridView1.Rows[0].Cells[0].Selected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void task_isolate_Load(object sender, EventArgs e)
        {
            Dash a = new Dash();
            this.txtName.Text = a.label2.Text;
            loadComments();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comment a = new comment(this);
            a.ShowDialog();
            loadComments();
        }
    }
}
