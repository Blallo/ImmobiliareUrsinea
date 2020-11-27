using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgenziaMVC.Models;

namespace AgenziaMVC.Controllers
{
    public class ContattiController : Controller
    {
        // GET: Contatti
        public ActionResult Index()
        {
            ContattiModel contatti = new ContattiModel()
            {
                NumeroTelefono = "342 / 3282955",
                Email = "info@ursineaimmobiliare.it"
            };
            return View("Contatti", contatti);
        }
    }
}