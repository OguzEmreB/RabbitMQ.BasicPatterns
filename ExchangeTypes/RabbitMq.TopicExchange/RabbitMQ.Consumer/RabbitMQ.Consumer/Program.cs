
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://xfyrvvna:ZsB56_6_Px0NJ46douFn2FoeN8vzS0nN@woodpecker.rmq.cloudamqp.com/xfyrvvna");

// Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1.Adım Queue Oluşturma
channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );

Console.Write("dinlenecek topic formatını giriniz : ");
string topic = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(

    queue: queueName,
    exchange: "topic-exchange-example",
    routingKey: topic
    );

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(
    queue: queueName, 
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();
 