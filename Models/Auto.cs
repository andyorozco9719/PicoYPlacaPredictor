using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PicoyPlacaPredictor.Models
{
    public class Auto
    {
        [Required]
        [StringLength(8, ErrorMessage = "Escriba este formato de placa: XXX-0000")]
        [Display(Name = "Placa: ")]
        public string placa { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Escriba este formato de fecha: dd/mm/yyyy")]
        [Display(Name = "Fecha: ")]
        public string fecha { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "Escriba este formato de hora: hh:mm")]
        [Display(Name = "Hora: ")]
        public string hora { get; set; }
        public string mensaje { get; set; }
    }
}