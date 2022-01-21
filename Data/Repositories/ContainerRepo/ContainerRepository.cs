using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Data.Context;
using Data.DataModel;
using Data.Generic;
using Entity.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.ContainerRepo
{   
    //Container-specific methods are defined in this repository. The default ones will be implemented from the generic repository.
    public class ContainerRepository:GenericRepository<Container>,IContainerRepository
    {
        public ContainerRepository(VehicleDbContext context, ILogger logger) : base(context, logger)
        {
        }
        
        private string connectionString = "Server=LAPTOP-58K1GN78; Database=HW2DB;Trusted_Connection=True;";

        //Getting all containers for the vehicle whose id is entered as parameter using Dapper.
        public async Task<IEnumerable<Container>> GetByVehicleId(long id)
        {
            //sql command for getting containers.
            var sql = "SELECT * FROM Container WHERE VehicleId=@VehicleId";
            using (var connection=new SqlConnection(connectionString))
            {
                connection.Open();
                //Executing sql command using Dapper.
                var listOfContainers = await connection.QueryAsync<Container>(sql, new {VehicleId = id});
                return listOfContainers;
            }
        }

        //This method will divide containers into n groups.
        public async Task<List<GroupModelForContainers>> GroupByVehicleId(long id, int n)
        {
            //Firstly, getting the containers of the vehicle.
            var listOfContainers = await GetByVehicleId(id);
            var enumerator = listOfContainers.GetEnumerator();
            
            //Creating lists need to use.
            List<ContainerViewModel> containers = new List<ContainerViewModel>();
            List<ContainerViewModel> containersByGroups = new List<ContainerViewModel>();
            List<GroupModelForContainers> result = new List<GroupModelForContainers>();
            while (enumerator.MoveNext())
            {
                var model = ConvertToViewModel(enumerator.Current);
                containers.Add(model);
            }

            //Defining variables. Target is the number of containers in the groups.
            int count = 0, groupNumber = 1;
            double target = Math.Round(((double) containers.Count / (double) n));
            for (int i = 0; i < containers.Count; i++)
            {
                containersByGroups.Add(containers[i]);
                count++;
                if (count == (int)target || i == containers.Count - 1)
                {
                    result.Add(GroupModelCreator(groupNumber,containersByGroups));
                    groupNumber++;
                    count = 0;
                    containersByGroups.Clear();
                }
            }

            return result;
        }

        //Creating group model. This model contains a group number and containers included in that group.
        public GroupModelForContainers GroupModelCreator(int n, List<ContainerViewModel> containers)
        {
            List<ContainerViewModel> temp = new List<ContainerViewModel>(containers);
            GroupModelForContainers result = new GroupModelForContainers {Containers = temp, GroupNumber = n};
            return result;
        }

        //Converting Container to ContainerViewModel. For this process AutoMapper can be used too.
        public ContainerViewModel ConvertToViewModel(Container entity)
        {
            ContainerViewModel model = new ContainerViewModel
            {
                ContainerName = entity.ContainerName, Latitude = entity.Latitude, Longtitude = entity.Longtitude,
                VehicleId = entity.VehicleId
            };
            return model;
        }
    }
}
