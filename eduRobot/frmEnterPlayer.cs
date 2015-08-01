using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eduRobot
{
    public partial class frmEnterPlayer : Form
    {
        public frmEnterPlayer()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPlayerName.Text.Trim()))
            {
                MessageBox.Show("Please Enter Your Nickname to Play a Game", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                frmMain frm = new frmMain(txtPlayerName.Text.Trim());
                frm.Show();
                this.Hide();
            }
        }
    }
}
