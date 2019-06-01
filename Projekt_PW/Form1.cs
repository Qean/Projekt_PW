using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Projekt_PW.Properties;

namespace Projekt_PW
{
    public partial class Form1 : Form
    {
        public delegate void SetVisibilityDelegate(bool visibility, int index);

        private const int LiczbaBoxow = 10;
        private static Form1 _form;
        private volatile ManualResetEvent eventHandle1;
        private volatile ManualResetEvent eventHandle2;
        private volatile int eventFlag1;
        private volatile int eventFlag2;
        private int _direction;
        private int _promFlag;
        private readonly int _leftBank;
        private readonly int _rightBank;
        private int _x;
        private readonly PictureBox[] _arrayBox;
        private volatile int _promCapacity;
        private double _promTimer;

        public SetVisibilityDelegate MyDelegate;

        public Form1(ManualResetEvent eventHandle1, ManualResetEvent eventHandle2)
        {
            MyDelegate = Set_Visibility;
            _form = this;
            this.eventHandle1 = eventHandle1;
            this.eventHandle2 = eventHandle2;
            eventFlag1 = 1;
            eventFlag2 = 1;
            InitializeComponent();
            _leftBank = pictureBox2.Left;
            _rightBank = 652 - 108;
            _x = pictureBox2.Left;
            _direction = 1;
            _promFlag = 0;
            _promCapacity = 0;
            _promTimer = 0;
            _arrayBox = new PictureBox[LiczbaBoxow];
            _arrayBox[0] = pictureBox3;
            _arrayBox[1] = pictureBox4;
            _arrayBox[2] = pictureBox5;
            _arrayBox[3] = pictureBox6;
            _arrayBox[4] = pictureBox7;
            _arrayBox[5] = pictureBox8;
            _arrayBox[6] = pictureBox9;
            _arrayBox[7] = pictureBox10;
            _arrayBox[8] = pictureBox11;
            _arrayBox[9] = pictureBox12;
        }


        private void Main_Timer_Tick(object sender, EventArgs e)
        {
            if (eventFlag1 == 1 && _x == _leftBank)
            {
                eventHandle1.Set();
                eventFlag1 = 0;
            }

            if (eventFlag2 == 1 && _x == _rightBank)
            {
                eventHandle2.Set();
                eventFlag2 = 0;
            }

            if (_x == _leftBank)
            {
                _promTimer += 0.1;
                if (_promCapacity == 0 && _promTimer >= 10)
                {
                    _promTimer = 0;
                }
                else if (_promCapacity >= 0 && _promTimer >= 10)
                {
                    _promFlag = 1;
                    _promTimer = 0;
                    eventHandle1.Reset();
                }
                else if (_promCapacity == 6)
                {
                    _promFlag = 1;
                    _promTimer = 0;
                    eventHandle1.Reset();
                }
            }

            if (_promFlag == 1 || _direction == 0)
            {
                if (_x <= _rightBank && _direction == 1)
                {
                    for (var i = 4; i < 10; i++)
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                    _x += 4;
                }
                else if (_x >= _leftBank && _direction == 0)
                {
                    if (_x == _leftBank)
                    {
                        _direction = 1;
                    }
                    else
                    {
                        for (var i = 4; i < 10; i++)
                            _arrayBox[i].Location = new Point(_arrayBox[i].Left - 4, _arrayBox[i].Top);
                        _x -= 4;
                    }
                }
                pictureBox2.Location = new Point(_x, pictureBox2.Top);
            }
        }

        public void Set_Visibility(bool visibility, int index)
        {
            _arrayBox[index].Visible = visibility;
            if (index > 3 && visibility)
                _promCapacity++;
            else if (index > 3 && visibility == false)
                _promCapacity--;
        }

        public int Get_Prom_Flag()
        {
            return _promFlag;
        }

        public int Get_Direction_Flag()
        {
            return _direction;
        }

        public void Set_Prom_Flag(int flag)
        {
            _promFlag = flag;
        }
        
        public void Set_event1_Flag(int flag)
        {
            eventFlag1 = flag;
        }
        
        public void Set_event2_Flag(int flag)
        {
            eventFlag2 = flag;
        }

        public void Change_Direction()
        {
            if (_direction == 0)
                _direction = 1;
            else
                _direction = 0;
        }
    }
}