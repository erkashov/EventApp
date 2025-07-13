using EventsConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Data
{
    public interface IEventsRepository
    {
        List<Event> GetEvents(DateTime? date);
        Event GetEvent(int id);
        void AddEvent(Event ev);
        void DeleteEvent(int id);
        void UpdateEvent(Event ev);
        void Notify();
    }
}
