using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AgenziaMVC.DAL
{
    public class EntityDB<T>
    {

        public T DataReaderMapToList<T>(IDataReader dr)
        {
            try
            {
                //List<T> list = new List<T>();
                T obj = default(T);
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    //list.Add(obj);
                }
                return obj;
                //return list;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> DataReaderMapList<T>(IDataReader dr)
        {
            try
            {
                List<T> list = new List<T>();
                T obj = default(T);
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    list.Add(obj);
                }
                dr.Close();
                return list;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}