using System;
using System.Collections.Generic;
using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase{
        private IDBAccess _dbAccess;
        private readonly ILogger<ScriptController> _log;

        public ScriptController(ILogger<ScriptController> log, IDBAccess dbAccess) {
            _log = log;
            _dbAccess = dbAccess;

        }
        // This method is a POST method that calls the Upsert method from the DBAccess to save the script in the database.
        // Param: a script object.
        // Return: If the script is saved in the database, it will return the help method Ok with the script. If there is an exception, it will return the statuscode 500. 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Script> UploadScript([FromBody] Script script) {
            try {
                _dbAccess.Upsert(script);
            } catch (Exception e) {
                _log.LogWarning(e, "Unable to upload script");
                return StatusCode(500);
            }
            return Ok(script);
        }

        // This method is a PUT method that calls the Upsert method from the DBAccess to update the script in the database.
        // Param: a script object.
        // Return: If the script is updated in the database, it will return the help method Ok with the script. If there is an exception, it will return the statuscode 500. 
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Script> UpdateScript([FromBody]Script script) {
            try {
                _dbAccess.Upsert(script);
            } catch (Exception e) {
                _log.LogWarning(e, "Unable to update script");
                return StatusCode(500);
            }
            return Ok(script);
        }

        // This method is a GET method that calls the GetScriptById method from the DBAccess to find the script from the database.
        // Param: an id in the form of string.
        // Return: If the script is found from the database, it will return the help method Ok with the script. If there is an exception, it will return the statuscode 500. 
        // /api/script?id={id}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Script> GetScriptById(string id) {
            Script script;
            try {
                script = _dbAccess.GetScriptById(id);
            } catch (Exception e) {
                _log.LogWarning(e, "Unable to find script by id");
                return StatusCode(500);
            }
            return Ok(script);
        }

        // This method is a GET method that calls the GetByCustomer method from the DBAccess to find a list of scripts from the database.
        // Param: a customer in the form of string.
        // Return: If the scripts are found from the database, it will return the help method Ok with the list of scripts. If there is an exception, it will return the statuscode 500.
        // api/script/customer?customer={customer}
        [HttpGet("Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Script> GetScriptByCustomer(string customer) {
            IEnumerable<Script> scripts = new List<Script>();
            try {
                scripts = _dbAccess.GetByCustomer(customer);

            } catch (Exception e) {
                _log.LogWarning(e, "Unable to find script by customer");
                return StatusCode(500);
            }
            return Ok(scripts);
        }

        // This method is a DELETTE method that calls the Delete method from the DBAccess to delete a script from the database.
        // Param: an id in the form of string.
        // Return: If the script is deleted from the database, it will return the help method Ok with a boolean. If there is an exception, it will return the statuscode 500.
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteScript(string id) {
            bool result;
            try {
                result= _dbAccess.Delete(id);
            } catch (Exception e) {
                _log.LogWarning(e, "Unable to delete script");
                return StatusCode(500);
            }
            return Ok(result);
        }

        // This method is a GET method that calls the GetAll method from the DBAccess to find a list of scripts from the database.
        // Return: If the scripts are found from the database, it will return the help method Ok with the list of scripts. If there is an exception, it will return the statuscode 500.
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Script> GetAllScripts() {
            IEnumerable<Script> scripts = new List<Script>();
            try {
                scripts = _dbAccess.GetAll();
            } catch (Exception e) {
                _log.LogWarning(e, "Unable to find any scripts");
                return StatusCode(500);
            }
            return Ok(scripts);
        }
    }
}