using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace eduRobot
{
    public partial class frmRanking : Form
    {
        private OleDbConnection conn;
        private OleDbDataReader reader;
        private OleDbCommand cmd;
        private string sqlStr;
        public frmRanking()
        {
            conn = new OleDbConnection(Program.conStr);
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRanking_Load(object sender, EventArgs e)
        {
            try
            {
                int i = 1;
                while (true)
                {
                    if (!File.Exists("Map\\" + i + ".map"))
                        break;
                    else
                    {
                        cbLevel.Items.Add("Level " + i++);
                    }

                }
                cbLevel.SelectedIndex = 0;
            }
            catch
            {

            }
        }

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {   
            try
            {
                lvRanking.Items.Clear();
                sqlStr = "SELECT TOP 10 * FROM rankinglogs WHERE levelnumber = " + cbLevel.Text.Substring(6).Trim() + "   ORDER BY Point";
                conn.Open();
                cmd = new OleDbCommand(sqlStr,conn);
                reader = cmd.ExecuteReader();
                int i = 1;
                while(reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(i+"");
                    lvi.SubItems.Add(reader["Nickname"].ToString());
                    lvi.SubItems.Add(reader["Point"].ToString());
                    lvi.SubItems.Add(reader["TimeSave"].ToString());
                    lvRanking.Items.Add(lvi);
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }     
            finally
            {
                try
                {
                    conn.Close();
                }
                catch
                {
                }
            }
           
        }
    }
}
