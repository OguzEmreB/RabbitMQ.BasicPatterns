
using RabbitMQ.Client;
using System.Text;


//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://xfyrvvna:ZsB56_6_Px0NJ46douFn2FoeN8vzS0nN@woodpecker.rmq.cloudamqp.com/xfyrvvna");

// Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel =connection.CreateModel();

//Queue Oluşturma
channel.ExchangeDeclare(
    exchange: "topic-exchange-example", 
    type: ExchangeType.Topic);



for (int i = 0; i < 100 ; i++)
{
    await Task.Delay(300);
    byte[] message = Encoding.UTF8.GetBytes($"Merha*ba {i}");
    Console.Write("Topic Belirtiniz:");
    string topic = Console.ReadLine();
    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message);
}
Console.Read();