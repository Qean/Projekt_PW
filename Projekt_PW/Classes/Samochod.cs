using System;
using System.Drawing;
using System.Threading;
using Projekt_PW.Properties;

namespace Projekt_PW.Classes

{
    public class Samochod
    {
        private volatile ManualResetEvent eventHandle1;
        private volatile ManualResetEvent eventHandle2;
        private readonly Form1 _form;
        private readonly Mutex _mutexDroga;
        private readonly Mutex _mutexProm;
        private readonly Semaphore _semaphoreDroga;
        private readonly Semaphore _semaphoreProm;
        private volatile int _count;
        private volatile bool[] testDroga;
        private volatile bool[] testProm;
        static ThreadLocal<int> _drogaMiejsce = new ThreadLocal<int>(() => 0);
        static ThreadLocal<int> _promMiejsce = new ThreadLocal<int>(() => 0);

        public Samochod(int miejscaDroga, int miejscaProm, int liczbaSamochodow, Form1 form,
            ManualResetEvent eventHandle1, ManualResetEvent eventHandle2)
        {
            _count = 0;
            this.eventHandle1 = eventHandle1;
            this.eventHandle2 = eventHandle2;
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
            Thread.Sleep(rand.Next(1500, 2000));
            //Console.WriteLine(@"{0} zaczyna dzialanie", Thread.CurrentThread.Name);

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
            
            eventHandle1.WaitOne();
            _form.Set_event2_Flag(1);
            Console.WriteLine(@"1");

            _semaphoreProm.WaitOne();
            Console.WriteLine(@"2");
            _mutexProm.WaitOne(); //Poczatek sekcji krytycznej zajecia miejsca na promie
            Console.WriteLine(@"3 {0} wjezdza na prom", Thread.CurrentThread.Name);
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
            Interlocked.Increment(ref _count);
            Thread.Sleep(rand.Next(800, 2000));
            _mutexProm.ReleaseMutex(); //Koniec sekcji krytycznej zajecia miejsca na promie

            eventHandle2.WaitOne();

            _mutexProm.WaitOne(); //Poczatek sekcji krytycznej zwolnienia miejsca na promie
            _form.Invoke(_form.MyDelegate, false, _promMiejsce.Value + 4);
            testProm[_promMiejsce.Value] = true;
            Interlocked.Decrement(ref _count);
            if (_count == 0)
            {
                _form.Change_Direction();
                _form.Set_Prom_Flag(0);
                _form.Set_event2_Flag(0);
                _form.Set_event1_Flag(1);
                eventHandle2.Reset();
            }
            Thread.Sleep(rand.Next(800, 2000));
            _mutexProm.ReleaseMutex(); //Koniec sekcji krytycznej zajecia miejsca na promie
            eventHandle1.WaitOne();
            _semaphoreProm.Release();

            Console.WriteLine(@"{0} zakonczy dzialanie", Thread.CurrentThread.Name);
            Thread.CurrentThread.Interrupt();
        }
    }
}