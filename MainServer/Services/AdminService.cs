﻿using System;
using ServerApp.Domain;
using Grpc.Core;
using ServerApp.Logic;
using ServerApp.Controllers;

namespace MainServer.Services
{
	public class AdminService : Admin.AdminBase
	{
        private readonly ProductLogic _productLogic = new ProductLogic();
        private readonly UserController _userController = new UserController();

        public override Task<MessageReply> PostProduct(ProductoNuevoDTO request, ServerCallContext context)
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

        public override Task<MessageReply> DeleteProduct(ProductoBorrarDTO request, ServerCallContext context)
        {
            Console.WriteLine("Antes de eliminar el producto con nombre {0}", request.Producto); //debug
            bool couldDelete;
            string message = "";
            try
            {
                couldDelete = _productLogic.eliminarProducto(request.Producto, request.User) != null;
            } catch(Exception e)
            {
                couldDelete = false;
                message = e.Message;
            }
            message = couldDelete ? "Producto eliminado correctamente" : "No se pudo eliminar producto: " + message;
            return Task.FromResult(new MessageReply { Message = message });
        }

        public override Task<MessageReply> PutProduct(ProductoModificarDTO request, ServerCallContext context)
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

        public override async Task<MessageReply> PostCompra(CompraDTO request, ServerCallContext context)
        {
            Console.WriteLine("Antes de realizar compra de producto {0}", request.Producto); //debug
            string message;
            try
            {
                message = (await _userController.agregarProductoAComprasAdmin(request.User, request.Producto)).MensajeEntregadoACliente;
            }
            catch (Exception e)
            {
                message = "Hubo un error: " + e.Message;
            }
            return await Task.Run(() => new MessageReply { Message = message });
        }

        public override Task<MessageReply> GetCalificaciones(CalificacionesDTO nombreProducto, ServerCallContext context)
        {
            Console.WriteLine("Antes de obtener las calificaciones de el producto {0}", nombreProducto.Producto); //debug
            string message;
            try
            {
                Producto prodEncontrado = _productLogic.buscarUnProducto(nombreProducto.Producto);
                message = prodEncontrado.calificaciones.ToString(); // debug ver como se muestra esto
            }
            catch (Exception e)
            {
                message = "Hubo un error: " + e.Message;
            }
            return Task.FromResult(new MessageReply { Message = message });
        }


        public Producto DTOToProducto(ProductoNuevoDTO productDTO)
        {
            return new Producto(productDTO.Nombre, productDTO.Descripcion, productDTO.Precio, productDTO.Stock);
        }

    }
}

