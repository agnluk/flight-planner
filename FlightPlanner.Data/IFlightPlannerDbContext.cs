﻿using FlightPlanner.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlightPlanner.Data
{
    public interface IFlightPlannerDbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }
        DbSet<T>Set<T>() where T : class;
        EntityEntry<T> Entry<T> (T entity) where T : class;
        public int SaveChanges();
    }
}
