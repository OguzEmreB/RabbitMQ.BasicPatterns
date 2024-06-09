
using RabbitMQ.Client;
using System.Text;


//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://xfyrvvna:ZsB56_6_Px0NJ46douFn2FoeN8vzS0nN@woodpecker.rmq.cloudamqp.com/xfyrvvna");

// Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel =connection.CreateModel();

//Queue Oluşturma
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesaj : ");
    string message=Console.ReadLine();
    byte[]byteMessage= Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey:"direct-queue-example",
        body:byteMessage

        );
};




Console.Read();