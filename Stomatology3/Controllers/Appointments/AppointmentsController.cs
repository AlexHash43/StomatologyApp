using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stomatology3.Controllers.Appointments.AppointmentmModels;
using Stomatology3.Models;
using Stomatology3.Repositories;
using Stomatology3.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Stomatology3.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsRepository _repository;
        private readonly ILogger<AppointmentsController> _logger;
        //private readonly ClaimsPrincipal _principal;

        public AppointmentsController (IAppointmentsRepository repository, ILogger<AppointmentsController> logger, ClaimsPrincipal principal)
        {
            _repository = repository;
            _logger = logger;
            //_principal = principal;
        }

        // Get Appointments
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var items = await _repository.GetAppointmentsAsync();


            if(!items.Any()) return NotFound();
            return Ok(items);
        }

        [HttpGet("id")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(string id)
        {
            var result = (await _repository.GetAppointmentAsync(id));
            if (result == null) return NotFound();
                    return Ok(result);

        }
        [HttpPost]
        public async Task<ActionResult<AppointmentModel>> CreateAppointment( CreateAppointmentModel appointment, CancellationToken cancellationToken) 
        { 
            var appt = await _repository.CreateAppointmentAsync(appointment, cancellationToken);
            if(appt == null) return NotFound();
            return Ok(appt);
        }
        [HttpPost("Edit")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment( UpdateAppointment appointment, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateAppointmentAsync(appointment, cancellationToken);
            if(result == null) return NotFound();
            return Ok(result);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteAppointmentAsync(id, cancellationToken);
            if (result != null) return NotFound();
            return Ok(AppResources.TaskDeleted);
        }







    }
}
