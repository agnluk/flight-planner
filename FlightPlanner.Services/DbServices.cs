using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Services
{
    public class DbServices : IDbService
    {
        protected readonly IFlightPlannerDbContext _context;
        public DbServices(IFlightPlannerDbContext context)
        {
            _context = context;
        }
        public void Create<T>(T entity) where T : Entity
        {
           _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : Entity
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public List<T> GetAll<T>() where T : Entity
        {
           return _context.Set<T>().ToList();
        }

        public void Update<T>(T entity) where T : Entity
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public T GetById<T>(int Id) where T : Entity
        {
            return _context.Set<T>().SingleOrDefault(x=> x.Id == Id);
        }
    }
}
