
using System.ComponentModel.DataAnnotations;

namespace smileRed.Domain
{
    public class Data
    {
        [Key]
        public int DataID { get; set; }

        public int VirtualCounter { get; set; }
    }
}

