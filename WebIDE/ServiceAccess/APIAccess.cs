using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using RestSharp;

namespace WebIDE.ServiceAccess {
    public class APIAccess {

        public List<Script> GetAllScripts() {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script/");
            var request = new RestRequest("all", Method.GET);
            var response = client.Execute(request);
            string scriptsJson = response.Content;
            List<Script> scripts = JsonConvert.DeserializeObject<List<Script>>(scriptsJson);
            return scripts;
        }

        public Script GetScriptById(string id) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest($"?id={id}", Method.GET);
            var response = client.Execute(request);
            string scriptJson = response.Content;
            Script script = JsonConvert.DeserializeObject<Script>(scriptJson);
            return script;

        }

        public void UploadScript(Script script) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(script);
            request.RequestFormat = DataFormat.Json;
            client.Execute(request);
        }

        public void DeleteScript(string id) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest($"?id={id}", Method.DELETE);
            client.Execute(request);
        }
    }
}
