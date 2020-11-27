using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AgenziaMVC.Models;
using AgenziaMVC;
using AgenziaMVC.Controllers.Helper;
using AgenziaMVC.DAL;
using System.Data.SqlClient;

namespace AgenziaMVC.Controllers
{
    public class RicercaImmobiliController : Controller
    {
        public ActionResult Index()
        {
            return View("RicercaImmobili");
        }

        public ActionResult RicercaImmobili(FormCollection form)
        {
            string tipoImmobile = form["TipoImmobile"].ToString();
            string posizione = form["Posizione"].ToString();
            string mq = form["Mq"].ToString();
            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        EntityDB<RicercaImmobiliModel> entityDb = new EntityDB<RicercaImmobiliModel>();
                        string query = "SELECT * FROM Immobili " +
                                       " WHERE 1=1";
                        if (!string.IsNullOrEmpty(tipoImmobile))
                        {
                            query = query + " AND TipoImmobile=@tipoImmobile";

                        }
                        if (!string.IsNullOrEmpty(mq))
                        {
                            query = query + " AND Mq=@mq";
                        }
                        if (!string.IsNullOrEmpty(posizione))
                        {
                            query = query + " AND Posizione=@posizione";
                        }
                        SqlCommand command = new SqlCommand(query, session, transaction);
                        if (!string.IsNullOrEmpty(tipoImmobile))
                        {
                            command.Parameters.AddWithValue("@tipoImmobile", tipoImmobile);
                        }
                        if (!string.IsNullOrEmpty(mq))
                        {
                            command.Parameters.AddWithValue("@mq", mq);
                        }
                        if (!string.IsNullOrEmpty(posizione))
                        {
                            command.Parameters.AddWithValue("@posizione", posizione);
                        }
                        SqlDataReader executeSqlDataReader = command.ExecuteReader();
                        List<RicercaImmobiliModel> immobili = new List<RicercaImmobiliModel>();
                        while (executeSqlDataReader.Read())
                        {
                            var immobile = new RicercaImmobiliModel()
                            {
                                Id = long.Parse(executeSqlDataReader["Id"].ToString()),
                                TipoImmobile = executeSqlDataReader["TipoImmobile"].ToString(),
                                Descrizione = executeSqlDataReader["Descrizione"].ToString(),
                                Mq = executeSqlDataReader["Mq"].ToString(),
                                Nome = executeSqlDataReader["Nome"].ToString(),
                                Posizione = executeSqlDataReader["Posizione"].ToString(),
                                Prezzo = executeSqlDataReader["Prezzo"].ToString()

                            };
                            immobili.Add(immobile);
                        }

                        //entityDb.DataReaderMapList<RicercaImmobiliModel>(executeSqlDataReader);
                       
                       
                      
                        executeSqlDataReader.Close();
                        return View(immobili);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return View(new List<RicercaImmobiliModel>());
                    }
                }
            }
        }

        public FileContentResult ImageGenerate(string id)
        {

            try
            {
                if (id != null)
                {
                    if (id.Contains(";"))
                        id = id.Replace(";", "");
                        string filepath = "~/Images/";
                List<PictureModel> pic = new List<PictureModel>();
                var path = Server.MapPath(filepath + id);
                DirectoryInfo d = new DirectoryInfo(path);
                if (d.Exists)
                {
                    foreach (var file in d.GetFiles())
                    {
                        if (file.Name.ToLower().Contains("copertina"))
                        {
                            return new FileContentResult(System.IO.File.ReadAllBytes(file.FullName), "image/jpeg");
                          
                           
                        }
                     
                    }
                }

                return null;


            }
                return null;
            }
            catch (Exception e)
            {

             
                throw;
            }

        }

        [HttpPost]
        public JsonResult GetPhoto(string id)
        {
            try
            {

                string filepath = "~/Images/";


                List<PictureModel> pic = new List<PictureModel>();
                var path = Server.MapPath(filepath + id);
                DirectoryInfo d = new DirectoryInfo(path);
                List<string> photos = new List<string>();
                if (d.Exists)
                {
                    foreach (var file in d.GetFiles())
                    {

                        photos.Add(Convert.ToBase64String(System.IO.File.ReadAllBytes(file.FullName)));
                     
                      
                       
                    }
                  
                }
                var jsonResult = Json(new { data = photos }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
                

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}


