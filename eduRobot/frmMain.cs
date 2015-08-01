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
