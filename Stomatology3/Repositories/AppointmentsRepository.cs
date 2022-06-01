using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Context;
using Stomatology3.Models;
using Stomatology3.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stomatology3.Repositories
{
    public class AppointmentsRepository : IAppointmentsRepository1
    //: IAppointmentsRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<User> _userManager;

        public AppointmentsRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            //_userManager = userManager;
        }
        public async Task<AppointmentModel> CreateAppointmentAsync(AppointmentModel appointment, CancellationToken cancellationToken)
        {
            await _context.AddAsync(appointment);
            await _context.SaveChangesAsync();
            
        }

        public async Task<int> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            _context.Appointments.Remove(appointment);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<AppointmentModel> GetAppointmentAsync(Guid id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<int> UpdateAppointmentAsync(AppointmentModel appointment, CancellationToken cancellationToken)
        {
            var appt = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == appointment.Id, cancellationToken);
            appt.AppointmentStart = appointment.AppointmentStart;
            appt.ProcedureId = appointment.ProcedureId;
            appt.PrType = appointment.PrType;
            appt.DoctorId = appointment.DoctorId;
            appt.PatientId = appointment.PatientId;
            appt.CreatedOn = appointment.CreatedOn;
            appt.Status = appointment.Status;
            _context.Appointments.Update(appt);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
