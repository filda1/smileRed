using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smileRed.Domain
{
    public class DataContext: DbContext
    {
        public DataContext() : base("DefaultConnection")

        {

        }
    }
}
