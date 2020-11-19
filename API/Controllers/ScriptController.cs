using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;
using API.Database;

namespace API.Controllers{
    public class ScriptController : ApiController{
        IDBAccess dbAccess = new DBAccess(new DBConfig());

        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] string script) {
            try {
                Script deserialzedScript = Deserialize(script);
                dbAccess.Upsert(deserialzedScript);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(HttpRequestMessage request, [FromBody]string script) {
            try {
                Script deserialzedScript = Deserialize(script);
                dbAccess.Upsert(deserialzedScript);
            } catch (Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string id) {
            string json = "";
            try {
                Script script = dbAccess.GetScriptById(id);
                json = Serialize(script);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK, json);
        }

        
        public HttpResponseMessage Delete(HttpRequestMessage request, string id) {
            try {
                dbAccess.Delete(id);
            }catch(Exception e) {
                return request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }

        private Script Deserialize(string json) {
            return JsonConvert.DeserializeObject<Script>(json);
        }

        private string Serialize(Script script) {
            return JsonConvert.SerializeObject(script);
        }

    }
}
