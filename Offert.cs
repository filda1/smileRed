using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smileRed.Domain
{
    public class Offert
    {
        [Key]
        public int OffertId { get; set; }

        //public int ProductId { get; set; }

        [Display(Name = "Offer")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Index("Product_Description_Index", IsUnique = true)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "You must enter the field {0}")]
        public decimal Price { get; set; }

        public string Image { get; set; }

        public double Stock { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End of Date ")]
        [DataType(DataType.DateTime)]
        public DateTime EndofDate { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

       /* [JsonIgnore]
        public virtual Product Product { get; set; }*/
    }
}
