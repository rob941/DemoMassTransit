using Components.Consumers;
using Contracts;
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public interface ISecondBus : IBus { }

    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();

            // First Bus with only RabbitMQ transport
            services.AddMassTransit(x => {

                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<MessageConsumer>(c => c.UseMessageRetry(r => r.Intervals(2,500)));

                x.UsingRabbitMq((context,cfg) => {

                    cfg.Message<IMessage>(c => c.SetEntityName("MyMessage"));

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            // Second Bus with RabbitMQ transport and Kafka rider
            services.AddMassTransit<ISecondBus>(x => {
                x.AddConsumer<KafkaConsumer>();

                x.UsingRabbitMq((context,cfg) => {
                    cfg.ConfigureEndpoints(context);
                });

                x.AddRider(rider => {
                    rider.AddConsumer<KafkaConsumer>();

                    rider.UsingKafka((context,k) => {
                        k.Host("localhost:9092");

                        k.TopicEndpoint<IMessage>("topic-name","consumer-group-name",e => {
                            e.ConfigureConsumer<KafkaConsumer>(context);
                        });
                    });
                });
            });
        }

        public void Configure(IApplicationBuilder app,IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
