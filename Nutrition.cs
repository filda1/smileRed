using Newtonsoft.Json;
using smileRed.Domain;
using System.ComponentModel.DataAnnotations;

namespace smileRed.Backend.Controllers
{
    public class Nutrition
    {
        [Key]
        public int NutritionId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Calories (k)")]
        public int Calories { get; set; }

        [Display(Name = "Total Fat (g)")]
        public int TotalFat { get; set; }

        [Display(Name = "Carbohydrates (g)")]
        public int Carbohydrates { get; set; }

        [Display(Name = "Proteins (g)")]
        public int Proteins { get; set; }
    }
}