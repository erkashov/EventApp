using EventsConsoleApp.Models;
using EventsConsoleApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Services
{
    public class TextExportService : IExportService
    {
        public bool ExportEvents(List<Event> events)
        {
            if (events.Count > 0)
            {
                var date = events.First().StartDate;
                string path = "Встречи за " + date.ToString("dd-MM-yyyy");
                using (StreamWriter writer = new StreamWriter(path + ".txt", false))
                {
                    writer.WriteLine(path);
                    foreach (var ev in events)
                    {
                        writer.WriteLine(ev.ToString());
                        writer.WriteLine();
                    }
                }
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = path + ".txt",
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                return true;
            }
            return false;
        }
    }
}
