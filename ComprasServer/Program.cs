using ComprasServer.Logic;
using ComprasServer.Service;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ComprasServer
{
    public class Program
    {
        private static readonly ComprasLogic _compraLogic = new ComprasLogic();
        //private static MQService mq;
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            try
            {
                // Iniciamos el servidor original
                var taskMQ = Task.Run(async () =>
                {
                    await InitializeMQServiceAsync();
                    Console.WriteLine("Se llam� al metodo");
                });

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            // Add services to the container.
            builder.Services.AddScoped<ComprasLogic>();
            builder.Services.AddControllers();
            


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Inicializar MQService de forma as�ncrona
            
            
            //mq = new MQService();
            app.Run();
            
        }

        private static async Task InitializeMQServiceAsync()
        {
            //mq = new MQService();
            // Si MQService tiene operaciones as�ncronas, tambi�n podr�as esperarlas aqu�
            // Ejemplo: await mq.InicializarAsync();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Compras", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

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
                    //compra.Fecha = compra.Fecha.ToShortDateString();

                    _compraLogic.agregarCompra(compra);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}