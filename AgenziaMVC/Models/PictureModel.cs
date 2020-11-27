using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenziaMVC.Models
{
    public class PictureModel
    {
        public byte[] image { get; set; }

        public string name { get; set; }

        public string source { get; set; }

        public bool selected { get; set; }
    }
}