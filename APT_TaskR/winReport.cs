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
    public partial class winReport : Form
    {
        public winReport()
        {
            InitializeComponent();
        }
        public void bindreport()
        {
            datasetReport ds = new datasetReport();
            using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
            {
                string qry = "SELECT code, title, description, assign_to, done_flag FROM TODO";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds, ds.Tables["todo"].TableName);
                reportTemplate crt = new reportTemplate();
                crt.SetDataSource(ds);
                this.crystalReportViewer1.ReportSource = crt;
            }
        }
        private void winReport_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
