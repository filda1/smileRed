using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smileRed.Backend.Models
{
    public class ProductsIngredients
    {
        [NotMapped]
        public int ProductId { get; set; }

        [NotMapped]
        public int CategoryId { get; set; }

        [NotMapped]
        public int IngredientId { get; set; }

        [NotMapped]
        [Display(Name = "Category")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Category { get; set; }

        [NotMapped]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "Ingredient")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Ingredient { get; set; }
    }
}