using System;
using Newtonsoft.Json;
using SimpleBus.Contract.Core;

namespace SimpleBus.Infrastructure
{
    internal class JsonSerializer : ISerializer
    {
        private readonly Formatting _formatting;
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer()
            : this(null, Formatting.None)
        {
        }

        public JsonSerializer(JsonSerializerSettings settings, Formatting formatting)
        {
            _settings = settings;
            _formatting = formatting;
        }

        public object Deserialize(string serializedObject, Type type)
        {
            return JsonConvert.DeserializeObject(serializedObject, type, _settings);
        }

        public string Serialize(object item)
        {
            string json = JsonConvert.SerializeObject(item, _formatting, _settings);
            return json;
        }
    }
}