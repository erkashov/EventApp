using EventsConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event = EventsConsoleApp.Models.Event;

namespace EventsConsoleTest
{
    [TestFixture]
    public class TextExportServiceTests
    {
        [Test]
        public void ExportEvents_ValidEvents_CreatesFile()
        {
            var service = new TextExportService();
            var events = new List<Event> {
                new Event {
                    Id = 1,
                    Name = "Test",
                    StartDate = DateTime.Now.AddHours(1),
                    EndDate = DateTime.Now.AddHours(2),
                    NotifyDate = DateTime.Now.AddMinutes(10)
                }
            };
            var result = service.ExportEvents(events);
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists($"Встречи за {DateTime.Now.ToString("dd-MM-yyyy")}.txt"));
        }

        [Test]
        public void ExportEvents_EmptyList_ReturnsFalse()
        {
            var service = new TextExportService();
            var result = service.ExportEvents(new List<Event>());
            Assert.IsFalse(result);
        }
    }
}
