using Contracts;
using MassTransit;
using System.Threading.Tasks;

namespace Components.Consumers
{
    public class KafkaConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context) {

            var message = context.Message;

            System.Console.WriteLine($"\nKafka Text: {message.Text}\n");
            return Task.CompletedTask;
        }
    }
}
