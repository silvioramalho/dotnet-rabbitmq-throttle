# dotnet-rabbitmq-throttle

Proof of concept to create a throttled consumer

## Run RabbitMQ on localhost

> docker run -d --hostname rabbit-local --name poc-rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=userpoc -e RABBITMQ_DEFAULT_PASS=POC2021! rabbitmq:3-management

## Connection string

amqp://userpoc:POC2021!@localhost:5672/

## Queue Name

testqueue

