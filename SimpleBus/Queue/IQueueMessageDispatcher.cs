using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBus.Queue
{
    internal interface IQueueMessageDispatcher
    {
        Task StartDispatchers(IEnumerable<KeyValuePair<Type, Func<object, Task>>> typeHandlerMaps);
    }
}