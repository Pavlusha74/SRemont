using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SRemont
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Vhod());
            if (Vhod.status == "running")
                Application.Run(new Form1(Vhod.userName, Vhod.dostup));  
        }
    }
}
