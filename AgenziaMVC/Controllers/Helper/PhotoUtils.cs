using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace AgenziaMVC.Controllers.Helper
{
    public static class PhotoUtils
    {

        private static string basePath = "~/images/";
        public static void GetImages(string relativePath, HtmlGenericControl innerControl)
        {
            string vPath = basePath + relativePath;
            string path = HttpContext.Current.Server.MapPath(vPath);
            DirectoryInfo d = new DirectoryInfo(path); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            int i = 0;
            foreach (FileInfo file in Files)
            {


                if (file.Name.ToLower().EndsWith("jpg") || file.Name.ToLower().EndsWith("png"))
                {
                    HtmlImage image = new HtmlImage()
                    {
                        Src = vPath + file.Name,
                    };

                    if (innerControl.TagName == "ul")
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        if (i > 0)
                        {
                            li.Attributes.Add("hidden", "hidden");
                        }
                        innerControl.Controls.Add(li);
                        li.Controls.Add(image);
                    }
                    else
                    {
                        image.Attributes["class"] = "item fill";
                        innerControl.Controls.Add(image);
                    }
                }
                i = i + 1;
            }
            if (innerControl.Controls.Count > 0 && innerControl.TagName != "ul")
            {
                HtmlImage first = (HtmlImage)innerControl.Controls[0];
                first.Attributes["class"] = "item active fill";
            }
        }
    }
}