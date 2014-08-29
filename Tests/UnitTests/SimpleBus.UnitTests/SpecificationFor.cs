using System;
using System.Diagnostics;
using NUnit.Framework;

namespace SimpleBus.UnitTests
{
    [TestFixture]
    public abstract class SpecificationFor<T> where T : class
    {
        [SetUp]
        public void Setup()
        {
            Subject = Given();

            _sw = Stopwatch.StartNew();
            When();
            _sw.Stop();

            Console.WriteLine("Elapsed time: {0} seconds", _sw.Elapsed.TotalSeconds);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Subject = null;
        }

        protected T Subject;

        protected abstract T Given();
        protected abstract void When();

        private Stopwatch _sw;

        protected TimeSpan ElapsedTime
        {
            get { return _sw.Elapsed; }
        }
    }
}