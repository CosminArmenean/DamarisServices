using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.GenericDtos
{
    public class ConsumerRecord<TKey, TValue>
    {
        public string Topic { get; }
        public int Partition { get; }
        public long Offset { get; }
        public TKey Key { get; }
        public TValue Value { get; }

        public ConsumerRecord(string topic, int partition, long offset, TKey key, TValue value)
        {
            Topic = topic;
            Partition = partition;
            Offset = offset;
            Key = key;
            Value = value;
        }
    }
}