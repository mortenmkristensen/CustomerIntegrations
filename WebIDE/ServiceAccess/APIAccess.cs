using System;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using RestSharp;

namespace WebIDE.ServiceAccess {
    public class APIAccess : IAPIAccess {
        //This method sends a GET request to API to get data of all the scripts from API.
        //Return: Is a list of scripts.
        public List<Script> GetAllScripts() {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script/");
            var request = new RestRequest("all", Method.GET);
            var response = client.Execute(request);
            string scriptsJson = response.Content;
            List<Script> scripts = JsonConvert.DeserializeObject<List<Script>>(scriptsJson);
            return scripts;
        }

        //This method sends a GET request to API to get data of a script from API.
        //Param: an id in the form of string.
        //Return: Is a script object.
        public Script GetScriptById(string id) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest($"?id={id}", Method.GET);
            var response = client.Execute(request);
            string scriptJson = response.Content;
            if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                return null;
            } else { 
                Script script = JsonConvert.DeserializeObject<Script>(scriptJson);
                return script;
            }

        }

        //This method sends a POST request to API to save data of a script in the database.
        //Param: a script object.
        //Return: Is a script object.
        public Script UploadScript(Script script) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(script);
            var response = client.Execute(request);
            string scriptJson = response.Content;
            Script returnScript = JsonConvert.DeserializeObject<Script>(scriptJson);
            return returnScript;
        }

        //This method sends a DELETE request to API to delete a script from the database.
        //Param: an id in the form of string.
        //Return: Is a boolean.
        public bool DeleteScript(string id) {
            bool result = false;
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest($"?id={id}", Method.DELETE);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK){
                result = bool.Parse(response.Content);
            }
            return result;
        }
    }
}
