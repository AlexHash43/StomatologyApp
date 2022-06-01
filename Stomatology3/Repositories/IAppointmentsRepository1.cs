using Stomatology3.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stomatology3.Repositories
{
    public interface IAppointmentsRepository1
    {
        Task CreateAppointmentAsync(AppointmentModel appointment, CancellationToken cancellationToken);
        Task<int> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken);
        Task<AppointmentModel> GetAppointmentAsync(Guid id);
        Task<IEnumerable<AppointmentModel>> GetAppointmentsAsync();
        Task<int> UpdateAppointmentAsync(AppointmentModel appointment, CancellationToken cancellationToken);
    }
}