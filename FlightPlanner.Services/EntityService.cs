using System;
using System.Collections.Generic;
using FlightPlanner.Core.Services;
using FlightPlanner.Core.Models;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class EntityService<T> : DbServices, IEntityService<T> where T : Entity
    {
        public EntityService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public void Create(T entity)
        {
            Create<T>(entity);
        }

        public void Delete(T entity)
        {
            Delete<T>(entity);
        }

        public List<T> GetAll()
        {
            return GetAll<T>();
        }

        public void Update(T entity)
        {
            Update<T>(entity);
        }
        public T GetById(int id) 
        {
            return GetById<T>(id);
        }
    }
}
