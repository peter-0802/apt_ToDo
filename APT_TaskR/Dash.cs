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
using System.IO;
using MaterialSkin;
using MaterialSkin.Controls;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Resources;

namespace APT_TaskR
{
    public partial class Dash : Form
    {
        public Dash()
        {
            InitializeComponent();
            
            this.comboBox1.SelectedIndex = 0;
            loadTask();
        }
        public void loadTask()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    string qry = " SELECT CODE `CODE`, TITLE `TITLE`, DESCRIPTION `DESCRIPTION`, ASSIGN_TO `ASSIGNEE` FROM TODO WHERE DONE_FLAG = @flag";
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@flag", this.comboBox1.Text);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selected = dataGridView1.Rows[index];
                label2.Text = selected.Cells[0].Value.ToString();
            }catch (Exception)
            {
                return;
            }
            
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addTask a = new addTask();
            a.ShowDialog();
            loadTask();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strPending = "update todo set done_flag = 'Done' where code = @code";
            string strDone = "update todo set done_flag = 'Pending' where code = @code";
            string messPending = "Task Marked as Done!";
            string messDone = "Task Marked as Pending!";
            string qry = null;
            string mess = null;
            if (comboBox1.Text == "Pending")
            {
                qry = strPending;
                mess = messPending;
            }
            else if (comboBox1.Text == "Done")
            {
                    validate val = new validate();
                    val.ShowDialog();

                    if (val.upflag == "1")
                        {
                            qry = strDone;
                            mess = messDone;
                        }
                    else
                        {
                            MessageBox.Show("Oops, Wrong Password");
                            return;
                        } 
            }
            else
            {
                return;
            }
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@code", label2.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(mess);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                label2.Text = "*";
                loadTask();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Pending")
            {
                button1.Text = "Mark as Done";
            }
            else if (comboBox1.Text == "Done")
            {
                button1.Text = "Mark as Pending";
            }
            else { return; }
            loadTask();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            validate val = new validate();
            val.ShowDialog();

            if (val.upflag == "1")
            {
                alterTask a = new alterTask(this);
                a.ShowDialog();
            }
            else
            {
                MessageBox.Show("Oops, Wrong Password");
                return;
            }
            loadTask();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            validate val = new validate();
            val.ShowDialog();

            if (val.upflag == "1")
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                    {
                        string qry = @"delete from todo where code = @code";
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                        cmd.Parameters.AddWithValue("@code", this.label2.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Task Deleted!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Oops, Wrong Password");
                return;
            }
            loadTask();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string dir = Directory.GetCurrentDirectory();
            System.IO.FileStream fs = new FileStream(dir + "\\" + "First PDF document.pdf", FileMode.Create);

            // Create an instance of the document class which represents the PDF document itself.  
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            // Create an instance to the PDF file by creating an instance of the PDF   
            // Writer class using the document and the filestrem in the constructor.  

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            document.AddAuthor("Author");
            document.AddCreator("Creator");
            document.AddKeywords("Keywords");
            document.AddSubject("Subject");
            document.AddTitle("Title");

            // Open the document to enable you to write to the document  
            document.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner  
            document.Add(new Paragraph("TAE!"));
            // Close the document  
            document.Close();
            // Close the writer instance  
            writer.Close();
            // Always close open filehandles explicity  
            fs.Close();
            string filename = "First PDF document.pdf";
            System.Diagnostics.Process.Start(filename);
            //System.IO.FileStream fsd = new FileStream(dir + "\\" + "First PDF document.pdf", FileMode.Truncate);

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            //    PdfWriter writer = PdfWriter.GetInstance(document, ms);
            //    document.Open();
            //    document.Add(new Paragraph("Hello World"));
            //    document.Close();
            //    writer.Close();
            //    Response.ContentType = "pdf/application";
            //    Response.AddHeader("content-disposition",
            //    "attachment;filename=First PDF document.pdf");
            //    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

            //    //string filename = "First PDF document.pdf";
            //    //System.Diagnostics.Process.Start(document);
            //}

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string filename = "First PDF document.pdf";
            System.Diagnostics.Process.Start(filename);
        }

        private void generateMasterlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            winReport a = new winReport();
            a.ShowDialog();
        }

        private void clearDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            validate val = new validate();
            val.ShowDialog();

            if (val.upflag == "1")
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(DBConn.LoadConnectionString()))
                    {
                        string qry = @"DELETE FROM todo;    
                                        DELETE FROM sqlite_sequence WHERE name = 'todo';";
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand(qry, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("DATABASE CLEARED!");
                        loadTask();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Oops, Wrong Password");
                return;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            task_isolate a = new task_isolate(this);
            a.ShowDialog();
        }

        private void Dash_Load(object sender, EventArgs e)
        {

        }
    }
}
