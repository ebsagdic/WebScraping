using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Core.Services
{
    public interface ITrackingService
    {
        Task UpdateAllStatusesAsync();
        Task<string> GetTrackingStatusAsync(string trackingNumber);
    }
}
