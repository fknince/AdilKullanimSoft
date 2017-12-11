using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GZSProjesi
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 fm1 = new Form1();
            fm1.StartPosition=FormStartPosition.CenterScreen;
            fm1.ControlBox = false;
            fm1.FormBorderStyle = FormBorderStyle.None;
            Application.Run(fm1);
        }
    }
}
