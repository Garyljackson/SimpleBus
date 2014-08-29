using System;

namespace SimpleBus.Contract.Core
{
    public interface ISerializer
    {
        object Deserialize(string serializedObject, Type type);
        string Serialize(object item);
    }
}