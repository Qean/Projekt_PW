using System;
using System.Drawing;
using System.Threading;
using Projekt_PW.Properties;

namespace Projekt_PW.Classes

{
    public class Samochod : Form1
    {
        private readonly Form1 _form;
        private readonly Mutex _mutexDroga;
        private readonly Mutex _mutexProm;
        private readonly Semaphore _semaphoreDroga;
        private readonly Semaphore _semaphoreProm;
        private volatile int _drogaCount;
        private volatile int _promCount;
        private volatile bool[] testDroga;
        private volatile bool[] testProm;
        static ThreadLocal<int> _drogaMiejsce = new ThreadLocal<int>( () => 0 );
        static ThreadLocal<int> _promMiejsce = new ThreadLocal<int>( () => 0 );

        public Samochod(int miejscaDroga, int miejscaProm, int liczbaSamochodow, Form1 form)
        {
            testDroga = new bool[miejscaDroga];
            for (int i = 0; i < miejscaDroga; i++)
            {
                testDroga[i] = true;
            }
            testProm = new bool[miejscaProm];
            for (int i = 0; i < miejscaProm; i++)
            {
                testProm[i] = true;
            }
            var liczbaSamochodow1 = liczbaSamochodow;
            _form = form;
            _semaphoreDroga = new Semaphore(4, 4);
            _semaphoreProm = new Semaphore(6, 6);
            _mutexDroga = new Mutex();
            _mutexProm = new Mutex();
            _drogaCount = 3;
            _promCount = 0;
            var samochod = new Thread[liczbaSamochodow1];
            for (var i = 0; i < liczbaSamochodow1; i++)
            {
                samochod[i] = new Thread(Kod_Samochodu);
                samochod[i].Name = "Samochod " + i;
                samochod[i].Start();
            }
        }

        public void Kod_Samochodu()
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(800, 2000));
            Console.WriteLine(@"{0} zaczyna dzialanie", Thread.CurrentThread.Name);
            
            _semaphoreDroga.WaitOne();
            _mutexDroga.WaitOne(); //Poczatek sekcji krytycznej zajecia miejsca na drodze
            for (int i = 0; i < testDroga.Length; i++)
            {
                if (testDroga[i])
                {
                    _drogaMiejsce.Value = i;
                    testDroga[i] = false;
                    break;
                }
            }
            _form.Invoke(_form.MyDelegate, true, _drogaMiejsce.Value);
            Thread.Sleep(rand.Next(800, 2000));
            _mutexDroga.ReleaseMutex(); //Koniec sekcji krytycznej zajecia miejsca na drodze
            
            _semaphoreProm.WaitOne();
            _mutexProm.WaitOne(); //Poczatek sekcji krytycznej zajecia miejsca na promie
            for (int i = 0; i < testProm.Length; i++)
            {
                if (testProm[i])
                {
                    _promMiejsce.Value = i;
                    testProm[i] = false;
                    break;
                }
            }
            _form.Invoke(_form.MyDelegate, false, _drogaMiejsce.Value);
            testDroga[_drogaMiejsce.Value] = true;
            _semaphoreDroga.Release();
            _form.Invoke(_form.MyDelegate, true, _promMiejsce.Value + 4);
            _mutexProm.ReleaseMutex();    //Koniec sekcji krytycznej zajecia miejsca na promie

            /*
            _mutexProm.WaitOne();    //Poczatek sekcji krytycznej zwolnienia miejsca na promie
            _form.Invoke(_form.MyDelegate, false, _promMiejsce.Value + 4);
            _promCount--;
            _semaphoreProm.Release();
            _mutexProm.ReleaseMutex();    //Koniec sekcji krytycznej zajecia miejsca na promie


            Thread.Sleep(rand.Next(800, 2000));
            Console.WriteLine(@"{0} zakonczy dzialanie", Thread.CurrentThread.Name);
            */
            Thread.CurrentThread.Interrupt();
        }
    }
}