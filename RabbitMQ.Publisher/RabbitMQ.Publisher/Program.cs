
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://xfyrvvna:ZsB56_6_Px0NJ46douFn2FoeN8vzS0nN@woodpecker.rmq.cloudamqp.com/xfyrvvna");


// Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P Tasarımı (genelde Direct Exchange)

//string queueName = "example-p2p-qyueue";
//channel.QueueDeclare(
//    queue:queueName,
//    durable:false,
//    autoDelete:false,
//    exclusive:false
//    );

//byte[] message = Encoding.UTF8.GetBytes("merhaba");
//channel.BasicPublish(exchange:string.Empty,routingKey:queueName,body:message);


#endregion

#region Publish/Subscribe Tasarımı ( fanout exchange)

//string exchangeName = "example-pub-sub-exchange";
//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(300);

//    byte[] message = Encoding.UTF8.GetBytes("merhaba"+i);
//    channel.BasicPublish(
//    exchange: exchangeName,
//    routingKey: string.Empty,
//    body: message);
//}




#endregion


#region Work Queue Tasarımı
//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    exclusive:false,
//    durable: false,
//    autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(300);

//    byte[] message=Encoding.UTF8.GetBytes("message"+i);
//    channel.BasicPublish(
//        exchange:string.Empty,
//        routingKey:queueName,
//        body: message
//        );
//}
#endregion


#region Request/response Tasarımı

string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();



#region Request Mesajını Oluşturma ve Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}
#endregion
#region Response Kuyruğu Dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        //....
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};
#endregion

#endregion

Console.Read();