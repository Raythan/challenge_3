using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIService.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Endpoint que verifica se a API foi construída até o fim.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///<![CDATA[
        ///     GET /v1/Health
        ///     Verifica se a API inicializou com sucesso.
        ///     Retorna as informações de compilação.
        ///]]>
        /// </remarks>
        /// <response code="200">Sucesso.</response>
        /// <response code="500">Em caso de erro.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Health> Get()
        {
            return JsonConvert.DeserializeObject<Health>(JsonConvert.SerializeObject(Extender.AssemblyInfo));
        }
    }
}
