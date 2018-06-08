using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace smileRed.Backend.Models
{
    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<smileRed.Domain.TypeofUser> TypeofUsers { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.User> Users { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Group> Groups { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Product> Products { get; set; }  
        public System.Data.Entity.DbSet<smileRed.Domain.Favorite> Favorites { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.OrderDetails> OrderDetails { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Reservation> Reservations { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Contacts> Contacts { get; set; }
        public System.Data.Entity.DbSet<smileRed.Backend.Controllers.Nutrition> Nutritions { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Offert> Offerts { get; set; }
        public System.Data.Entity.DbSet<smileRed.Domain.Admixtures> Admixtures { get; set; }

        public System.Data.Entity.DbSet<smileRed.Domain.OrderStatus> OrderStatus { get; set; }
    }
}
