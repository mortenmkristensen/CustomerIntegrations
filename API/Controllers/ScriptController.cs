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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Script> GetScriptById(string id) {
            string json = "";
            try {
                Script script = dbAccess.GetScriptById(id);
                json = Serialize(script);
            } catch (Exception e) {
                return StatusCode(500);
            }
            return Ok(json);
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

        private Script Deserialize(string json) {
            return JsonConvert.DeserializeObject<Script>(json);
        }

        private string Serialize(Script script) {
            return JsonConvert.SerializeObject(script);
        }

    }
}