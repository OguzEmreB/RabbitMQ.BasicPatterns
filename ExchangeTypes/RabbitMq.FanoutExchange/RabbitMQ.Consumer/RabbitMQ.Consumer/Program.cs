
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
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout
    );

Console.Write("Kuyruk adını giriniz : ");
string queueName = Console.ReadLine();

channel.QueueDeclare(queue: queueName,
    exclusive: false);
channel.QueueBind(

    queue: queueName,
    exchange: "fanout-exchange-example",
    routingKey: string.Empty
    );

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(
    queue: queueName, autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();

//1.adım: Publisher'daki exchange ile birebir aynı isim ve type'a sahip bir
//exchange tanımlanmalıdır.
// 2.adım: publisher tarafından routing key'de bulunan deperdeki kuyruğa
// göönderilen mesajları, kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz
// gerekmektedir. bunun için öncelikle bir kuyruk oluşturulmalıdır.
//3. adım