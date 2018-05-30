

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smileRed.Domain
{
    public class Contacts
    {
        [Key]
        public int ContactsId { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Company { get; set; }

     
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Index("Product_Description_Index", IsUnique = true)]
        public string Description { get; set; }

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

        [Display(Name = "Open")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public bool Active { get; set; }
    }
}
