using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Context;
using Stomatology3.Controllers.Appointments.AppointmentmModels;
using Stomatology3.Models;
//using Stomatology3.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Stomatology3.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Stomatology3.Repositories
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        //private readonly ClaimsPrincipal _principal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentsRepository(ApplicationDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) //ClaimsPrincipal principal)
        {
            _context = context;
            _userManager = userManager;
            //_principal = principal;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentModel appointment)//, CancellationToken cancellationToken)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            //var user = _userManager.GetUserId(User);
       
            var newAppointment = new AppointmentModel
            {
                Id = Guid.NewGuid().ToString(),
                AppointmentStart = appointment.AppointmentStart,
                ProcedureId = appointment.ProcedureId,
                DoctorId = appointment.DoctorId,
                PatientId = user.Id,
                CreatedOn = DateTime.Now,
                Status = AppointmentStatus.InProgress
            };
            var add = await _context.Appointments.AddAsync(newAppointment);

            var save = await _context.SaveChangesAsync();//cancellationToken);

            return await GetAppointmentAsync(newAppointment.Id.ToString());
        }

        public async Task<AppointmentDto> GetAppointmentAsync(string id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            var procedure = await _context.ProcedureTypes.FindAsync(appointment.ProcedureId);
            var doctor = await _context.Users.FindAsync(appointment.DoctorId);
            var patient = await _context.Users.FindAsync(appointment.PatientId);
            var appt = new AppointmentDto
            {
                AppointmentStart = appointment.AppointmentStart,
                ProcedureName = procedure.ProcedureName,
                DoctorFullName = doctor.FirstName + doctor.LastName,
                PatientFullName = patient.FullName,
                CreatedOn = appointment.CreatedOn,
                Status = appointment.Status
            };
            return appt;
        }
        //public async Task<IEnumerable<AppointmentReturn>> GetAppointmentsUserAsync()
        //{

        //}

        public async Task<IEnumerable<AppointmentReturn>> GetAppointmentsAsync()
        {
            var proceduresList = await _context.ProcedureTypes.ToListAsync();
            var appointmentsList = await _context.Appointments.ToListAsync();
            var patients = await _userManager.GetUsersInRoleAsync("Patient");
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            return appointmentsList.AsEnumerable()
                .Select(appt => new AppointmentReturn
                {
                    Id = appt.Id,
                    AppointmentStart = appt.AppointmentStart,
                    ProcedureName = proceduresList.FirstOrDefault(procedure => procedure.ProcId == appt.ProcedureId).ProcedureName,
                    DoctorFullName = doctors.FirstOrDefault(doctor => doctor.Id == appt.DoctorId).FullName,
                    PatientFullName = patients.FirstOrDefault(patient => patient.Id == appt.PatientId).FullName,
                    CreatedOn = appt.CreatedOn,
                    Status = appt.Status

                });
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(UpdateAppointment appointment)//, CancellationToken cancellationToken)
        {
            var appt = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == appointment.Id);//, cancellationToken);
            appt.AppointmentStart = appointment.AppointmentStart;
            appt.ProcedureId = appointment.ProcedureId;
            //appt.PrType = appointment.PrType;
            appt.DoctorId = appointment.DoctorId;
            appt.PatientId = appointment.PatientId;
            appt.CreatedOn = appointment.CreatedOn;
            appt.Status = appointment.Status;
            _context.Appointments.Update(appt);
            await _context.SaveChangesAsync(); //cancellationToken);
            return await GetAppointmentAsync(appointment.Id);
        }

        public async Task<AppointmentDto> DeleteAppointmentAsync(string id)//, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id.ToString() == id);//, cancellationToken);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();// cancellationToken);
            return await GetAppointmentAsync(id);
        }
    }
}
