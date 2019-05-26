using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projekt_PW.Classes;
using Projekt_PW.Properties;

namespace Projekt_PW
{
    public partial class Form1 : Form
    {
        private static Form1 form = null;
        private int _leftBank;
        private int _rightBank;
        private int _x;
        private int _y;
        private int _direction;
        private const int LiczbaBoxow = 10;
        private PictureBox[] arrayBox;

        public delegate void SetVisibilityDelegate(bool visibility, int index);

        public SetVisibilityDelegate myDelegate;

        public Form1()
        {
            myDelegate = Set_Visibility;
            form = this;
            InitializeComponent();
            _leftBank = pictureBox2.Left;
            _rightBank = 651 - 108;
            _x = pictureBox2.Left;
            _y = pictureBox2.Top;
            _direction = 1;
            arrayBox = new PictureBox[LiczbaBoxow];
            arrayBox[0] = pictureBox3;
            arrayBox[1] = pictureBox4;
            arrayBox[2] = pictureBox5;
            arrayBox[3] = pictureBox6;
            arrayBox[4] = pictureBox7;
            arrayBox[5] = pictureBox8;
            arrayBox[6] = pictureBox9;
            arrayBox[7] = pictureBox10;
            arrayBox[8] = pictureBox11;
            arrayBox[9] = pictureBox12;
        }


        private void Main_Timer_Tick(object sender, EventArgs e)
        {
            if (_x <= _rightBank && _direction == 1)
            {
                _x += 4;
                if (_x >= _rightBank)
                {
                    _direction = 0;
                }
            }
            else if (_x >= _leftBank && _direction == 0)
            {
                _x -= 4;
                if (_x <= _leftBank)
                {
                    _direction = 1;
                }
            }

            /*
            if (arrayBox[0].Visible == false)
            {
                arrayBox[0].Visible = true;
            }
            else
            {
                arrayBox[0].Visible = false;
            }
            */
            for (int i = 4; i < 10; i++)
            {
                arrayBox[i].Location = new Point(_x + 10, arrayBox[i].Top);
            }
            pictureBox2.Location = new Point(_x, pictureBox2.Top);
            //Invalidate();
            //_y++;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap background = new Bitmap(Resources.Projekt_PW_rzeka);
            Bitmap prom = new Bitmap(Resources.Prom);

            //e.Graphics.DrawImage(background, pictureBox1.Bounds);
            //e.Graphics.DrawImage(prom, pictureBox2.Bounds);
        }

        public void Set_Visibility(bool visibility, int index)
        {
            arrayBox[index].Visible = visibility;
        }
    }
}