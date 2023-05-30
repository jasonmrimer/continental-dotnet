// using System;
// using System.Threading.Tasks;
// using Confluent.Kafka;
// using NUnit.Framework;
//
// [TestFixture]
// public class MessageProducerTest
// {
//     [Test]
//     public async void SendsMessage()
//     {
//         Console.WriteLine("test kafka");
//         string bootstrapServers = "localhost:9092";
//
//         string topic = "test";
//         MessageProducer producer = new MessageProducer(
//             bootstrapServers: bootstrapServers,
//             topic: topic
//         );
//
//         // await messageProducer.ProduceMessageAsync("hw");
//         string message = "hw";
//
//         await producer.ProduceMessageAsync(message);
//         using (var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
//                {
//                    BootstrapServers = bootstrapServers,
//                    GroupId = "test-group",
//                    AutoOffsetReset = AutoOffsetReset.Earliest
//                }).Build())
//         {
//             consumer.Subscribe(topic);
//
//             // Act
//             await producer.ProduceMessageAsync(message);
//
//             // Assert
//             var consumeResult = consumer.Consume();
//             string receivedMessage = consumeResult.Message.Value;
//         }
//
//         Assert.IsTrue(false);
//         Assert.AreEqual(message, "receivedMessage®");
//     }
// }