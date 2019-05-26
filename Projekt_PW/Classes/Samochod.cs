using System;
using System.Drawing;
using System.Threading;
using Projekt_PW.Properties;

namespace Projekt_PW.Classes

{
    public class Samochod : Form1
    {
        private Form1 _form;
        private Mutex _mutexDroga;
        private Mutex _mutexProm;
        private bool[] _miejscaDroga;
        private bool[] _miejscaProm;
        public Samochod( int miejscaDroga, int miejscaProm, int liczbaSamochodow, Form1 form)
        {
            int liczbaSamochodow1 = liczbaSamochodow;
            _form = form;
            _miejscaDroga = new bool[miejscaDroga];
            for (int i = 0; i < miejscaDroga; i++)
            {
                _miejscaDroga[i] = true;
            }
            _miejscaProm = new bool[miejscaProm];
            for (int i = 0; i < miejscaProm; i++)
            {
                _miejscaProm[i] = true;
            }
            _mutexDroga = new Mutex();
            _mutexProm = new Mutex();
            var samochod = new Thread[liczbaSamochodow1];
            for (int i = 0; i < liczbaSamochodow1; i++)
            {
                samochod[i] = new Thread(Kod_Samochodu);
                samochod[i].Name = "Samochod " + i;
                samochod[i].Start();
            }
        }

        public void Kod_Samochodu()
        {
            Random rand = new Random();
            Bitmap samochod = new Bitmap(Resources.Samochod);
            Thread.Sleep(rand.Next(800, 2000));
            Console.WriteLine(@"{0} zaczyna dzialanie", Thread.CurrentThread.Name);
            _mutexDroga.WaitOne();
            for (int i = _miejscaDroga.Length - 1; i > 0; i--)
            {
                if (_miejscaDroga[i])
                {
                    _miejscaDroga[i] = false;
                    _form.Invoke(_form.myDelegate, true, i);
                    Thread.Sleep(rand.Next(800, 2000));
                    _mutexProm.WaitOne();
                    _miejscaDroga[i] = true;
                    _form.Invoke(_form.myDelegate, false, i);
                    _mutexDroga.ReleaseMutex();
                    for (int j = _miejscaProm.Length - 1; j > 0 ; j--)
                    {
                        if (_miejscaProm[j])
                        {
                            _form.Invoke(_form.myDelegate, true, j);
                            _miejscaProm[j] = false;
                            Thread.Sleep(rand.Next(800, 2000));
                            _miejscaProm[j] = true;
                            _form.Invoke(_form.myDelegate, false, j);
                            _mutexProm.ReleaseMutex();
                            break;
                        }
                    }
                    break;
                }
            }

            Thread.Sleep(rand.Next(800, 2000));
            Console.WriteLine(@"{0} zakonczy dzialanie", Thread.CurrentThread.Name);
            Thread.CurrentThread.Interrupt();
        }
    }
}