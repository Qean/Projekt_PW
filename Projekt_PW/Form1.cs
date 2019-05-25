using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_PW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.Aqua, 30, 30, 30, 30);
            e.Graphics.DrawImage(global::Projekt_PW.Properties.Resources.Projekt_PW_rzeka, 30, 30, 30, 30);
        }
    }
}