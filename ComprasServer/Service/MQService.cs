using ComprasServer.Logic;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ComprasServer.Service
{
    public class MQService
    {
        private readonly ComprasLogic _compraLogic = new ComprasLogic();
        public MQService() {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Compras", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName; //Declaro una queue por defecto

                channel.QueueBind(queue: queueName,
                                  exchange: "Compras",
                                  routingKey: "");

                Console.WriteLine(" [*] Esperando por Compras.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Compra compra = JsonSerializer.Deserialize<Compra>(message);


                    //=========== Acá hago algo con el mensaje que llega ===========
                    
                    _compraLogic.agregarCompra(compra);
                    Console.WriteLine("================================Acá llegó la info ========================");
                    Console.WriteLine(compra);
                    Console.WriteLine("================================si se guardó ========================");
                    Console.WriteLine(_compraLogic.darListadoCompras());
                    //==============================================================
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                //Console.ReadLine();
            }

        }
    }
}
