using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HotelBookingSystem.Repositories
{
    public class ThreadSafeRepositoryWrapper<T> : IThreadSafeRepositoryWrapper<T>
    {
        // Use a Reader writer lock on the rooms data - we allow a shared lock for reading data but require exclusive locking for read write and write
        static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();

        public IEnumerable<T> ReadManyWrapper(Func<IEnumerable<T>> query)
        {
            try
            {
                _rw.EnterReadLock();
                IEnumerable<T> result = query();
                _rw.ExitReadLock();
                return result;
            }
            catch(Exception e)
            {
                if (_rw.IsReadLockHeld)
                {
                    _rw.ExitReadLock();
                }

                throw e;
            }
        }

        public T ReadWrapper(Func<T> query)
        {
            try
            {
                _rw.EnterReadLock();
                T result = query();
                _rw.ExitReadLock();
                return result;
            }
            catch (Exception e)
            {
                if (_rw.IsReadLockHeld)
                {
                    _rw.ExitReadLock();
                }

                throw e;
            }
        }

        public void WriteWrapper(Action query)
        {
            try
            {
                _rw.EnterWriteLock();
                query();
                _rw.ExitWriteLock();
                return;
            }
            catch (Exception e)
            {
                if (_rw.IsWriteLockHeld)
                {
                    _rw.ExitWriteLock();
                }

                throw e;
            }
        }
    }
}
