using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smileRed.Domain
{
    public class Ingredients
    {
        [Key]
        public int IngredientsId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Ingredient")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(80, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Ingredient { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
