using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Generic
{
    //Generic repository for CRUD actions. This generic repository is using Entity Framework Core for actions.
    public class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        private readonly ILogger _logger;
        protected VehicleDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(VehicleDbContext context,ILogger logger)
        {
            _context = context;
            _logger = logger;

            dbSet = context.Set<T>();
        }

        //Adding entity to the database. Method will return false if action failed.
        public async Task<bool> Add(T entity)
        {
            var temp = dbSet.AddAsync(entity);
            if (temp.IsCompletedSuccessfully)
            {
                return true;
            }

            return false;
        }

        //Updating an entity.
        public async Task<bool> Update(T entity)
        {
            var temp =  dbSet.Update(entity);
            return true;
        }

        //Deleting an entity. If there is no entity whose id is parameter, method will return false.
        public async Task<bool> Delete(long id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity is null)
            {
                return false;
            }
            var temp = dbSet.Remove(entity);
            return true;
        }

        //Default getbyid method.
        public async Task<T> GetById(long id)
        {
            var model = await dbSet.FindAsync(id);
            return model;
        }

        //Getting all entities from database.
        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }
    }
}
