using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenziaMVC.Models
{
    public class ImmobiliModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        [DataType(DataType.MultilineText)]
        public string Descrizione { get; set; }
        public string Mq { get; set; }
        public string TipoImmobile { get; set; }
        public string Prezzo { get; set; }
        public List<PictureModel> Picture { get; set; }
        public string Posizione { get; set; }
        
    }
}