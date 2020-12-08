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

        public IActionResult OpenScripts() {
            IEnumerable<Script> scripts = scriptController.GetAllScripts();
            return View();

        }

        private IEnumerable<Script> GetAllScripts() {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/script");
            var request = new RestRequest("api/GetAllScripts", Method.GET);
            var response = client.Execute(request);
            string scriptsJson = response.Content;
            List<RootObject> scriptsRoot = JsonConvert.DeserializeObject<List<RootObject>>(scriptsJson);



        }
    }
}
