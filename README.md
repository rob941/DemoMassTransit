# DemoMassTransit
Simple demo with one producer and one consumer per bus instance.
2 configuration producer/consumer:
  - MassTransit/RabbitMQ transport                  (bus 1)
  - MassTransit/RabbitMQ transport and Kafka rider  (bus 2)

__Table of content__

1) Run container of RabbitMQ and Kafka before. 
2) Send a message via bus 1/2 and get response.
2) Go to RabbitMQ management UI local site (http://127.0.0.1:15672) to check queues and message state.

__Quick url namespaces__

1)   POST method http://127.0.0.1:5000/message/sendasync       ---> send a simple message with random id and text guid.
