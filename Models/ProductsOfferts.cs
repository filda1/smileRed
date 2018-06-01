using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smileRed.Backend.Models
{
    public class ProductsOfferts
    {
        [NotMapped]
        public int ProductId { get; set; }

        [NotMapped]
        public int OffertId { get; set; }

        [NotMapped]
        [Display(Name = "product")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Product { get; set; }

        [NotMapped]
        [Display(Name = "Offert")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Offert { get; set; }

        [NotMapped]
        [Display(Name = "Descripttion")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Descripttion { get; set; }

        [NotMapped]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "You must enter the field {0}")]
        public decimal Price { get; set; }

        [NotMapped]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [NotMapped]
        [Display(Name = "End of Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndofDate { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [NotMapped]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
    }
}