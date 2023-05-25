using FlightPlanner.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlightPlanner.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected IFlightPlannerDbContext _context;
        public BaseApiController( IFlightPlannerDbContext context)
        {
            _context= context;
        }
    }
}
