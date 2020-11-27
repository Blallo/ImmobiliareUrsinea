using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using AgenziaMVC.DAL;
using AgenziaMVC.Models;
using MySql.Data;
using System.Data.SqlClient;

namespace AgenziaMVC.Controllers
{
    public class ImmobiliController : Controller
    {
        public ActionResult Immobili()
        {
            var immobili = GetImmobiliList();
            return View(immobili);
        }

        private static List<ImmobiliModel> GetImmobiliList()
        {
            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        EntityDB<ImmobiliModel> entityDb = new EntityDB<ImmobiliModel>();
                        string query = "SELECT * FROM Immobili";
                        SqlCommand command = new SqlCommand(query, session, transaction);
                        SqlDataReader executeSqlDataReader = command.ExecuteReader();
                        List<ImmobiliModel> immobili = new List<ImmobiliModel>();
                        while (executeSqlDataReader.Read())
                        {
                            var immobile = new ImmobiliModel()
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
                        executeSqlDataReader.Close();
                        return immobili;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }


        //Chiamata figa alla web api
        //using (var client = new HttpClient())
        //{
        //    client.BaseAddress = new Uri("http://localhost:57023/api/");
        //    var responseTask = client.GetAsync("Immobili");
        //    responseTask.Wait();
        //    var result = responseTask.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var readTask = result.Content.ReadAsAsync<IList<ImmobiliModel>>();
        //        readTask.Wait();

        //        immobili = readTask.Result;
        //    }
        //    else
        //    {
        //        //Error response received   
        //        immobili = new List<ImmobiliModel>();
        //        ModelState.AddModelError(string.Empty, "Server error try after some time.");
        //    }
        //}



        public ActionResult DisplayInsert()
        {
            return View("View");
        }

        public ActionResult Delete(string id)
        {
            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        EntityDB<ImmobiliModel> entityDb = new EntityDB<ImmobiliModel>();
                        string preQuery = "Delete from Immobili WHERE 1=1 AND id=@id";
                        SqlCommand preCommand = new SqlCommand(preQuery, session, transaction);
                        preCommand.Parameters.AddWithValue("@id", id);
                        preCommand.ExecuteNonQuery();
                        transaction.Commit();

                        var immobiliList = GetImmobiliList();
                        return View("Immobili", immobiliList);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public ActionResult Edit(string id)
        {
            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        EntityDB<ImmobiliModel> entityDb = new EntityDB<ImmobiliModel>();
                        string query = "select * from Immobili WHERE 1=1 AND id=@id";
                        SqlCommand command = new SqlCommand(query, session, transaction);
                        command.Parameters.AddWithValue("@id", id);
                        SqlDataReader executeSqlDataReader = command.ExecuteReader();
                        ImmobiliModel immobili = new ImmobiliModel();
                        while (executeSqlDataReader.Read())
                        {
                            var immobile = new ImmobiliModel()
                            {
                                Id = long.Parse(executeSqlDataReader["Id"].ToString()),
                                TipoImmobile = executeSqlDataReader["TipoImmobile"].ToString(),
                                Descrizione = executeSqlDataReader["Descrizione"].ToString(),
                                Mq = executeSqlDataReader["Mq"].ToString(),
                                Nome = executeSqlDataReader["Nome"].ToString(),
                                Posizione = executeSqlDataReader["Posizione"].ToString(),
                                Prezzo = executeSqlDataReader["Prezzo"].ToString()

                            };
                            List<PictureModel> pic = new List<PictureModel>();
                            string filepath = "~/Images/";
                            var path = Server.MapPath(filepath + immobile.Id);
                            DirectoryInfo d = new DirectoryInfo(path);
                            if (d.Exists)
                            {
                                foreach (var file in d.GetFiles())
                                {

                                    pic.Add(new PictureModel()
                                    {
                                        name = file.Name,
                                        image = System.IO.File.ReadAllBytes(file.FullName),
                                        selected = file.Name.ToLowerInvariant().Contains("copertina") ? true : false
                                    });
                                }

                                immobile.Picture = pic;
                            }
                            immobili = immobile;
                        }
                        executeSqlDataReader.Close();
                        return View("ModifyImmobile", immobili);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public
            ActionResult Crea(ImmobiliModel immobile)
        {
            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        EntityDB<ImmobiliModel> entityDb = new EntityDB<ImmobiliModel>();
                        string preQuery = "SELECT TOP 1 id FROM Immobili " +
                                       " WHERE 1=1  ORDER BY id DESC";

                       // string preQuery = "SELECT * FROM Immobili " +
                         //                 " WHERE 1=1  ORDER BY id DESC LIMIT 1";
                        SqlCommand preCommand = new SqlCommand(preQuery, session, transaction);
                        SqlDataReader executeSqlDataReader = preCommand.ExecuteReader();
                        if (executeSqlDataReader.HasRows)
                        {
                            while (executeSqlDataReader.Read())
                            {
                                immobile.Id = long.Parse(executeSqlDataReader["Id"].ToString()) + 1;
                            }
                        }
                        else
                        {
                            immobile.Id = 1;
                        }
                        executeSqlDataReader.Close();
                        string query =
                            "INSERT INTO Immobili (Id, Nome, Descrizione, Mq, Prezzo, TipoImmobile, Posizione) VALUES (@id, @nome, @descrizione, @mq, @prezzo, @tipoImmobile, @posizione)";
                        SqlCommand command = new SqlCommand(query, session, transaction);
                        command.Parameters.AddWithValue("@id", immobile.Id);
                        command.Parameters.AddWithValue("@descrizione", immobile.Descrizione);
                        command.Parameters.AddWithValue("@nome", immobile.Nome);
                        if (!string.IsNullOrEmpty(immobile.Mq))
                        {
                            command.Parameters.AddWithValue("@mq", immobile.Mq);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@mq", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@prezzo", immobile.Prezzo);
                        command.Parameters.AddWithValue("@tipoImmobile", immobile.TipoImmobile);
                        command.Parameters.AddWithValue("@posizione", immobile.Posizione);
                        command.ExecuteNonQuery();
                        transaction.Commit();

                        var immobiliList = GetImmobiliList();
                        return View("Immobili", immobiliList);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }



            }
        }

        public
          ActionResult Update(ImmobiliModel immobile)
        {
            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        string query =
                            "Update Immobili set nome=@nome, descrizione=@descrizione, mq=@mq, prezzo=@prezzo, tipoImmobile=@tipoImmobile, posizione=@posizione where id=@id";
                        SqlCommand command = new SqlCommand(query, session, transaction);
                        command.Parameters.AddWithValue("@id", immobile.Id);
                        command.Parameters.AddWithValue("@descrizione", immobile.Descrizione);
                        command.Parameters.AddWithValue("@nome", immobile.Nome);
                        if (!string.IsNullOrEmpty(immobile.Mq))
                        {
                            command.Parameters.AddWithValue("@mq", immobile.Mq);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@mq", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@prezzo", immobile.Prezzo);
                        command.Parameters.AddWithValue("@tipoImmobile", immobile.TipoImmobile);
                        command.Parameters.AddWithValue("@posizione", immobile.Posizione);
                        command.ExecuteNonQuery();
                        transaction.Commit();

                        var immobiliList = GetImmobiliList();
                        return View("Immobili", immobiliList);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }



            }
        }


        [HttpPost]
        public String SetCopertina(string picName, string id)
        {
            try
            {

                string filepath = "~/Images/";

                var path = Server.MapPath(filepath + id)+"\\";
                DirectoryInfo d = new DirectoryInfo(path);
                if (d.Exists)
                {
                    foreach (var file in d.GetFiles())
                    {
                        if (file.Name.Contains(picName))
                        {
                            if (!file.Name.Contains("_"))
                            {
                                var extension = "."+file.Name.Split('.')[1];
                                var name= file.Name.Split('.')[0];
                                
                                System.IO.File.Move(path+name + extension, path + name + "_copertina"+ extension);

                            }
                        }
                        else
                        {
                            if (file.Name.Contains("_"))
                            {
                                var extension = "."+file.Name.Split('.')[1];
                                var name = file.Name.Split('.')[0];
                                System.IO.File.Move(path + file.Name, path + name.Split('_')[0]+extension);

                            }
                        }
                    }
                }

               return "success";
             
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult AggiungiFoto(IEnumerable<HttpPostedFileBase> file, string id)
        {
            string filepath = "~/Images/";
            var path = Server.MapPath(filepath + id);
            DirectoryInfo d = new DirectoryInfo(path);
            if (!d.Exists)
            {
                d.Create();
            }
            int i = 0;
            foreach (var f in file)
            {

                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(f.FileName);
                    string imgPath = System.IO.Path.Combine(path, pic);
                    f.SaveAs(imgPath);
                }
            }


            using (var session = DatabaseUtilities.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        EntityDB<ImmobiliModel> entityDb = new EntityDB<ImmobiliModel>();
                        string query = "SELECT * FROM Immobili " +
                                       " WHERE id=@id";
                        SqlCommand command = new SqlCommand(query, session, transaction);
                        command.Parameters.AddWithValue("@id", id);
                        SqlDataReader executeSqlDataReader = command.ExecuteReader();
                        ImmobiliModel immobili = entityDb.DataReaderMapToList<ImmobiliModel>(executeSqlDataReader);
                        executeSqlDataReader.Close();
                        if (immobili != null)
                        {
                            List<PictureModel> pic = new List<PictureModel>();

                            if (d.Exists)
                            {
                                foreach (var f in d.GetFiles())
                                {


                                    pic.Add(new PictureModel()
                                    {
                                        name = f.Name,
                                        image = System.IO.File.ReadAllBytes(f.FullName)
                                    });
                                }
                                immobili.Picture = pic;
                            }
                            return PartialView("RiepilogoImmobile", immobili);
                        }
                        return PartialView("RiepilogoImmobile", new ImmobiliModel());
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        return PartialView("RiepilogoImmobile", new ImmobiliModel());
                    }
                }
            }
        }
    }
}
