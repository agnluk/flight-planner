using FlightPlanner.Core.Models;
using System.Collections.Generic;

namespace FlightPlanner.Services
{
    public interface IDbService
    {
        public T GetById<T>(int Id) where T :Entity;
        public void Create<T>(T entity) where T : Entity;
        public void Update<T>(T entity) where T : Entity;
        public void Delete<T>(T entity) where T : Entity;
        public List<T> GetAll<T>() where T : Entity;
    }
}
