using System.Collections.Generic;

namespace Entity.DTOs
{
    public class GroupModelForContainers
    {
        public int GroupNumber { get; set; }
        public List<ContainerViewModel> Containers { get; set; }
    }
}
    