using Stomatology3.Controllers.Appointments.AppointmentmModels;
using Stomatology3.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Stomatology3.Repositories
{
    public interface IAppointmentsRepository
    {
        Task<AppointmentDto> CreateAppointmentAsync(ClaimsPrincipal principal, CreateAppointmentModel appointment, CancellationToken cancellationToken);
        Task<int> DeleteAppointmentAsync(string id, CancellationToken cancellationToken);
        Task<AppointmentDto> GetAppointmentAsync(string id);
        Task<IEnumerable<AppointmentReturn>> GetAppointmentsAsync();
        Task<int> UpdateAppointmentAsync(AppointmentModel appointment, CancellationToken cancellationToken);
    }
}