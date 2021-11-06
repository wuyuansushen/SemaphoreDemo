using System;
using System.Threading;

namespace SemaphoreDemo
{
    public class SemaphoreIndex
    {
        public SemaphoreSlim _pool;
        public int indexI;
    }
    class Program
    {
        //private static SemaphoreSlim _pool;

        static void Main(string[] args)
        {
            var syncI = new SemaphoreIndex();
            syncI.indexI = 0;
            //Current Semaphore number means how many vacanies are waiting for used here. 
            syncI._pool = new SemaphoreSlim(1,3);
            Console.WriteLine($"--------------------\n| Single Thread |\n--------------------");
            for(int i=0;i<5;i++)
            {
                Thread t = new Thread(ThreadProc);
                syncI.indexI = i;
                t.Start(syncI);
            }
            Thread.Sleep(3000);
            syncI._pool.Release(2);
            Console.WriteLine($"--------------------\n| Three thread parallel |\n--------------------");
            Console.WriteLine($"Main thread exits.\n");
        }

        private static void ThreadProc(object state)
        {
            var num = ((SemaphoreIndex)state).indexI;
            var _pool = ((SemaphoreIndex)state)._pool;
            Console.WriteLine($"Thread {(int)num} starts.\nThread {(int)num} waits for the semaphore.\n");
            _pool.Wait();
            _pool.Wait();
            Console.WriteLine($"Thread {(int)num} Enter the semaphore.");

            Thread.Sleep(1500);

            Console.WriteLine($"Thread {(int)num} releases the semephore.");

            //Release and show count previously.
            Console.WriteLine($"Thread {(int)num} previous semaphore count: {_pool.Release(2)}\nPresent semaphore count: {_pool.CurrentCount}\n");
        }
    }
}
