using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
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
        public ActionResult Index(string scriptID) {
            List<Script> scripts = new List<Script>();
            Script script = GetScriptById(scriptID);
            scripts.Add(script);
            return RedirectToAction("OpenScripts", scripts);

        }
        public ActionResult OpenScripts() {
           List<Script> scripts = GetAllScripts();
            return View(scripts);

        }
        [HttpPost]
        public ActionResult OpenScripts(string scriptID2) {
            Script script = GetScriptById(scriptID2);
            return View("ScriptState", script);
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
    }
}
