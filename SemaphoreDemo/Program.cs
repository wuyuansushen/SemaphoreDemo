using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SemaphoreDemo
{
    partial class  Program
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

    }
}
