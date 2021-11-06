using System;
using System.Threading;

namespace SemaphoreDemo
{
    class Program
    {
        private static SemaphoreSlim _pool;

        //Default value is 0
        private static int _padding=0;
        static void Main(string[] args)
        {
            //Current Semaphore number means how many vacanies are waiting for used here. 
            _pool = new SemaphoreSlim(0,3);
            for(int i=0;i<5;i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Start(i);
            }
            Thread.Sleep(3000);

            Console.WriteLine($"Semaphore call Release(3), make other threads use it.");
            _pool.Release(3);
            Console.WriteLine($"Main thread exits.\n");
        }

        private static void ThreadProc(object num)
        {
            Console.WriteLine($"Thread {(int)num} starts.\nThread {(int)num} waits for the semaphore.\n");
            _pool.Wait();
            int padding = Interlocked.Add(ref _padding, 500);
            Console.WriteLine($"Thread {(int)num} Enter the semaphore");

            Thread.Sleep(1500 + padding);

            Console.WriteLine($"Thread {(int)num} releases the semephore.");

            //Release and show count previously.
            Console.WriteLine($"Thread {(int)num} previous semaphore count: {_pool.Release(1)}\nPresent semaphore count: {_pool.CurrentCount}\n");
        }
    }
}
