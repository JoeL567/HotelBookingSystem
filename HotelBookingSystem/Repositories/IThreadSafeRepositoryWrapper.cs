using System;
using System.Collections;
using System.Collections.Generic;

namespace HotelBookingSystem.Repositories
{
    public interface IThreadSafeRepositoryWrapper<T>
    {
        public T ReadWrapper(Func<T> query);

        public IEnumerable<T> ReadManyWrapper(Func<IEnumerable<T>> query);

        public void WriteWrapper(Action query);
    }
}