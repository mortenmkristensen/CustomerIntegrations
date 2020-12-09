using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase{
        IDBAccess dbAccess = new DBAccess(new DBConfig());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UploadScript([FromBody] string script) {
            try {
                Script deserialzedScript = Deserialize(script);
                dbAccess.Upsert(deserialzedScript);
            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateScript([FromBody]string script) {
            try {
                Script deserialzedScript = Deserialize(script);
                dbAccess.Upsert(deserialzedScript);
            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok();
        }
        // /api/script?id={id}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Script> GetScriptById(string id) {
            Script script;
            try {
                script = dbAccess.GetScriptById(id);
            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok(script);
        }

        //this metod returns an empty list no matter what i send to it and i cant figure out why
        // api/script/customer?customer={customer}
        [HttpGet("Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Script> GetScriptByCustomer(string customer) {
            IEnumerable<Script> scripts = new List<Script>();
            try {
                scripts = dbAccess.GetByCustomer(customer);

            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok(scripts);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteScript(string id) {
            try {
                dbAccess.Delete(id);
            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Script> GetAllScripts() {
            IEnumerable<Script> scripts = new List<Script>();
            try {
                scripts = dbAccess.GetAll();
            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok(scripts);
        }

        private Script Deserialize(string json) {
            return JsonConvert.DeserializeObject<Script>(json);
        }

    }
}