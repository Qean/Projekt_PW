using System;
using System.Threading;
using System.Windows.Forms;
using Projekt_PW.Classes;

namespace Projekt_PW
{
    internal static class Program
    {
        public static Form1 form;
        public static volatile AutoResetEvent eventHandle1;
        public static volatile AutoResetEvent eventHandle2;
        public static volatile AutoResetEvent eventHandle3;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            eventHandle1 = new AutoResetEvent(false);
            eventHandle2 = new AutoResetEvent(false);
            eventHandle3 = new AutoResetEvent(false);
            form = new Form1(eventHandle1, eventHandle2, eventHandle3);
            var samochod = new Samochod(4, 6, 20, form, eventHandle1, eventHandle2, eventHandle3);
            form.GetSamochod(samochod);
            Application.Run(form);
            
        }
    }
}