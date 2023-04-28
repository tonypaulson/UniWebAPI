using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Controllers
{
    [Route("carewell/api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {

        [HttpGet]
        public string GetHelloWorld()
        {
            return "hello world from hello world controller";
        }
    }
}
