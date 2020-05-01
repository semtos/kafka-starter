using System.Threading.Tasks;
using Confluent.Kafka;
using System;

namespace Producer
{
    class Program
    {

        private static readonly string TOPIC = "LOOP-TOPIC";
        private static readonly string NODES = "localhost:9092,localhost:9093,localhost:9094";

        static void Main(string[] args)
        {
            System.Console.Title = "Producer";
            for (int i = 0; i < 100; i++)
            {
                WriteMessagesAsync($"Test Message {i}").Wait();
            }
        }

        private static async Task WriteMessagesAsync(string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = NODES
            };
            using (var p = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(TOPIC, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
                    System.Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (KafkaException e)
                {
                    System.Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
