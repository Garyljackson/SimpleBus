using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleBus.Infrastructure
{
    /// Based on Stephen Toub's article:
    /// http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/10116210.aspx
    internal sealed class AsyncLazy<T> : Lazy<Task<T>>
    {

        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.Run(valueFactory))
        {
        }

        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Run(taskFactory))
        {
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}