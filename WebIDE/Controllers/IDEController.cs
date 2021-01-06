using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebIDE.ServiceAccess;

namespace WebIDE.Controllers {
    public class IDEController : Controller {
        private IAPIAccess aPIAccess;

        public IDEController(IAPIAccess aPIAccess2) {
            aPIAccess = aPIAccess2;
        }

        public IDEController() {
            aPIAccess = new APIAccess();
        }
        public ActionResult Index() {
            return View();
        }

        //This method calls the method GetScriptById from APIAccess to find a script. After that it shows the state of the script in the view.
        //Param: a scrpt's id in the form of string.
        //Return: Is a view.
        [HttpPost]
        public ActionResult ScriptState(string scriptID2) {
            Script script = null;
            script = aPIAccess.GetScriptById(scriptID2);
            if (script != null) {
                return View("ScriptState", script);
            } else {
                ViewBag.Situation = 1;
                return View("EditScript");
            }
        }

        //This method calls the method GetAllScripts from APIAccess to get the data of all the scripts.
        //Return: Is a view.
        public ActionResult OpenScripts() {
            List<Script> scripts = aPIAccess.GetAllScripts();
            return View("OpenScripts", scripts);
        }

        //This method calls the method GetScriptById from APIAccess to find a script.
        //Param: a script's id in the form of string.
        //Return: Is a view.
        [HttpPost]
        public ActionResult SearchScriptById(string scriptID) {
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
                return View("SaveScript", insertedScript);
            } else {
                ViewBag.Situation = 1;
                return View("SaveScript");
            }
        }

        //This method calls the method DeleteScript from APIAccess to delete the script from the database.
        //Param: a script's id in the form of string.
        //Return: Is a view.
        [HttpPost]
       public ActionResult DeleteScript(string scriptID4) {
            bool result = aPIAccess.DeleteScript(scriptID4);
            if (result) {
                ViewBag.Situation = 0;
                return View("DeleteScript");
            } else {
                ViewBag.Situation = 1;
                return View("DeleteScript");
            }
        }

        //This method calls the method GetScriptById from APIAccess to find a script. After that show the data of the script in the view to edit.
        //Param: a script's id in the form of string.
        //Return: Is a view.
        [HttpPost]
        public ActionResult EditScript(string scriptID3) {
            Script script = null;
            script = aPIAccess.GetScriptById(scriptID3);
            if (script != null) {
                ViewBag.Situation = 0;
                return View("EditScript", script); ;
            } else {
                ViewBag.Situation = 1;
                return View("EditScript");
            }
        }
    }
}
