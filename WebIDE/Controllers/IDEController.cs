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
        public ActionResult ScriptState(string scriptID2) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
            if (scriptID2 != "" && scriptID2 != null) {
                script = aPIAccess.GetScriptById(scriptID2);
                if (script != null) {
                    return View(script);
                } else {
                    ViewBag.Message = "Id is wrong , provide an correct Id!";
                    List<Script> scripts2 = aPIAccess.GetAllScripts();
                    return View("OpenScripts", scripts2);
                }
            } else {
                ViewBag.Message = "Provide an Id!";
                List<Script> scripts = aPIAccess.GetAllScripts();
                return View("OpenScripts", scripts);
            }
        }

        public ActionResult OpenScripts() {
            APIAccess aPIAccess = new APIAccess();
            List<Script> scripts = aPIAccess.GetAllScripts();
            return View(scripts);

        }
        [HttpPost]
        public ActionResult SearchScriptById(string scriptID) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
            if (scriptID != "" && scriptID != null) {
                List<Script> scripts = new List<Script>();
                script = aPIAccess.GetScriptById(scriptID);
                if (script != null) {
                    scripts.Add(script);
                    return View("OpenScripts", scripts);
                } else {
                    ViewBag.Message = "Id is wrong , provide an correct Id!";
                    List<Script> scripts2 = aPIAccess.GetAllScripts();
                    return View("OpenScripts", scripts2);
                }
                
            } else {
                ViewBag.Message = "Provide an Id!";
                List<Script> scripts = aPIAccess.GetAllScripts();
                return View("OpenScripts", scripts);
            }
        }

        [HttpPost]
        public ActionResult SaveScript(IFormCollection collection) {
            APIAccess aPIAccess = new APIAccess();
            Script script = new Script();
            script._id = collection["id"].ToString();
            script.Name = collection["scriptName"].ToString();
            script.Language = collection["language"].ToString();
            script.ScriptVersion = collection["version"].ToString();
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

      
       [HttpPost]
       public ActionResult DeleteScript(string scriptID4) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
          
                script = aPIAccess.GetScriptById(scriptID4);
                aPIAccess.DeleteScript(scriptID4);
                Script script1 = aPIAccess.GetScriptById(scriptID4);
                if (script != null && script1 == null) {
                    ViewBag.Situation = 0;
                    return View(script);
                } else {
                    ViewBag.Situation = 1;
                    return View();
                }
        }

        [HttpPost]
        public ActionResult EditScript(string scriptID3) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
            if(scriptID3 != "" && scriptID3 != null) {
                script = aPIAccess.GetScriptById(scriptID3);
                if (script != null) {
                    ViewBag.Situation = 0;
                    return View(script); ;
                } else {
                    ViewBag.Situation = 1;
                    return View();
                }
            } else {
                ViewBag.Message = "Select one script!";
                List<Script> scripts = aPIAccess.GetAllScripts();
                return View("OpenScripts", scripts);
            }
        }
    }
}
