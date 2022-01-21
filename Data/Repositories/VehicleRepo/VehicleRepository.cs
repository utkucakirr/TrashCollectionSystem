using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Data.Context;
using Data.DataModel;
using Data.Generic;
using Entity.DTOs;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.VehicleRepo
{
    //This class contains CRUD methods for Vehicle model using Dapper.
    public class VehicleRepository:GenericRepository<Vehicle>,IVehicleRepository
    {
        public VehicleRepository(VehicleDbContext context, ILogger logger) : base(context, logger)
        {
        }
        //My connection string for Dapper sql actions.
        private string connectionString = "Server=LAPTOP-58K1GN78; Database=HW2DB;Trusted_Connection=True;";

        //Adding a vehicle to database.
        public new async Task<bool> Add(Vehicle entity)
        {
            //Sql query for inserting.
            var sql = "Insert into Vehicle(VehicleName,VehiclePlate) VALUES(@VehicleName,@VehiclePlate)";
            using (var connection=new SqlConnection(connectionString))
            {
                connection.Open();
                //Query executing using Dapper.
                var result = await connection.ExecuteAsync(sql, entity);
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
        }

        //Updating a vehicle on database.
        public new async Task<bool> Update(Vehicle entity)
        {
            //Sql query for updating.
            var sql = "UPDATE Vehicle SET VehicleName=@VehicleName, VehiclePlate=@VehiclePlate WHERE Id=@Id";
            using (var connection=new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
        }

        //Deleting a vehicle from database.
        public new async Task<bool> Delete(long id)
        {
            //Sql query for removing vehicle from database.
            var sql = "DELETE FROM Vehicle WHERE Id=@Id";

            //Sql query for removing the vehicle's containers.
            var sql2 = "DELETE FROM Container WHERE VehicleId=@Id";
            using (var connection=new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new {Id = id});
                await connection.ExecuteAsync(sql2, new {Id = id});
                if (result==1)
                {
                    return true;
                }

                return false;
            }
        }

        //GetById method using the Generic Entity Framework Repository.
        public Task<Vehicle> GetById(long id)
        {
            return base.GetById(id);
        }

        //GetAll method using the Generic Entity Framework Repository.
        public Task<IEnumerable<Vehicle>> GetAll()
        {
            return base.GetAll();
        }
    }
}
