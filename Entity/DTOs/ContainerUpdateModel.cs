using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.DTOs
{
    public class ContainerUpdateModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string ContainerName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
    }
}
