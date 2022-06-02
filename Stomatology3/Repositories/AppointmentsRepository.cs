using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Context;
using Stomatology3.Controllers.Appointments.AppointmentmModels;
using Stomatology3.Models;
using Stomatology3.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Stomatology3.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Stomatology3.Repositories
{
    public class AppointmentsRepository : IAppointmentsRepository
    //: IAppointmentsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public AppointmentsRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<AppointmentDto> CreateAppointmentAsync(ClaimsPrincipal principal, CreateAppointmentModel appointment, CancellationToken cancellationToken)
        {
            //var user = _userManager.GetUserId(User);
            var newAppointment = new AppointmentModel
            {
                Id = Guid.NewGuid().ToString(),
                AppointmentStart = appointment.AppointmentStart,
                ProcedureId = appointment.ProcedureId,
                DoctorId = appointment.DoctorId,
                PatientId = principal.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedOn = DateTime.Now,
                Status = AppointmentStatus.InProgress
            };
            await _context.AddAsync(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return await GetAppointmentAsync(newAppointment.Id.ToString());
        }

        public async Task<int> DeleteAppointmentAsync(string id, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id.ToString() == id, cancellationToken);
            _context.Appointments.Remove(appointment);
            return await _context.SaveChangesAsync(cancellationToken);
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
                DoctorFullName = doctor.FullName,
                PatientFullName = patient.FullName,
                CreatedOn = appointment.CreatedOn,
                Status = appointment.Status
            };
            return appt;
        }

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

        public async Task<int> UpdateAppointmentAsync(AppointmentModel appointment, CancellationToken cancellationToken)
        {
            var appt = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == appointment.Id, cancellationToken);
            appt.AppointmentStart = appointment.AppointmentStart;
            appt.ProcedureId = appointment.ProcedureId;
            //appt.PrType = appointment.PrType;
            appt.DoctorId = appointment.DoctorId;
            appt.PatientId = appointment.PatientId;
            appt.CreatedOn = appointment.CreatedOn;
            appt.Status = appointment.Status;
            _context.Appointments.Update(appt);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
