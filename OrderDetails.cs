using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smileRed.Domain
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Quantity { get; set; }

        /*[DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get { return Price * (decimal)Quantity; } }*/

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }

        [Display(Name = "Visible")]
        public bool VisibleOrderDetails{ get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool ActiveOrderDetails { get; set; }

        public virtual Product Product { get; set; }
       
        public virtual Order Order { get; set; }
    }
}
