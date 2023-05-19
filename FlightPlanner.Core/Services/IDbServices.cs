using FlightPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Services
{
    public interface IDbServices
    {
        public void Create<T>(T entity) where T : Entity;
        public void Update<T>(T entity);
        public void Delete<T>(T entity);
        public List<T> GetAll<T>() where T : Entity;
    }
}
