using System;
using ServerApp.Domain;
using Grpc.Core;
using ServerApp.Logic;

namespace MainServer.Services
{
	public class AdminService : Admin.AdminBase
	{
        private readonly ProductLogic _productLogic = new ProductLogic();
        private readonly UserLogic _userLogic = new UserLogic();

        public override Task<MessageReply> PostProduct(ProductDTO request, ServerCallContext context)
        {
            Console.WriteLine("Antes de crear el producto con nombre {0}", request.Nombre); //debug
            string message;
            try
            {
                message = _productLogic.publicarProducto(DTOToProducto(request), request.User).Nombre;
            } catch(Exception e)
            {
                message = "Hubo un error: " + e.Message;
            }
            return Task.FromResult(new MessageReply { Message = message });
        }

        public override Task<MessageReply> DeleteProduct(ProductDTO request, ServerCallContext context)
        {
            Console.WriteLine("Antes de eliminar el producto con nombre {0}", request.Nombre); //debug
            bool couldDelete;
            string message = "";
            try
            {
                couldDelete = _productLogic.eliminarProducto(request.Nombre, request.User) != null;
            } catch(Exception e)
            {
                couldDelete = false;
                message = e.Message;
            }
            message = couldDelete ? "Producto eliminado correctamente" : "No se pudo eliminar producto: " + message;
            return Task.FromResult(new MessageReply { Message = message });
        }

        public override Task<MessageReply> PutProduct(ProductDTO request, ServerCallContext context)
        {
            Console.WriteLine("Antes de modificar el producto con nombre {0}", request.Nombre); //debug
            string message;
            try
            {
                Producto prodEncontrado = _productLogic.buscarUnProducto(request.Nombre);
                message = _productLogic.modificarProducto(prodEncontrado, request.User, request.AtributoAModificar, request.NuevoValor);
            }
            catch (Exception e)
            {
                message = "Hubo un error: " + e.Message;
            }
            return Task.FromResult(new MessageReply { Message = message });
        }

        public Producto DTOToProducto(ProductDTO productDTO)
        {
            return new Producto(productDTO.Nombre, productDTO.Descripcion, productDTO.Precio, productDTO.Stock);
            // debug ver que pasa si el productDTO no tiene todos los datos
        }


        // DEBUG EJEMPLO DE GREETER
        //private readonly ILogger<GreeterService> _logger;
        //public GreeterService(ILogger<GreeterService> logger)
        //{
        //    _logger = logger;
        //}

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //    });
        //}
    }
}

