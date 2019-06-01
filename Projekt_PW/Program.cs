using System;
using System.Windows.Forms;
using Projekt_PW.Classes;

namespace Projekt_PW
{
    internal static class Program
    {
        public static Form1 form;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            var samochod = new Samochod(4, 6, 6, form);
            Application.Run(form);
        }
    }
}