using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TinyUrl.Models
{
    public class ReaderWritersSync
    {
        private Mutex readersLock;
        private Mutex writersLock;
        private int readersCount;
        public ReaderWritersSync()
        {
            readersLock = new Mutex();
            writersLock = new Mutex();
            readersCount = 0;
        }

        public void EnterRead()
        {

            readersLock.WaitOne();
            readersCount++;
            if (readersCount == 1)
            {
                writersLock.WaitOne();
            }

            readersLock.ReleaseMutex();
            
        }
        
        public void EnterWrite()
        {
            writersLock.WaitOne();
        }

        public void LeaveRead()
        {
            readersLock.WaitOne();
            readersCount--;
            if (readersCount == 0)
            {
                writersLock.ReleaseMutex();
            }

            readersLock.ReleaseMutex();
        }

        public void LeaveWrite()
        {
            writersLock.ReleaseMutex();
        }
    }
}
