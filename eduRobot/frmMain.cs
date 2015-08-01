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
using System.Data;
using System.Data.OleDb;
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
        private OleDbConnection conn;
        private OleDbDataReader reader;
        private OleDbCommand cmd;
        private string sqlStr;
        #endregion

        #region Methods
        private int checkStatusOfRobot(int xRobot, int yRobot)
        {
            if (arrStatusMap[xRobot, yRobot] == MAP_CANGO)
            {
                if (trendOfRobot == ROBOT_UP)
                    return MAP_ROBOT_UP;
                else if (trendOfRobot == ROBOT_DOWN)
                    return MAP_ROBOT_DOWN;
                else if (trendOfRobot == ROBOT_LEFT)
                    return MAP_ROBOT_LEFT;
                else if (trendOfRobot == ROBOT_RIGHT)
                    return MAP_ROBOT_RIGHT;
            }
            else if (arrStatusMap[xRobot, yRobot] == MAP_LIGHT_UP)
            {
                if (trendOfRobot == ROBOT_UP)
                    return MAP_ROBOT_UP_LIGHT_ON;
                else if (trendOfRobot == ROBOT_DOWN)
                    return MAP_ROBOT_DOWN_LIGHT_ON;
                else if (trendOfRobot == ROBOT_LEFT)
                    return MAP_ROBOT_LEFT_LIGHT_ON;
                else if (trendOfRobot == ROBOT_RIGHT)
                    return MAP_ROBOT_RIGHT_LIGHT_ON;
            }
            else if (arrStatusMap[xRobot, yRobot] == MAP_LIGHT_ON)
            {
                if (trendOfRobot == ROBOT_UP)
                    return MAP_ROBOT_UP_LIGHT_OFF;
                else if (trendOfRobot == ROBOT_DOWN)
                    return MAP_ROBOT_DOWN_LIGHT_OFF;
                else if (trendOfRobot == ROBOT_LEFT)
                    return MAP_ROBOT_LEFT_LIGHT_OFF;
                else if (trendOfRobot == ROBOT_RIGHT)
                    return MAP_ROBOT_RIGHT_LIGHT_OFF;
            }
            return 0;
        }
        private void checkGoStraight()
        {
            if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
            {
                arrStatusMap[xRobot, yRobot] = MAP_CANGO;
            }
            else if (arrMap[xRobot, yRobot] == MAP_LIGHT_ON)
            {
                if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_UP_LIGHT_ON || arrStatusMap[xRobot, yRobot] == MAP_ROBOT_DOWN_LIGHT_ON || arrStatusMap[xRobot, yRobot] == MAP_ROBOT_LEFT_LIGHT_ON || arrStatusMap[xRobot, yRobot] == MAP_ROBOT_RIGHT_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_LIGHT_UP;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_LIGHT_ON;
            }
        }
        private void goStraight()
        {

            if (trendOfRobot == ROBOT_LEFT)
            {
                if (arrStatusMap[xRobot, yRobot - 1] != MAP_VIRUS && yRobot > 1)
                {
                    checkGoStraight();
                    arrStatusMap[xRobot, yRobot - 1] = checkStatusOfRobot(xRobot, --yRobot);
                }
            }

            else if (trendOfRobot == ROBOT_RIGHT)
            {
                if (arrStatusMap[xRobot, yRobot + 1] != MAP_VIRUS && yRobot < 8)
                {
                    checkGoStraight();
                    arrStatusMap[xRobot, yRobot + 1] = checkStatusOfRobot(xRobot, ++yRobot);
                }
            }
            else if (trendOfRobot == ROBOT_UP)
            {

                if (arrStatusMap[xRobot - 1, yRobot] != MAP_VIRUS && xRobot > 1)
                {
                    checkGoStraight();
                    arrStatusMap[xRobot - 1, yRobot] = checkStatusOfRobot(--xRobot, yRobot);
                }
            }
            else if (trendOfRobot == ROBOT_DOWN)
            {
                if (arrStatusMap[xRobot + 1, yRobot] != MAP_VIRUS && xRobot < 8)
                {
                    checkGoStraight();
                    arrStatusMap[xRobot + 1, yRobot] = checkStatusOfRobot(++xRobot, yRobot);
                }
            }
        }
        private void turnLeft()
        {
            if (trendOfRobot == ROBOT_UP)
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_UP_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT_LIGHT_OFF;
                trendOfRobot = ROBOT_LEFT;
            }
            else if (trendOfRobot == ROBOT_LEFT)
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_LEFT_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN_LIGHT_OFF;
                trendOfRobot = ROBOT_DOWN;
            }
            else if (trendOfRobot == ROBOT_DOWN)
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_DOWN_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT_LIGHT_OFF;
                trendOfRobot = ROBOT_RIGHT;
            }
            else
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_RIGHT_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP_LIGHT_OFF;
                trendOfRobot = ROBOT_UP;
            }
        }
        private void turnRight()
        {
            if (trendOfRobot == ROBOT_UP)
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_UP_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT_LIGHT_OFF;
                trendOfRobot = ROBOT_RIGHT;
            }
            else if (trendOfRobot == ROBOT_LEFT)
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_LEFT_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP_LIGHT_OFF;
                trendOfRobot = ROBOT_UP;
            }
            else if (trendOfRobot == ROBOT_DOWN)
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_DOWN_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT_LIGHT_OFF;
                trendOfRobot = ROBOT_LEFT;
            }
            else
            {
                if (arrMap[xRobot, yRobot] == MAP_CANGO || arrMap[xRobot, yRobot] == MAP_ROBOT_UP)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN;
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_RIGHT_LIGHT_ON)
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN_LIGHT_ON;
                else
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN_LIGHT_OFF;
                trendOfRobot = ROBOT_DOWN;
            }
        }
        private void resetAll()
        {
            for (int i = 0; i < 13; i++)
                arrCmdOfMain[i] = 0;
            for (int i = 0; i < 9; i++)
            {
                arrCmdOfF1[i] = 0;
                arrCmdOfF2[i] = 0;
            }
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    arrStatusMap[i, j] = arrMap[i, j];
        }
        private void loadLevel(int level)
        {
            if (File.Exists("Map\\" + level + ".map"))
            {
                if (level != 1)
                {                    
                    try
                    {
                        conn.Open();
                        sqlStr = "INSERT INTO rankinglogs(nickname,levelnumber,point,timesave) VALUES ('" + this.playerName + "','" + (level-1) + "','" + (int.Parse(lblPoint.Text) - int.Parse(lastPoint)) + "','" + DateTime.Now.ToString() + "')";
                        cmd = new OleDbCommand(sqlStr, conn);
                        cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Have some problem, so your point can't save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    lastPoint = lblPoint.Text;
                    MessageBox.Show("You are complete " + (level - 1) + ". You will be redirect to level " + level);
                }

                resetAll();
                var pics = this.Controls
                                        .OfType<PictureBox>().OrderBy(b => b.Name)
                                        .Where(b => b.Name.Substring(0, 5).Equals("mGame"));

                foreach (var pic in pics)
                {
                    pic.Image = null;
                }
                pics = this.Controls
                                        .OfType<PictureBox>().OrderBy(b => b.Name)
                                        .Where(b => b.Name.Substring(0, 5).Equals("f1Gam"));

                foreach (var pic in pics)
                {
                    pic.Image = null;
                }
                pics = this.Controls
                                        .OfType<PictureBox>().OrderBy(b => b.Name)
                                        .Where(b => b.Name.Substring(0, 5).Equals("f2Gam"));

                foreach (var pic in pics)
                {
                    pic.Image = null;
                }
                trendOfRobot = ROBOT_UP;
                numberOfLight = 0;
                numberOfLightIsOn = 0;
                StreamReader sr = File.OpenText("Map\\" + level + ".map");
                string input = null;
                input = sr.ReadLine();
                xRobot = input[0] - 48;
                xRobotBegin = xRobot;
                yRobot = input[2] - 48;
                yRobotBegin = yRobot;
                for (int i = 1; i <= 8; i++)
                {
                    input = sr.ReadLine();
                    string[] s = input.Split(' ');
                    for (int j = 1; j <= 8; j++)
                    {
                        arrMap[i, j] = int.Parse(s[j - 1]);
                        arrStatusMap[i, j] = arrMap[i, j];
                        if (arrMap[i, j] == MAP_LIGHT_ON)
                            numberOfLight++;
                    }
                }
                sr.Close();
                updateMap();
                if (this.btnPlay.InvokeRequired)
                {
                    btnPlay.Invoke(new MethodInvoker(delegate { btnPlay.Enabled = true; }));
                }
                else
                {
                    btnPlay.Enabled = true;
                }
                if (this.btnStop.InvokeRequired)
                {
                    btnStop.Invoke(new MethodInvoker(delegate { btnStop.Enabled = false; }));
                }
                else
                {
                    btnStop.Enabled = false;
                }
                if (this.btnClear.InvokeRequired)
                {
                    btnClear.Invoke(new MethodInvoker(delegate { btnClear.Enabled = true; }));
                }
                else
                {
                    btnClear.Enabled = true;
                }
            }
            else
            {
                if (this.btnPlay.InvokeRequired)
                {
                    btnPlay.Invoke(new MethodInvoker(delegate { btnPlay.Enabled = false; }));
                }
                else
                {
                    btnPlay.Enabled = false;
                }
                if (this.btnStop.InvokeRequired)
                {
                    btnStop.Invoke(new MethodInvoker(delegate { btnStop.Enabled = false; }));
                }
                else
                {
                    btnStop.Enabled = false;
                }
                if (this.btnClear.InvokeRequired)
                {
                    btnClear.Invoke(new MethodInvoker(delegate { btnClear.Enabled = false; }));
                }
                else
                {
                    btnClear.Enabled = false;
                }
                MessageBox.Show("Congratulation! You are finish game with " + lblPoint.Text, " command(s).\nPlease visit homepage to get more map.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void processLight()
        {

            if (arrMap[xRobot, yRobot] == MAP_LIGHT_ON)
            {
                if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_UP_LIGHT_OFF)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP_LIGHT_ON;
                    numberOfLightIsOn++;
                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_DOWN_LIGHT_OFF)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN_LIGHT_ON;
                    numberOfLightIsOn++;
                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_LEFT_LIGHT_OFF)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT_LIGHT_ON;
                    numberOfLightIsOn++;
                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_RIGHT_LIGHT_OFF)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT_LIGHT_ON;
                    numberOfLightIsOn++;
                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_DOWN_LIGHT_ON)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_DOWN_LIGHT_OFF;
                    numberOfLightIsOn++;

                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_UP_LIGHT_ON)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_UP_LIGHT_OFF;
                    numberOfLightIsOn--;
                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_LEFT_LIGHT_ON)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_LEFT_LIGHT_OFF;
                    numberOfLightIsOn--;
                }
                else if (arrStatusMap[xRobot, yRobot] == MAP_ROBOT_RIGHT_LIGHT_ON)
                {
                    arrStatusMap[xRobot, yRobot] = MAP_ROBOT_RIGHT_LIGHT_OFF;
                    numberOfLightIsOn--;
                }
            }
        }
        private void callF1()
        {
            for (int i = 1; i <= 8; i++)
            {
                if (arrCmdOfF1[i] == 0)
                    break;
                bool isDo = false;
                if (arrCmdOfF1[i] == 1)
                {
                    goStraight();
                    isDo = true;
                }
                if (arrCmdOfF1[i] == 2)
                {
                    turnLeft();
                    isDo = true;
                }
                if (arrCmdOfF1[i] == 3)
                {
                    turnRight();
                    isDo = true;
                }
                if (arrCmdOfF1[i] == 4)
                {
                    processLight();
                    isDo = true;
                }
                if (arrCmdOfF1[i] == 5)
                {
                    callF1();
                    isDo = true;
                }
                if (arrCmdOfF1[i] == 6)
                {
                    callF2();
                    isDo = true;
                }
                if (isDo)
                {
                    if (this.lblPoint.InvokeRequired)
                    {
                        lblPoint.Invoke(new MethodInvoker(delegate { lblPoint.Text = (int.Parse(lblPoint.Text) + 1) + ""; }));
                    }
                    else
                    {
                        lblPoint.Text = (int.Parse(lblPoint.Text) + 1) + "";
                    }
                }

                updateMap();
                if (isStop == true)
                {
                    isStop = false;
                    return;
                }
                Thread.Sleep(300);
            }



        }
        private void callF2()
        {
            for (int i = 1; i <= 8; i++)
            {
                if (arrCmdOfF2[i] == 0)
                    break;
                bool isDo = false;
                if (arrCmdOfF2[i] == 1)
                {
                    goStraight();
                    isDo = true;
                }
                if (arrCmdOfF2[i] == 2)
                {
                    turnLeft();
                    isDo = true;
                }
                if (arrCmdOfF2[i] == 3)
                {
                    turnRight();
                    isDo = true;
                }
                if (arrCmdOfF2[i] == 4)
                {
                    processLight();
                    isDo = true;
                }
                if (arrCmdOfF2[i] == 5)
                {
                    callF1();
                    isDo = true;
                }
                if (arrCmdOfF2[i] == 6)
                {
                    callF2();
                    isDo = true;
                }
                if (isDo)
                {
                    if (this.lblPoint.InvokeRequired)
                    {
                        lblPoint.Invoke(new MethodInvoker(delegate { lblPoint.Text = (int.Parse(lblPoint.Text) + 1) + ""; }));
                    }
                    else
                    {
                        lblPoint.Text = (int.Parse(lblPoint.Text) + 1) + "";
                    }
                }

                updateMap();
                if (isStop == true)
                {
                    isStop = false;
                    return;
                }
                Thread.Sleep(300);
            }
        }
        private void updateMap()
        {
            int i = 1, j = 1;
            var pics = this.Controls
                                    .OfType<PictureBox>().OrderBy(b => b.Name)
                                    .Where(b => b.Name.Substring(0, 5).Equals("gGame"));

            foreach (var pic in pics)
            {
                if (arrStatusMap[i, j] == 0)
                    pic.Image = null;
                else
                    pic.Image = arrImageOfMap.Images[arrStatusMap[i, j]];
                j++;
                if (j > 8)
                {
                    j = 1;
                    i++;
                }
            }
        }
        private void setCommandOfMainToArray(int index, ref PictureBox pic)
        {
            int i = index;
            arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            if (arrCmdOfMain[i] == 0)
                pic.Image = null;
            else
                pic.Image = arrImage.Images[arrCmdOfMain[i]];
        }
        private void setCommandOfF1ToArray(int index, ref PictureBox pic)
        {
            int i = index;
            arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            if (arrCmdOfF1[i] == 0)
                pic.Image = null;
            else
                pic.Image = arrImage.Images[arrCmdOfF1[i]];
        }
        private void setCommandOfF2ToArray(int index, ref PictureBox pic)
        {
            int i = index;
            arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            if (arrCmdOfF2[i] == 0)
                pic.Image = null;
            else
                pic.Image = arrImage.Images[arrCmdOfF2[i]];
        }
        #endregion

        private void mGame1_Click(object sender, EventArgs e)
        {
            //int i = 1;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame1.Image = null;
            //else
            //    mGame1.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(1, ref mGame1);
        }

        private void mGame2_Click(object sender, EventArgs e)
        {
            //int i = 2;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame2.Image = null;
            //else
            //    mGame2.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(2, ref mGame2);
        }

        private void mGame3_Click(object sender, EventArgs e)
        {
            //int i = 3;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame3.Image = null;
            //else
            //    mGame3.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(3, ref mGame3);
        }

        private void mGame4_Click(object sender, EventArgs e)
        {
            //int i = 4;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame4.Image = null;
            //else
            //    mGame4.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(4, ref mGame4);
        }

        private void mGame5_Click(object sender, EventArgs e)
        {
            //int i = 5;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame5.Image = null;
            //else
            //    mGame5.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(5, ref mGame5);
        }

        private void mGame6_Click(object sender, EventArgs e)
        {
            //int i = 6;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame6.Image = null;
            //else
            //    mGame6.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(6, ref mGame6);
        }

        private void mGame7_Click(object sender, EventArgs e)
        {
            //int i = 7;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame7.Image = null;
            //else
            //    mGame7.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(7, ref mGame7);
        }

        private void mGame8_Click(object sender, EventArgs e)
        {
            //int i = 8;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame8.Image = null;
            //else
            //    mGame8.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(8, ref mGame8);
        }

        private void mGame9_Click(object sender, EventArgs e)
        {
            //int i = 9;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame9.Image = null;
            //else
            //    mGame9.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(9, ref mGame9);
        }

        private void mGame10_Click(object sender, EventArgs e)
        {
            //int i = 10;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame10.Image = null;
            //else
            //    mGame10.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(10, ref mGame10);
        }

        private void mGame11_Click(object sender, EventArgs e)
        {
            //int i = 11;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame11.Image = null;
            //else
            //    mGame11.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(11, ref mGame11);
        }

        private void mGame12_Click(object sender, EventArgs e)
        {
            //int i = 12;
            //arrCmdOfMain[i] = (arrCmdOfMain[i] + 1) % 7;
            //if (arrCmdOfMain[i] == 0)
            //    mGame12.Image = null;
            //else
            //    mGame12.Image = arrImage.Images[arrCmdOfMain[i]];
            setCommandOfMainToArray(12, ref mGame12);
        }

        private void f1Game1_Click(object sender, EventArgs e)
        {
            //int i = 1;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game1.Image = null;
            //else
            //    f1Game1.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(1, ref f1Game1);
        }

        private void f1Game2_Click(object sender, EventArgs e)
        {
            //int i = 2;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game2.Image = null;
            //else
            //    f1Game2.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(2, ref f1Game2);
        }

        private void f1Game3_Click(object sender, EventArgs e)
        {
            //int i = 3;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game3.Image = null;
            //else
            //    f1Game3.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(3, ref f1Game3);
        }

        private void f1Game4_Click(object sender, EventArgs e)
        {
            //int i = 4;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game4.Image = null;
            //else
            //    f1Game4.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(4, ref f1Game4);
        }

        private void f1Game5_Click(object sender, EventArgs e)
        {
            //int i = 5;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game5.Image = null;
            //else
            //    f1Game5.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(5, ref f1Game5);
        }

        private void f1Game6_Click(object sender, EventArgs e)
        {
            //int i = 6;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game6.Image = null;
            //else
            //    f1Game6.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(6, ref f1Game6);
        }

        private void f1Game7_Click(object sender, EventArgs e)
        {
            //int i = 7;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game7.Image = null;
            //else
            //    f1Game7.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(7, ref f1Game7);
        }

        private void f1Game8_Click(object sender, EventArgs e)
        {
            //int i = 8;
            //arrCmdOfF1[i] = (arrCmdOfF1[i] + 1) % 7;
            //if (arrCmdOfF1[i] == 0)
            //    f1Game8.Image = null;
            //else
            //    f1Game8.Image = arrImage.Images[arrCmdOfF1[i]];
            setCommandOfF1ToArray(8, ref f1Game8);
        }

        private void f2Game1_Click(object sender, EventArgs e)
        {
            //int i = 1;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game1.Image = null;
            //else
            //    f2Game1.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(1, ref f2Game1);
        }

        private void f2Game2_Click(object sender, EventArgs e)
        {
            //int i = 2;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game2.Image = null;
            //else
            //    f2Game2.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(2, ref f2Game2);
        }

        private void f2Game3_Click(object sender, EventArgs e)
        {
            //int i = 3;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game3.Image = null;
            //else
            //    f2Game3.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(3, ref f2Game3);
        }

        private void f2Game4_Click(object sender, EventArgs e)
        {
            //int i = 4;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game4.Image = null;
            //else
            //    f2Game4.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(4, ref f2Game4);
        }

        private void f2Game5_Click(object sender, EventArgs e)
        {
            //int i = 5;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game5.Image = null;
            //else
            //    f2Game5.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(5, ref f2Game5);
        }

        private void f2Game6_Click(object sender, EventArgs e)
        {
            //int i = 6;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game6.Image = null;
            //else
            //    f2Game6.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(6, ref f2Game6);
        }

        private void f2Game7_Click(object sender, EventArgs e)
        {
            //int i = 7;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game7.Image = null;
            //else
            //    f2Game7.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(7, ref f2Game7);
        }

        private void f2Game8_Click(object sender, EventArgs e)
        {
            //int i = 8;
            //arrCmdOfF2[i] = (arrCmdOfF2[i] + 1) % 7;
            //if (arrCmdOfF2[i] == 0)
            //    f2Game8.Image = null;
            //else
            //    f2Game8.Image = arrImage.Images[arrCmdOfF2[i]];
            setCommandOfF2ToArray(8, ref f2Game8);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                trendOfRobot = ROBOT_UP;
                isStop = false;
                numberOfLightIsOn = 0;
                lastPoint = lblPoint.Text;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnClear.Enabled = false;
                bgwRun.RunWorkerAsync();
            }
            catch
            {

            }
        }

        private void bgwRun_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (arrCmdOfMain[i] == 0)
                    break;
                bool isDo = false;
                if (arrCmdOfMain[i] == 1)
                {
                    goStraight();
                    isDo = true;
                }
                if (arrCmdOfMain[i] == 2)
                {
                    turnLeft();
                    isDo = true;
                }
                if (arrCmdOfMain[i] == 3)
                {
                    turnRight();
                    isDo = true;
                }
                if (arrCmdOfMain[i] == 4)
                {
                    processLight();
                    isDo = true;
                }
                if (arrCmdOfMain[i] == 5)
                {
                    callF1();
                    isDo = true;
                }
                if (arrCmdOfMain[i] == 6)
                {
                    callF2();
                    isDo = true;
                }
                if (isDo)
                {
                    if (this.lblPoint.InvokeRequired)
                    {
                        lblPoint.Invoke(new MethodInvoker(delegate { lblPoint.Text = (int.Parse(lblPoint.Text) + 1) + ""; }));
                    }
                    else
                    {
                        lblPoint.Text = (int.Parse(lblPoint.Text) + 1) + "";
                    }
                }

                updateMap();
                if (isStop == true)
                {
                    isStop = false;
                    return;
                }
                Thread.Sleep(300);
            }
            if (numberOfLight == numberOfLightIsOn)
            {
                loadLevel(++level);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lblPoint.Text = lastPoint;
            btnPlay.Enabled = true;
            btnStop.Enabled = false;
            btnClear.Enabled = true;
            xRobot = xRobotBegin;
            yRobot = yRobotBegin;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    arrStatusMap[i, j] = arrMap[i, j];
            updateMap();
            isStop = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblPoint.Text = lastPoint;
            xRobot = xRobotBegin;
            yRobot = yRobotBegin;
            resetAll();
            updateMap();
            var pics = this.Controls
                                    .OfType<PictureBox>().OrderBy(b => b.Name)
                                    .Where(b => b.Name.Substring(0, 5).Equals("mGame"));

            foreach (var pic in pics)
            {
                pic.Image = null;
            }
            pics = this.Controls
                                    .OfType<PictureBox>().OrderBy(b => b.Name)
                                    .Where(b => b.Name.Substring(0, 5).Equals("f1Gam"));

            foreach (var pic in pics)
            {
                pic.Image = null;
            }
            pics = this.Controls
                                    .OfType<PictureBox>().OrderBy(b => b.Name)
                                    .Where(b => b.Name.Substring(0, 5).Equals("f2Gam"));

            foreach (var pic in pics)
            {
                pic.Image = null;
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void btnRank_Click(object sender, EventArgs e)
        {
            frmRanking frm = new frmRanking();
            frm.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public frmMain()
        {
            conn = new OleDbConnection(Program.conStr);
            InitializeComponent();
        }
        public frmMain(string playerName)
        {
            conn = new OleDbConnection(Program.conStr);
            this.playerName = playerName;
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            loadLevel(1);
        }
    }
}
