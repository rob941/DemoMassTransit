using Contracts;
using MassTransit;
using System.Threading.Tasks;
using System;

namespace Components.Consumers
{

    public class MessageConsumer : IConsumer<IMessage>
    {

        public Task Consume(ConsumeContext<IMessage> context) {

            var message = context.Message;

            if (message.Id == 1) {
                if (context.GetRetryAttempt() == 0) {
                    throw new Exception("Eccezione generata 1");
                } else if (context.GetRetryAttempt() == 1) {
                    Console.WriteLine("Messaggio ricevuto!!");
                }
            }
            System.Console.WriteLine($"\nText: {message.Text}\n");
            return Task.CompletedTask;
        }
    }
}
