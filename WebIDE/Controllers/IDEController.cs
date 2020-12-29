using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebIDE.ServiceAccess;

namespace WebIDE.Controllers {
    public class IDEController : Controller {
        public ActionResult Index() {
            return View();
        }

        //This method calls the method GetScriptById from APIAccess to find a script. After that it shows the state of the script in the view.
        //Param: a scrpt's id in the form of string.
        //Return: Is a view.
        [HttpPost]
        public ActionResult ScriptState(string scriptID2) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
            script = aPIAccess.GetScriptById(scriptID2);
            if (script != null) {
                return View(script);
            } else {
                ViewBag.Situation = 1;
                return View("EditScript");
            }
        }

        //This method calls the method GetAllScripts from APIAccess to get the data of all the scripts.
        //Return: Is a view.
        public ActionResult OpenScripts() {
            APIAccess aPIAccess = new APIAccess();
            List<Script> scripts = aPIAccess.GetAllScripts();
            return View(scripts);
        }

        //This method calls the method GetScriptById from APIAccess to find a script.
        //Param: a script's id in the form of string.
        //Return: Is a view.
        [HttpPost]
        public ActionResult SearchScriptById(string scriptID) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
            List<Script> scripts = new List<Script>();
            script = aPIAccess.GetScriptById(scriptID);
            if (script != null) {
                scripts.Add(script);
                return View("OpenScripts", scripts);
            } else {
                ViewBag.Situation = 1;
                return View("EditScript");
            }
        }

        //This method creats a new script and calls the method UploadScript from APIAccess to save the script in the database.
        //Param: an IFormCollection object.
        //Return: Is a view.
        [HttpPost]
        public ActionResult SaveScript(IFormCollection collection) {
            APIAccess aPIAccess = new APIAccess();
            Script script = new Script();
            script.Id = collection["id"].ToString();
            script.Name = collection["scriptName"].ToString();
            script.Language = collection["language"].ToString();
            script.ScriptVersion = collection["version"].ToString();
            script.DateCreated = DateTime.Parse(collection["dateCreated"].ToString());
            script.Author = collection["author"].ToString();
            script.LastModified = DateTime.Parse(collection["lastModified"].ToString());
            script.Code = collection["editorContent"].ToString();
            Script insertedScript = aPIAccess.UploadScript(script);
            if (insertedScript != null) {
                ViewBag.Situation = 0;
                return View(insertedScript);
            } else {
                ViewBag.Situation = 1;
                return View();
            }
        }

        //This method calls the method DeleteScript from APIAccess to delete the script from the database.
        //Param: a script's id in the form of string.
        //Return: Is a view.
        [HttpPost]
       public ActionResult DeleteScript(string scriptID4) {
            APIAccess aPIAccess = new APIAccess();
            bool result = aPIAccess.DeleteScript(scriptID4);
            if (result) {
                ViewBag.Situation = 0;
                return View();
            } else {
                ViewBag.Situation = 1;
                return View();
            }
        }

        //This method calls the method GetScriptById from APIAccess to find a script. After that show the data of the script in the view to edit.
        //Param: a script's id in the form of string.
        //Return: Is a view.
        [HttpPost]
        public ActionResult EditScript(string scriptID3) {
            APIAccess aPIAccess = new APIAccess();
            Script script = null;
            script = aPIAccess.GetScriptById(scriptID3);
            if (script != null) {
                ViewBag.Situation = 0;
                return View(script); ;
            } else {
                ViewBag.Situation = 1;
                return View();
            }
        }
    }
}
