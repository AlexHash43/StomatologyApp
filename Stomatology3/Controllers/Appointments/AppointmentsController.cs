using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stomatology3.Models;
using Stomatology3.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stomatology3.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsRepository1 _repository;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController (IAppointmentsRepository1 repository, ILogger<AppointmentsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Get Appointments
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var items = (await _repository.GetAppointmentsAsync()).ToList();


            if(items == null) return NotFound();
            return Ok(items);
        }

        [HttpGet("id")]
        public async Task<ActionResult<AppointmentModel>> GetAppointment(Guid id)
        {
            var result = (await _repository.GetAppointmentAsync(id));
            if (result == null) return NotFound();
                    return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentModel appointment, CancellationToken cancellationToken) 
        { 
            var result = (await _repository.CreateAppointmentAsync(appointment, cancellationToken));
            if(result == null) return NotFound();
            return Ok(result.Appointment);
        }




    }
}
