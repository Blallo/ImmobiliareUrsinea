using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgenziaMVC.Models;

namespace AgenziaMVC.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            IndexModel indexModel = new IndexModel();
            string filepath = "~/Images/Shared/Index";
            var path = Server.MapPath(filepath);
            DirectoryInfo d = new DirectoryInfo(path);
            if (d.Exists)
            {
                foreach (var file in d.GetFiles())
                {
                    if (file.Name.Contains("Pitigliano"))
                    {
                        indexModel.DefaultImage = System.IO.File.ReadAllBytes(file.FullName);
                        indexModel.DefaultImageName = file.Name;
                    }

                }                
            }
            return View(indexModel);
        }
    }
}