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
    public class Reservation
    {
        public int ReservationId { get; set; }

        // public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_Email_Index", IsUnique = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Active")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public bool Active { get; set; }

        [Display(Name = " Reservation date ")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}
