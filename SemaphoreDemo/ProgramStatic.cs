using System;
using System.Threading;

namespace SemaphoreDemo
{
    partial class Program
    {
        private static void ThreadProc(object state)
        {
            ((SemaphoreIndex)state).indexTrigger.Set();
            var num = ((SemaphoreIndex)state).indexI;
            var _pool = ((SemaphoreIndex)state)._pool;
            Console.WriteLine($"Thread {(int)num} starts.\nThread {(int)num} waits for the semaphore.\n");
            _pool.Wait();
            Console.WriteLine($"Thread {(int)num} Enter the semaphore.");

            Thread.Sleep(1500);

            Console.WriteLine($"Thread {(int)num} releases the semephore.");

            //Release and show count previously.
            Console.WriteLine($"Thread {(int)num} previous semaphore count: {_pool.Release(1)}\nPresent semaphore count: {_pool.CurrentCount}\n");
        }
    }
}
