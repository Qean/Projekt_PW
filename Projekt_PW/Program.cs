using System;
using System.Threading;
using System.Windows.Forms;
using Projekt_PW.Classes;

namespace Projekt_PW
{
    internal static class Program
    {
        public static Form1 form;
        public static volatile ManualResetEvent eventHandle1;
        public static volatile ManualResetEvent eventHandle2;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            eventHandle1 = new ManualResetEvent(false);
            eventHandle2 = new ManualResetEvent(false);
            form = new Form1(eventHandle1, eventHandle2);
            var samochod = new Samochod(4, 6, 20, form, eventHandle1, eventHandle2);
            Application.Run(form);
        }
    }
}