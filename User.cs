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
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public int TypeofUserId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_Email_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(300, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Address { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Location { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int Code { get; set; }

        [Display(Name = "Nº Door")]
        [Required(ErrorMessage = "The field {0} is requiered.")]       
        public int Door { get; set; }
     
        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }
                return string.Format(
                    "http://landsapi1.azurewebsites.net/{0}",
                    ImagePath.Substring(1));
            }
        }

        [Display(Name = "User")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        [Display(Name = "Active")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual TypeofUser TypeofUser { get; set; }

        [JsonIgnore]
        public virtual Reservation Reservation { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }

        [JsonIgnore]
        public virtual ICollection<Favorite> Favorite { get; set; }
    }
}
