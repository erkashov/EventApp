using EventsConsoleApp.Data;
using EventsConsoleApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Services
{
    public class NotifyTimer
    {
        private Timer _timer = null;
        private IEventsRepository _eventsRepository;
        public NotifyTimer(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
            _timer = new Timer(TimerCallback, null, 0, 36000);
        }

        private void TimerCallback(Object o)
        {
            _eventsRepository.Notify();
        }
    }
}
