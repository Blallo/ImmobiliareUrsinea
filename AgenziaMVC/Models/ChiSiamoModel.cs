using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenziaMVC.Models
{
    public class ChiSiamoModel
    {
        public string Name { get; set; }
        public string TextBefore { get; set; }
        public string TextAfter { get; set; }
        public string TextFinally { get; set; }
        public byte[] ImgBytes { get; set; }

    }
}