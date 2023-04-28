using  UniWeb.API.Exceptions;
using UniWeb.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace UniWeb.API.Controllers
{
    [Route("carewell/api/[controller]")]
    [ApiController]
    class BaseController : ControllerBase
    {
        public ISharedResource _sharedResource { get; set; }
        public BaseController(ISharedResource sharedResource)
        {
            _sharedResource = sharedResource;
        }

        public int GetTenantId()
        {
            var tenantIdString = HttpContext.Request.Headers["TokenId"];
            if (!string.IsNullOrEmpty(tenantIdString))
            {
                return Convert.ToInt32(tenantIdString);
            }
            throw new UniWebBusinessException(_sharedResource.TokenIdNotPresent);
        }
    }
}