using Models;
using RestSharp;
using System;

namespace TestAPI2 {
    class Program {
        static void Main(string[] args) {
            Script script = new Script();
            script._id = "10";
            script.Name = "Test";
            string response = UploadScript(script);
            Console.WriteLine(response);
            Console.ReadLine();
        }
        private static string UploadScript(Script script) {
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
