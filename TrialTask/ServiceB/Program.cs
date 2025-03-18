using ServiceB.Services;

var kafkaBootstrapServer =
    Environment.GetEnvironmentVariable("Kafka:BootstrapServer", EnvironmentVariableTarget.Process);
var kafkaTopicName =
    Environment.GetEnvironmentVariable("Kafka:TopicName", EnvironmentVariableTarget.Process);


var kafkaConsumer = new KafkaConsumerService(kafkaBootstrapServer);
kafkaConsumer.ConsumeMessages(kafkaTopicName);