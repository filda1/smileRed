using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smileRed.Domain
{
    public class Order
    {
        [Key]
        [Display(Name = "OrderID")]
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_Email_Index", IsUnique = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = " Orders date ")]
        [DataType(DataType.DateTime)]
        public DateTime  DateOrder { get; set; }

        [Display(Name = "Visible")]
        public bool VisibleOrders { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool ActiveOrders { get; set; }

        public virtual User Customer { get; set; }
        public virtual ICollection<OrderDetails> Details { get; set; }
    }
}
