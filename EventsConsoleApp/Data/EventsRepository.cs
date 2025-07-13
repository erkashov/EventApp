using EventsConsoleApp.Models;
using EventsConsoleApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Data
{
    public class EventsRepository : IEventsRepository
    {
        private List<Event> _events = new List<Event>();
        private readonly IOService _ioService;
        public EventsRepository(IOService oService)
        {
            _ioService = oService;
            _events.Add(new Event()
            {
                Id = 1,
                Name = "Техническое собеседование",
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 00, 0),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 00, 0),
                NotifyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 31, 0),
            });
            _events.Add(new Event()
            {
                Id = 2,
                Name = "Техническое собеседование 2",
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 0),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 00, 0),
                NotifyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 30, 0),
                Description = "Дополнительное"
            });
        }

        public List<Event> GetEvents(DateTime? date)
        {
            return _events.Where(p => date == null || date != null && p.StartDate.Date == date.Value.Date).OrderBy(p => p.StartDate).ToList();
        }

        public Event GetEvent(int id)
        {
            var ev = _events.FirstOrDefault(p => p.Id == id);
            if (ev == null)
            {
                throw new Exception($"Встреча {id} не найдена");
            }
            return ev;
        }

        public void AddEvent(Event ev)
        {
            Validate(ev);
            ev.Id = _events.Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;
            _events.Add(ev);
            _ioService.WriteSuccess($"Встреча добавлена #{ev.Id}");
        }

        public void DeleteEvent(int id)
        {
            var ev = _events.FirstOrDefault(p => p.Id == id);
            if (ev == null)
            {
                throw new Exception("Укажите объект Event");
            }
            _events.Remove(ev);
            _ioService.WriteSuccess($"Удалена встреча #{ev.Id}");
        }

        public void UpdateEvent(Event ev)
        {
            Validate(ev);

            var savedEvent = GetEvent(ev.Id);
            if (savedEvent != null)
            {
                savedEvent.Name = ev.Name;
                savedEvent.StartDate = ev.StartDate;
                savedEvent.EndDate = ev.EndDate;
                savedEvent.NotifyDate = ev.NotifyDate;
                savedEvent.Description = ev.Description;

                _ioService.WriteSuccess($"Встреча {ev.Id} изменена");
            }
        }

        public void Notify()
        {
            var eventsForNotify = _events.Where(p => p.NotifyDate.Date == DateTime.Now.Date && p.NotifyDate.Hour == DateTime.Now.Hour && p.NotifyDate.Minute == DateTime.Now.Minute);
            if (eventsForNotify.Count() > 0)
            {
                _ioService.WriteNotify("Напоминание о встречах");
                foreach (var ev in eventsForNotify)
                {
                    _ioService.WriteNotify(ev.ToString());
                }
            }
        }

        private void Validate(Event ev)
        {
            if (ev == null)
            {
                throw new Exception("Укажите объект Event");
            }
            if (HasTimeConflicts(ev))
            {
                throw new Exception("Встречи не могут пересекаться");
            }
            ev.Validate();
        }

        private bool HasTimeConflicts(Event ev)
        {
            return _events.Any(existing =>
                ev.StartDate < existing.EndDate &&
                ev.EndDate > existing.StartDate &&
                ev.Id != existing.Id);
        }
    }
}
