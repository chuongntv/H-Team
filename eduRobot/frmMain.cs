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
using System.Threading;
namespace eduRobot
{
    public partial class frmMain : Form
    {
        #region Variables (Const)
        private const int ROBOT_UP = 1;
        private const int ROBOT_DOWN = 2;
        private const int ROBOT_LEFT = 3;
        private const int ROBOT_RIGHT = 4;
        private const int MAP_CANGO = 0;
        private const int MAP_ROBOT_UP = 1;
        private const int MAP_ROBOT_DOWN = 2;
        private const int MAP_ROBOT_LEFT = 3;
        private const int MAP_ROBOT_RIGHT = 4;
        private const int MAP_ROBOT_UP_LIGHT_ON = 5;
        private const int MAP_ROBOT_DOWN_LIGHT_ON = 6;
        private const int MAP_ROBOT_LEFT_LIGHT_ON = 7;
        private const int MAP_ROBOT_RIGHT_LIGHT_ON = 8;
        private const int MAP_ROBOT_UP_LIGHT_OFF = 9;
        private const int MAP_ROBOT_DOWN_LIGHT_OFF = 10;
        private const int MAP_ROBOT_LEFT_LIGHT_OFF = 11;
        private const int MAP_ROBOT_RIGHT_LIGHT_OFF = 12;
        private const int MAP_LIGHT_UP = 13;
        private const int MAP_LIGHT_ON = 14;
        private const int MAP_VIRUS = 15;
        #endregion
        #region Variables (Standard)
        private string lastPoint;
        private bool isStop = false;
        private int[] arrCmdOfMain = new int[13];
        private int[] arrCmdOfF1 = new int[9];
        private int[] arrCmdOfF2 = new int[9];
        private int[,] arrMap = new int[10, 10];
        private int[,] arrStatusMap = new int[10, 10];
        private int trendOfRobot = ROBOT_UP;
        private int xRobot = 1, yRobot = 1;
        private int xRobotBegin, yRobotBegin;
        private int numberOfLight, numberOfLightIsOn;
        private int level = 1;
        private string playerName;
        #endregion

        private void mGame1_Click(object sender, EventArgs e)
        {
            int i = 1;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame1.Image = null;
            else
                mGame1.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame2_Click(object sender, EventArgs e)
        {
            int i = 2;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame2.Image = null;
            else
                mGame2.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame3_Click(object sender, EventArgs e)
        {
            int i = 3;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame3.Image = null;
            else
                mGame3.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame4_Click(object sender, EventArgs e)
        {
            int i = 4;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame4.Image = null;
            else
                mGame4.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame5_Click(object sender, EventArgs e)
        {
            int i = 5;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame5.Image = null;
            else
                mGame5.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame6_Click(object sender, EventArgs e)
        {
            int i = 6;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame6.Image = null;
            else
                mGame6.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame7_Click(object sender, EventArgs e)
        {
            int i = 7;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame7.Image = null;
            else
                mGame7.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame8_Click(object sender, EventArgs e)
        {
            int i = 8;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame8.Image = null;
            else
                mGame8.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame9_Click(object sender, EventArgs e)
        {
            int i = 9;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame9.Image = null;
            else
                mGame9.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame10_Click(object sender, EventArgs e)
        {
            int i = 10;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame10.Image = null;
            else
                mGame10.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame11_Click(object sender, EventArgs e)
        {
            int i = 11;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame11.Image = null;
            else
                mGame11.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void mGame12_Click(object sender, EventArgs e)
        {
            int i = 12;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                mGame12.Image = null;
            else
                mGame12.Image = arrImage.Images[arrCmdOfMain[i]];
        }

        private void f1Game1_Click(object sender, EventArgs e)
        {
            int i = 1;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game1.Image = null;
            else
                f1Game1.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game2_Click(object sender, EventArgs e)
        {
            int i = 2;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game2.Image = null;
            else
                f1Game2.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game3_Click(object sender, EventArgs e)
        {
            int i = 3;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game3.Image = null;
            else
                f1Game3.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game4_Click(object sender, EventArgs e)
        {
            int i = 4;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game4.Image = null;
            else
                f1Game4.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game5_Click(object sender, EventArgs e)
        {
            int i = 5;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game5.Image = null;
            else
                f1Game5.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game6_Click(object sender, EventArgs e)
        {
            int i = 6;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game6.Image = null;
            else
                f1Game6.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game7_Click(object sender, EventArgs e)
        {
            int i = 7;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game7.Image = null;
            else
                f1Game7.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f1Game8_Click(object sender, EventArgs e)
        {
            int i = 8;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                f1Game8.Image = null;
            else
                f1Game8.Image = arrImage.Images[arrCmdOfF1[i]];
        }

        private void f2Game1_Click(object sender, EventArgs e)
        {
            int i = 1;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game1.Image = null;
            else
                f2Game1.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game2_Click(object sender, EventArgs e)
        {
            int i = 2;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game2.Image = null;
            else
                f2Game2.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game3_Click(object sender, EventArgs e)
        {
            int i = 3;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game3.Image = null;
            else
                f2Game3.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game4_Click(object sender, EventArgs e)
        {
            int i = 4;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game4.Image = null;
            else
                f2Game4.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game5_Click(object sender, EventArgs e)
        {
            int i = 5;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game5.Image = null;
            else
                f2Game5.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game6_Click(object sender, EventArgs e)
        {
            int i = 6;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game6.Image = null;
            else
                f2Game6.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game7_Click(object sender, EventArgs e)
        {
            int i = 7;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game7.Image = null;
            else
                f2Game7.Image = arrImage.Images[arrCmdOfF2[i]];
        }

        private void f2Game8_Click(object sender, EventArgs e)
        {
            int i = 8;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                f2Game8.Image = null;
            else
                f2Game8.Image = arrImage.Images[arrCmdOfF2[i]];
        }
        public frmMain()
        {
            InitializeComponent();
        }
        public frmMain(string playerName)
        {
            this.playerName = playerName;
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }
    }
}
