using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModel
{
    public class Container
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public string ContainerName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public long VehicleId { get; set; }
    }
}
