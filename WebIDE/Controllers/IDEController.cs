using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using RestSharp;

namespace WebIDE.Controllers {
    public class IDEController : Controller {
        public ActionResult Index() {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string scriptID2) {
            Script script = GetScriptById(scriptID2);
            return View("ScriptState", script);
        }

        public ActionResult OpenScripts() {
           List<Script> scripts = GetAllScripts();
            return View(scripts);

        }
        [HttpPost]
        public ActionResult OpenScripts(string scriptID) {
            List<Script> scripts = new List<Script>();
            Script script = GetScriptById(scriptID);
            scripts.Add(script);
            return View(scripts);
        }
       
        public ActionResult SaveScript() {
            return View();
        }
        [HttpPost]
        public ActionResult SaveScript(IFormCollection collection) {
            Script script = new Script();
            string id = collection["id"].ToString();
            string scriptName = collection["scriptName"].ToString();
            string language = collection["language"].ToString();
            string version = collection["version"];
            string dateCreatedString = collection["dateCreated"].ToString();
            string author = collection["author"].ToString();
            string lastModifiedString = collection["lastModified"].ToString();
            string code = collection["textEditor"].ToString();
            DateTime dateCreated = DateTime.Parse(dateCreatedString);
            DateTime lastModified = DateTime.Parse(lastModifiedString);
            script._id = id;
            script.Name = scriptName;
            script.Language = language;
            script.ScriptVersion = Convert.ToDouble(version);
            script.DateCreated = dateCreated;
            script.Author = author;
            script.LastModified = lastModified;
            script.Code = code;
            UploadScript(script);
            return View(script);
        }

        private List<Script> GetAllScripts() {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script/");
            var request = new RestRequest("all", Method.GET);
            var response = client.Execute(request);
            string scriptsJson = response.Content;
            List<Script> scripts = JsonConvert.DeserializeObject<List<Script>>(scriptsJson);
            return scripts;
        }

        private Script GetScriptById(string id) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest($"?id={id}", Method.GET);
            var response = client.Execute(request);
            string scriptJson = response.Content;
            Script script = JsonConvert.DeserializeObject<Script>(scriptJson);
            return script;

        }

        private void UploadScript(Script script) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(script);
            request.RequestFormat = DataFormat.Json;
            client.Execute(request);
        }
    }
}
