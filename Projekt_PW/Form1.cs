using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Projekt_PW.Properties;

namespace Projekt_PW
{
    public partial class Form1 : Form
    {
        private int _x;
        private int _y;
        private int direction;

        public Form1()
        {
            InitializeComponent();
            _x = 220;
            _y = 131;
            direction = 1;
        }


        private void Main_Timer_Tick(object sender, EventArgs e)
        {
            //DoubleBuffered = true;
            if (_x <= 650 - 106 && direction == 1)
            {
                _x += 4;
                if (_x >= 650 - 106)
                {
                    direction = 0;
                }
            }
            else if (_x >= 220 && direction == 0)
            {
                _x -= 4;
                if (_x <= 220)
                {
                    direction = 1;
                }
            }
            pictureBox2.Location = new Point(_x, _y);
            //_y++;
        }
    }
}