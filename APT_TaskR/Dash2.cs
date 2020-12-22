using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SQLite;

namespace APT_TaskR
{
    public partial class Dash2 : MaterialForm
    {
        public Dash2()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.LightBlue600, Primary.LightBlue700, Primary.Red700, Accent.DeepPurple200, TextShade.WHITE);
            //this.materialListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            //this.materialListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            //this.materialListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
            loadTask();
        }

        public void loadTask()
        {

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    DataTable dt = new DataTable();
                    dt.Clear();
                    this.materialListView1.Items.Clear();
                    string query = $"SELECT CODE `CODE`, TITLE `TITLE`, DESCRIPTION `DESCRIPTION`, ASSIGN_TO `ASSIGNEE` FROM TODO";
                    SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        ListViewItem item = new ListViewItem(Convert.ToString(row["CODE"]));
                        ListViewItem.ListViewSubItem[] subitems = new ListViewItem.ListViewSubItem[]
                        {
                        new ListViewItem.ListViewSubItem(item, Convert.ToString(row["TITLE"])),
                        new ListViewItem.ListViewSubItem(item, Convert.ToString(row["DESCRIPTION"])),
                        new ListViewItem.ListViewSubItem(item, Convert.ToString(row["ASSIGNEE"]))
                        };
                        item.SubItems.AddRange(subitems);
                        this.materialListView1.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void materialListView1_Click(object sender, EventArgs e)
        {
            if (materialListView1.SelectedItems.Count > 0)
            {
                this.materialLabel1.Text = materialListView1.SelectedItems[0].SubItems[0].Text;
            }
        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            task_isolate a = new task_isolate(this);
            a.ShowDialog();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red400, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            addTask a = new addTask();
            a.ShowDialog();
            loadTask();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("asd");
        }
    }
}
