using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;


namespace TestAPI {
    class Program {
        static void Main(string[] args) {
            Script script = new Script();
        }

        private string UploadScript(Script script) {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://localhost:44321/api/script");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(script);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
            return response.Content;
        }
    }
}
