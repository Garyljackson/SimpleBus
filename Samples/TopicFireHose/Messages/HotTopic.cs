using System;

namespace TopicFireHose.Messages
{
    public class HotTopic
    {
        public int MessageNumber { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
    }
}