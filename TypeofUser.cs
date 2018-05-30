
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smileRed.Domain
{
    public class TypeofUser
    {
        [Key]
        public int TypeofUserId { get; set; }

        [Display(Name = "Type of User")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string TypeofUsers { get; set; }

        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Index("Product_Description_Index", IsUnique = false)]
        public string Description { get; set; }

        [Display(Name = "Active")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}
