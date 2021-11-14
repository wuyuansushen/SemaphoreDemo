using System;
using System.Threading;

namespace SemaphoreDemo
{
    public class SemaphoreIndexBase:Object,IDisposable
    {
        public SemaphoreSlim _pool;
        //public IDisposable _pool;
        public EventWaitHandle indexTrigger;
        //public IDisposable indexTrigger;
        public SemaphoreIndexBase():base()
        {
            _pool = new SemaphoreSlim(1,3);
            indexTrigger = new EventWaitHandle(false,EventResetMode.AutoReset);
            _disposed = false;
        }

        public int indexI;
        private bool _disposed;
        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
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
                    _pool=null;
                    indexTrigger=null;
                }
                // Don't need to free unmanaged resource
                // because this class doesn't override finalizer.
                _disposed = true;
            }
        }
    }
    public class SemaphoreIndex:SemaphoreIndexBase
    {
        private bool _disposedValue;
        public SemaphoreIndex():base()
        {
            _disposedValue= false;
        }
        protected override void Dispose(bool disposing)
        {
            if(_disposedValue==false)
            {
                if (disposing) {
                }
                _disposedValue= true;
            }
            base.Dispose(disposing);
        }
    }
}
