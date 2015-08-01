using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eduRobot
{
    static class Program
    {
        public static string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\eduRobot.data;User Id=admin;Password=;";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
       
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmEnterPlayer());
        }
    }
}
