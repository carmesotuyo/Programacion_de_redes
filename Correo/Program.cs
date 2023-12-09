using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Correo;
class Program
{
    static async Task Main(string[] args)
    {
        //using var channel = GrpcChannel.ForAddress("http://localhost:5239");
        //var client = new Correo.CorreoClient(channel);

        // LO DE NACHO -------------------------------------------------------------------------------------------
        //// Aquí deberías tener la lógica para obtener el destinatario y el mensaje
        //var destinatario = "usuario@example.com";
        //var mensaje = "Este es el cuerpo del correo...";

        //// Llama al método EnviarCorreo del servicio de correo
        //var response = client.EnviarCorreo(new EnviarCorreoRequest { Destinatario = destinatario, Mensaje = mensaje });

        //Console.WriteLine($"Respuesta del servidor: {response.Resultado}");

        // ANTERIOR DE GREETER -------------------------------------------------------------------------------------------
        //var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
        //Console.WriteLine("Greeting: " + reply.Message);
        //Console.WriteLine("Press any key to exit...");
        //Console.ReadKey();

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "Compras", type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName; //Declaro una queue por defecto

            channel.QueueBind(queue: queueName,
                              exchange: "Compras",
                              routingKey: "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }

}