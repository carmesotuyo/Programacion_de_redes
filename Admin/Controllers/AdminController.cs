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
    //[Route("api/[controller]")] //debug
    [Route("admin")]
    [ApiController]
    public class AdminController : Controller
    {
        // COSAS DEL EJEMPLO QUE NO ENTIENDO ------------------------ DEBUG

        private Admin.AdminClient client;



        //static readonly ISettingsManager SettingsMgr = new SettingsManager();
        //public AdminController()
        //{


        //    AppContext.SetSwitch(
        //          "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        //}

        [HttpPost("products")]
        public async Task<ActionResult> PostProduct([FromBody] ProductDTO product)
        {
            using var channel = GrpcChannel.ForAddress(ServerConfig.GrpcURL);
            client = new Admin.AdminClient(channel);
            var reply = await client.PostProductAsync(product);
            return Ok(reply.Message);
        }




        // COSAS POR DEFECTO ----------------------- DEBUG

        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

