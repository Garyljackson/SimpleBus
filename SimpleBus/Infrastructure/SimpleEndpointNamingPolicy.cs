using System;
using System.Globalization;
using System.Linq;
using System.Text;
using SimpleBus.Contract.Core;

namespace SimpleBus.Infrastructure
{
    internal class SimpleEndpointNamingPolicy : IEndpointNamingPolicy
    {
        // Entity segments can contain only letters, numbers, periods (.), hyphens (-), and underscores.
        private const string QueueCharacterWhitelist = "abcdefghijklmnopqrstuvwxyz1234567890.-";

        private const string QueuePrefix = "q";
        private const string TopicPrefix = "t";
        private const string SubscriptionPrefix = "s";

        public string GetQueueName(Type messageType)
        {
            return Sanitize(QueuePrefix + messageType.Name);
        }

        public string GetTopicName(Type messageType)
        {
            return Sanitize(TopicPrefix + messageType.Name);
        }

        public string GetSubscriptionName(Type messageType, Type processorType)
        {
            return Shorten(Sanitize(SubscriptionPrefix + processorType.Name),50);
        }

        private static string Shorten(string path, int maxlength)
        {
            if (path.Length <= maxlength)
                return path;

            var hash = CalculateAdler32Hash(path);

            var shortPath = path.Substring(0, maxlength - hash.Length) + hash;
            return shortPath;
        }

        private static string Sanitize(string path)
        {
            path = string.Join("", path.ToLower().ToCharArray().Select(SanitiseCharacter));
            return path;
        }

        private static char SanitiseCharacter(char currentChar)
        {
            var whiteList = QueueCharacterWhitelist.ToCharArray();

            if (!whiteList.Contains(currentChar))
                return '.';

            return currentChar;
        }

        private static string CalculateAdler32Hash(string inputString)
        {
            const uint BASE = 65521;
            var buffer = Encoding.UTF8.GetBytes(inputString);
            uint checksum = 1;
            var offset = 0;
            var count = buffer.Length;

            var s1 = checksum & 0xFFFF;
            var s2 = checksum >> 16;

            while (count > 0)
            {
                var n = 3800;
                if (n > count)
                {
                    n = count;
                }
                count -= n;
                while (--n >= 0)
                {
                    s1 = s1 + (uint)(buffer[offset++] & 0xff);
                    s2 = s2 + s1;
                }
                s1 %= BASE;
                s2 %= BASE;
            }

            checksum = (s2 << 16) | s1;
            return checksum.ToString(CultureInfo.InvariantCulture);
        }
    }
}