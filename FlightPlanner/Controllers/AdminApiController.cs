using FlightPlanner.Core.Models;
using FlightPlanner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]

    public class AdminApiController : BaseApiController
    {
        private readonly object _locker = new object();
        public AdminApiController(IFlightPlannerDbContext context) :base(context) { }

    }
}
