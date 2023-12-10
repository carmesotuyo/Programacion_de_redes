using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Admin;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Admin.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private Admin.AdminClient client;

        [HttpPost("products")]
        public async Task<ActionResult> PostProduct([FromBody] ProductDTO product)
        {
            using var channel = GrpcChannel.ForAddress(ServerConfig.GrpcURL);
            client = new Admin.AdminClient(channel);
            var reply = await client.PostProductAsync(product);
            return Ok(reply.Message);
        }

        [HttpDelete("products")]
        public async Task<ActionResult> DeleteProduct([FromBody] ProductDTO product)
        {
            using var channel = GrpcChannel.ForAddress(ServerConfig.GrpcURL);
            client = new Admin.AdminClient(channel);
            var reply = await client.DeleteProductAsync(product);
            return Ok(reply.Message);
        }

        [HttpPut("products")]
        public async Task<ActionResult> PutProduct([FromBody] ProductDTO product)
        {
            using var channel = GrpcChannel.ForAddress(ServerConfig.GrpcURL);
            client = new Admin.AdminClient(channel);
            var reply = await client.PutProductAsync(product);
            return Ok(reply.Message);
        }


    }
}

