using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SemaphoreDemo
{
    public class SemaphoreIndex:IDisposable
    {
        public SemaphoreSlim _pool;
        //public IDisposable _pool;
        public EventWaitHandle indexTrigger;
        //public IDisposable indexTrigger;
        public SemaphoreIndex()
        {
            _pool = new SemaphoreSlim(1,3);
            indexTrigger = new EventWaitHandle(false,EventResetMode.AutoReset);
        }

        public int indexI;
        private bool _disposed=false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SemaphoreIndex()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(_disposed)
            { return; }
            else
            {
                if(disposing)
                {
                    _pool?.Dispose();
                    indexTrigger?.Dispose();
                }
                _disposed = true;
            }
        }
    }
    class Program
    {
        //private static SemaphoreSlim _pool;

        static void Main(string[] args)
        {
            var syncI = new SemaphoreIndex();
            syncI.indexI = 0;
            int missionCount = 5;
            //Current Semaphore number means how many vacanies are waiting for used here. 
            Console.WriteLine($"--------------------\n| Single Thread |\n--------------------");
            /*
             * while (syncI.indexI < 5)
             * {
             * syncI.indexTrigger.Reset();
             * Thread t = new Thread(ThreadProc);
             * t.Start(syncI);
             * syncI.indexTrigger.WaitOne();
             * Interlocked.Increment(ref syncI.indexI);
             * }
             */
            var TaskList=new List<Task>();
            while (syncI.indexI <missionCount)
            {
                syncI.indexTrigger.Reset();
                TaskList.Add(Task.Factory.StartNew(()=>ThreadProc(syncI)));
                syncI.indexTrigger.WaitOne();
                Interlocked.Increment(ref syncI.indexI);
            }
            Thread.Sleep(3000);
            syncI._pool.Release(2);
            Console.WriteLine($"--------------------\n| Three thread parallel |\n--------------------");
            Task.WaitAll(TaskList.ToArray());
            Console.WriteLine($"Disposing...");
            syncI.Dispose();
            Console.WriteLine($"Main thread exits.\n");
        }

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
