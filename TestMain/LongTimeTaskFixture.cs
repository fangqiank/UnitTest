using System;
using UnitTest;

namespace TestMain
{
    public class LongTimeTaskFixture:IDisposable
    {
        public LongTermTask Task { get; private set; }

        public LongTimeTaskFixture()
        {
            Task = new LongTermTask();
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
