using API.Models;
using API.Database;
using System.Net.Http;
using System.Net;
using System;
using System.Web.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.Controllers {
    public class ScriptController : ApiController{
        IDBAccess dbAccess = new DBAccess(new DBConfig());

        [HttpPost]
        public HttpResponseMessage UploadScript(HttpRequestMessage request, [FromBody] string script) {
            try {
                Script deserialzedScript = Deserialize(script);
                dbAccess.Upsert(deserialzedScript);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage UpdateScript(HttpRequestMessage request, [FromBody]string script) {
            try {
                Script deserialzedScript = Deserialize(script);
                dbAccess.Upsert(deserialzedScript);
            } catch (Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage GetScriptById(HttpRequestMessage request, string id) {
            string json = "";
            try {
                Script script = dbAccess.GetScriptById(id);
                json = Serialize(script);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK, json);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteScript(HttpRequestMessage request, string id) {
            try {
                dbAccess.Delete(id);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);<
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

       
        private Script Deserialize(string json) {
            return JsonConvert.DeserializeObject<Script>(json);
        }

        private string Serialize(Script script) {
            return JsonConvert.SerializeObject(script);
        }
        [HttpGet]
        public HttpResponseMessage GetScriptByCustomer(HttpRequestMessage request, string customer) {
            IEnumerable<Script> scripts = new List<Script>();
            try {
                scripts = dbAccess.GetByCustomer(customer);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK,scripts);
        }
    }
}
 