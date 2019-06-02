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

        private const int LiczbaBoxow = 13;
        private static Form1 _form;
        private volatile AutoResetEvent eventHandle1;
        private volatile AutoResetEvent eventHandle2;
        private volatile AutoResetEvent eventHandle3;
        private volatile int eventFlag1;
        private int[] _stopPlaces;
        private int _locDiff;
        private int _closestPicture;
        private int _direction;
        private int _promFlag;
        private readonly int _leftBank;
        private readonly int _rightBank;
        private int _x;
        private readonly PictureBox[] _arrayBox;
        private volatile int _promCapacity;
        private double _promTimer;

        public SetVisibilityDelegate MyDelegate;

        public Form1(AutoResetEvent eventHandle1, AutoResetEvent eventHandle2, AutoResetEvent eventHandle3)
        {
            MyDelegate = Set_Visibility;
            _form = this;
            this.eventHandle1 = eventHandle1;
            this.eventHandle2 = eventHandle2;
            this.eventHandle3 = eventHandle3;
            eventFlag1 = 1;
            InitializeComponent();
            _leftBank = pictureBox2.Left;
            _rightBank = 652 - 108;
            _x = pictureBox2.Left;
            _locDiff = -1000;
            _direction = 1;
            _closestPicture = 0;
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
            _arrayBox[10] = pictureBox13;
            _arrayBox[11] = pictureBox14;
            _arrayBox[12] = pictureBox15;
            _stopPlaces = new int[4];
            for (int i = 0; i < _stopPlaces.Length; i++)
            {
                _stopPlaces[i] = _arrayBox[i].Left;
            }
        }

        public void Set_Visibility(bool visibility, int index)
        {
            _arrayBox[index].Visible = visibility;
            if (index > 9)
            {
                _arrayBox[index].Location = new Point(652, _arrayBox[index].Top);
            }
            else if (index > 3 && visibility)
            {
                _promCapacity++;
            }
            else if (index > 3 && visibility == false)
            {
                _promCapacity--;
            }
            else
            {
                _arrayBox[index].Location = new Point(0, _arrayBox[index].Top);
            }
        }

        public void Set_Prom_Flag(int flag)
        {
            _promFlag = flag;
        }

        public void Set_event1_Flag(int flag)
        {
            eventFlag1 = flag;
        }

        public void Change_Direction()
        {
            if (_direction == 0)
            {
                _direction = 1;
            }
            else
            {
                _direction = 0;
            }
        }

        private void Main_Timer_Tick_1(object sender, EventArgs e)
        {
           DoubleBuffered = true;
            if (_promFlag == 0 && _x == _leftBank)
            {
                for (int i = 0; i < 4; i++)
                {
                    _locDiff = 1000;
                    _closestPicture = i;
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arrayBox[i].Visible && _arrayBox[j].Visible && _arrayBox[i].Left < _arrayBox[j].Left)
                        {
                            if (_locDiff > _arrayBox[j].Left - _arrayBox[i].Right)
                            {
                                _locDiff = _arrayBox[j].Left - _arrayBox[i].Right ;
                                _closestPicture = j;
                            }
                        }
                    }

                    if (_arrayBox[i].Visible && _arrayBox[i].Right + 10 < _arrayBox[_closestPicture].Left)
                    {
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                    }
                    else if (_arrayBox[i].Visible && _arrayBox[i].Right < _arrayBox[_closestPicture].Right)
                    {
                    }
                    else if (_locDiff == 1000 && _arrayBox[i].Visible)
                    {
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                    }
                    
                    if (_arrayBox[i].Right >= _leftBank && _promFlag == 0)
                    {
                        eventHandle1.Set();
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    _locDiff = 1000;
                    _closestPicture = i;
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arrayBox[i].Visible && _arrayBox[j].Visible && _arrayBox[i].Left < _arrayBox[j].Left)
                        {
                            if (_locDiff > _arrayBox[j].Left - _arrayBox[i].Right)
                            {
                                _locDiff = _arrayBox[j].Left - _arrayBox[i].Right ;
                                _closestPicture = j;
                            }
                        }
                    }

                    if (_arrayBox[i].Visible && _arrayBox[i].Right + 10 < _arrayBox[_closestPicture].Left)
                    {
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                    }
                    else if (_arrayBox[i].Visible && _arrayBox[i].Right < _arrayBox[_closestPicture].Right)
                    {
                    }
                    else if (_locDiff == 1000 && _arrayBox[i].Visible && _arrayBox[i].Right + 10 < _leftBank )
                    {
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                    }
                }

                for (int i = 10; i < 13; i++)
                {
                    _locDiff = 1000;
                    _closestPicture = i;
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arrayBox[i].Visible && _arrayBox[j].Visible && _arrayBox[i].Left < _arrayBox[j].Left)
                        {
                            if (_locDiff > _arrayBox[j].Left - _arrayBox[i].Right)
                            {
                                _locDiff = _arrayBox[j].Left - _arrayBox[i].Right ;
                                _closestPicture = j;
                            }
                        }
                    }

                    if (_arrayBox[i].Visible && _arrayBox[i].Right + 10 < _arrayBox[_closestPicture].Left)
                    {
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                    }
                    else if (_arrayBox[i].Visible && _arrayBox[i].Right < _arrayBox[_closestPicture].Right)
                    {
                    }else if (_arrayBox[i].Visible)
                    {
                        _arrayBox[i].Location = new Point(_arrayBox[i].Left + 4, _arrayBox[i].Top);
                        if (_arrayBox[i].Right >= 824)
                            eventHandle3.Set();
                    }
                }
            }

            if (eventFlag1 == 1 && _x == _leftBank)
            {
                eventFlag1 = 0;
            }

            if (_x >= _rightBank)
            {
                for (int i = 0; i < 6; i++)
                {
                    eventHandle2.Set();
                }

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
            PromTime.Text = "Czas do odplyniecia promu: " + (int) Math.Abs(10 - _promTimer);
        }
    }
}