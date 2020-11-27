using System.IO;
using System.Web.Mvc;
using AgenziaMVC.Models;

namespace AgenziaMVC.Controllers
{
    public class ChiSiamoController : Controller
    {
        public ActionResult Index()
        {
            ChiSiamoModel chiSiamo = new ChiSiamoModel();
            chiSiamo.Name = "Chi Siamo";
            chiSiamo.TextBefore =
                "L'agenzia immobiliare ursinea nasce nel 1982, con quasi 40 anni di presenza sul mercato, è "+
                "un punto di riferimento per tutti coloro che desiderano acquistare una casa in Toscana," +
                "da Pitigliano fino alle coste del Monte Argentario";
            chiSiamo.TextAfter = "Avvalendosi di uno staff qualificato, ti segue nella ricerca dell'immobile, nella scelta della migliore forma di finanziamento, fornendo validi consigli riguardo alla ristrutturazione del tuo nuovo immobile, accompagnandoti in tutte le operazioni che vorrai realizzare! ";
            chiSiamo.TextFinally="Immobiliare Ursinea realizza il vostro sogno!";
            string filepath = "~/Images/Shared/ChiSiamo";
            var path = Server.MapPath(filepath);
            DirectoryInfo d = new DirectoryInfo(path);
            if (d.Exists)
            {
                foreach (var file in d.GetFiles())
                {
                        chiSiamo.ImgBytes = System.IO.File.ReadAllBytes(file.FullName);
                }
            }
            return View("ChiSiamo",chiSiamo);
        }

    }
}