using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smileRed.Domain
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        //public int? UserId { get; set; }

        [Display(Name = "ProductId")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_Email_Index", IsUnique = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Active")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public bool Active { get; set; }

        [Display(Name = "Favorite Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FavoriteDate { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
