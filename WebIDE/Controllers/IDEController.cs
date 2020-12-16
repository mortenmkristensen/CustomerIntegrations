using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using RestSharp;
using WebIDE.ServiceAccess;

namespace WebIDE.Controllers {
    public class IDEController : Controller {
        public ActionResult Index() {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string scriptID2) {
            APIAccess aPIAccess = new APIAccess();
            Script script = aPIAccess.GetScriptById(scriptID2);
            return View("ScriptState", script);
        }

        public ActionResult OpenScripts() {
            APIAccess aPIAccess = new APIAccess();
            List<Script> scripts = aPIAccess.GetAllScripts();
            return View(scripts);

        }
        [HttpPost]
        public ActionResult OpenScripts(string scriptID) {
            APIAccess aPIAccess = new APIAccess();
            List<Script> scripts = new List<Script>();
            Script script = aPIAccess.GetScriptById(scriptID);
            scripts.Add(script);
            return View(scripts);
        }

        public ActionResult SaveScript() {
            return View();
        }
        [HttpPost]
        public ActionResult SaveScript(IFormCollection collection) {
            APIAccess aPIAccess = new APIAccess();
            Script script = new Script();
            string version = collection["version"].ToString();
            script._id = collection["id"].ToString();
            script.Name = collection["scriptName"].ToString();
            script.Language = collection["language"].ToString();
            script.ScriptVersion = Convert.ToDouble(version, CultureInfo.GetCultureInfo("en-US").NumberFormat);
            script.DateCreated = DateTime.Parse(collection["dateCreated"].ToString());
            script.Author = collection["author"].ToString();
            script.LastModified = DateTime.Parse(collection["lastModified"].ToString());
            script.Code = collection["editorContent"].ToString();
            aPIAccess.UploadScript(script);
            Script script2 = aPIAccess.GetScriptById(script._id);
            if (script2 != null) {
                ViewBag.Situation = 0;
                return View(script);
            } else {
                ViewBag.Situation = 1;
                return View();
            }
        }

       

        //Private void DeleteScript(Script script)
    }
}
