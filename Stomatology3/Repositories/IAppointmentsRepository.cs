using Stomatology3.Controllers.Appointments.AppointmentmModels;
using Stomatology3.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stomatology3.Repositories
{
    public interface IAppointmentsRepository
    {
        Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentModel appointment, CancellationToken cancellationToken);
        Task<AppointmentDto> DeleteAppointmentAsync(string id, CancellationToken cancellationToken);
        Task<AppointmentDto> GetAppointmentAsync(string id);
        Task<IEnumerable<AppointmentReturn>> GetAppointmentsAsync();
        Task<AppointmentDto> UpdateAppointmentAsync(UpdateAppointment appointment, CancellationToken cancellationToken);
    }
}