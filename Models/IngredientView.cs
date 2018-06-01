using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smileRed.Backend.Models
{
    public class IngredientView
    {
        public Product Product { get; set; }
        public Ingredients Ingredient { get; set; }
        public List<Ingredients> Ingredients { get; set; }
        public List<ProductsIngredients> ProductsIngredients { get; set; }
    }
}