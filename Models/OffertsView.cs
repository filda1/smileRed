using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smileRed.Backend.Models
{
    public class OffertsView
    {
        public Product Product { get; set; }
        public Product ProductId { get; set; }
        public Offert Offert { get; set; }
        public List<Offert> Offerts { get; set; }
        public List<ProductsOfferts> ProductsOfferts { get; set; }
    }
}