using System.Collections.Generic;
using System.Threading.Tasks;
using Data.DataModel;
using Data.Generic;
using Entity.DTOs;

namespace Data.Repositories.ContainerRepo
{
    public interface IContainerRepository:IGenericRepository<Container>
    {
        Task<IEnumerable<Container>> GetByVehicleId(long id);
        Task<List<GroupModelForContainers>> GroupByVehicleId(long id, int n);
        GroupModelForContainers GroupModelCreator(int n, List<ContainerViewModel> containers);
        ContainerViewModel ConvertToViewModel(Container entity);
    }
}
