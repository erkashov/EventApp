using EventsConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Services
{
    public interface IExportService
    {
        bool ExportEvents(List<Event> events);
    }
}
