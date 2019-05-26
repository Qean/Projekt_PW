using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projekt_PW.Classes;

namespace Projekt_PW
{
    static class Program
    {
        public static Form1 form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            Samochod samochod = new Samochod(4, 6, 1, form);
            Application.Run(form);
        }
    }
}