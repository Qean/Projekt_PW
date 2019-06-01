using System;
using System.Drawing;
using System.Threading;
using Projekt_PW.Properties;

namespace Projekt_PW.Classes

{
    public class Samochod
    {
        private volatile AutoResetEvent eventHandle1;
        private volatile AutoResetEvent eventHandle2;
        private volatile AutoResetEvent eventHandle3;
        private readonly Form1 _form;
        private readonly Mutex _mutexDroga1;
        private readonly Mutex _mutexDroga2;
        private readonly Mutex _mutexProm;
        private readonly Semaphore _semaphoreDroga1;
        private readonly Semaphore _semaphoreDroga2;
        private readonly Semaphore _semaphoreProm;
        private volatile int _count;
        private volatile bool[] testDroga1;
        private volatile bool[] testDroga2;
        private volatile bool[] testProm;
        static ThreadLocal<int> _droga1Miejsce = new ThreadLocal<int>(() => 0);
        static ThreadLocal<int> _droga2Miejsce = new ThreadLocal<int>(() => 0);
        static ThreadLocal<int> _promMiejsce = new ThreadLocal<int>(() => 0);

        public Samochod(int miejscaDroga, int miejscaProm, int liczbaSamochodow, Form1 form,
            AutoResetEvent eventHandle1, AutoResetEvent eventHandle2, AutoResetEvent eventHandle3)
        {
            _count = 0;
            this.eventHandle1 = eventHandle1;
            this.eventHandle2 = eventHandle2;
            this.eventHandle3 = eventHandle3;
            testDroga1 = new bool[miejscaDroga];
            for (int i = 0; i < miejscaDroga; i++)
            {
                testDroga1[i] = true;
            }

            testDroga2 = new bool[3];
            for (int i = 0; i < 3; i++)
            {
                testDroga2[i] = true;
            }

            testProm = new bool[miejscaProm];
            for (int i = 0; i < miejscaProm; i++)
            {
                testProm[i] = true;
            }

            var liczbaSamochodow1 = liczbaSamochodow;
            _form = form;
            _semaphoreDroga1 = new Semaphore(4, 4);
            _semaphoreDroga2 = new Semaphore(3, 3);
            _semaphoreProm = new Semaphore(6, 6);
            _mutexDroga1 = new Mutex();
            _mutexDroga2 = new Mutex();
            _mutexProm = new Mutex();
            var samochod = new Thread[liczbaSamochodow1];
            for (var i = 0; i < liczbaSamochodow1; i++)
            {
                samochod[i] = new Thread(Kod_Samochodu) {Name = "Samochod " + i};
                samochod[i].Start();
            }
        }

        public void Kod_Samochodu()
        {
            var rand = new Random();
            Thread.Sleep(rand.Next(1500, 2000));
            //Console.WriteLine(@"{0} zaczyna dzialanie", Thread.CurrentThread.Name);

            _semaphoreDroga1.WaitOne();
            _mutexDroga1.WaitOne(); //Poczatek sekcji krytycznej zajecia miejsca na drodze
            for (int i = 0; i < testDroga1.Length; i++)
            {
                if (testDroga1[i])
                {
                    _droga1Miejsce.Value = i;
                    testDroga1[i] = false;
                    break;
                }
            }

            _form.Invoke(_form.MyDelegate, true, _droga1Miejsce.Value);
            Thread.Sleep(rand.Next(800, 2000));
            _mutexDroga1.ReleaseMutex(); //Koniec sekcji krytycznej zajecia miejsca na drodze

            eventHandle1.WaitOne();

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

            _form.Invoke(_form.MyDelegate, false, _droga1Miejsce.Value);
            testDroga1[_droga1Miejsce.Value] = true;
            _form.Invoke(_form.MyDelegate, true, _promMiejsce.Value + 4);
            Interlocked.Increment(ref _count);
            _mutexProm.ReleaseMutex(); //Koniec sekcji krytycznej zajecia miejsca na promie
            _semaphoreDroga1.Release();

            eventHandle2.Reset();
            eventHandle2.WaitOne();
            _mutexProm.WaitOne(); //Poczatek sekcji krytycznej zwolnienia miejsca na promie
            _form.Invoke(_form.MyDelegate, false, _promMiejsce.Value + 4);
            testProm[_promMiejsce.Value] = true;
            _semaphoreDroga2.WaitOne();
            _mutexDroga2.WaitOne();
            for (int i = 0; i < testDroga2.Length; i++)
            {
                if (testDroga2[i])
                {
                    _droga2Miejsce.Value = i;
                    testDroga2[i] = false;
                    break;
                }
            }
            _form.Invoke(_form.MyDelegate, true, _droga2Miejsce.Value + 10);
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
            _semaphoreProm.Release();

            
            Console.WriteLine(1);
            //Thread.Sleep(rand.Next(800, 2000));
            _mutexDroga2.ReleaseMutex();
            
            Console.WriteLine(2);
            eventHandle3.WaitOne();
            _form.Invoke(_form.MyDelegate, false, _droga2Miejsce.Value + 10);
            Console.WriteLine(3);
            _mutexDroga2.WaitOne();
            testDroga2[_droga2Miejsce.Value] = true;
            //Thread.Sleep(rand.Next(800, 2000));
            _mutexDroga2.ReleaseMutex();
            
            _semaphoreDroga2.Release();

            Thread.CurrentThread.Interrupt();
        }
    }
}