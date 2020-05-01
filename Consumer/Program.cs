using System;
using Confluent.Kafka;
using System.Threading;

namespace Consumer
{
    class Program
    {

        private static readonly string TOPIC = "LOOP-TOPIC";
        private static readonly string NODES = "localhost:9092,localhost:9093,localhost:9094";

        static void Main(string[] args)
        {
            Console.WriteLine($"Consumer {args[0]}");
            MessageProcessor();
        }

        private static void MessageProcessor()
        {
            //string consumerGroupId = "consumer-group-1";
            string consumerGroupId = Guid.NewGuid().ToString();
            var config = new ConsumerConfig()
            {
                GroupId = consumerGroupId,
                BootstrapServers = NODES,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            using (var consumerClient = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                try
                {
                    consumerClient.Subscribe(TOPIC);

                    while (true)
                    {
                        var consumeResult = consumerClient.Consume(cts.Token);
                        System.Console.WriteLine($"{consumeResult.TopicPartitionOffset} Processed message: {consumeResult.Value}");
                        //commits the message log
                        consumerClient.Commit(consumeResult);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                catch (KafkaException e)
                {
                    System.Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
                finally
                {
                    consumerClient.Close();
                }
            }
        }
    }
}
