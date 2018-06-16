using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smileRed.API.Models
{
    public class OffertsView : Offert
    {
        [Index("Product_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        //[Required(ErrorMessage = "You must enter the field {0}")]
        public decimal Price { get; set; }

        [Display(Name = "IVA (%)")]
        public decimal VAT { get; set; }

        public double Stock { get; set; }
    }
}