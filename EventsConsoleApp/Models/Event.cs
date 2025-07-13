using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime NotifyDate { get; set; }

        public override string ToString()
        {
            return $"Встреча #{Id} {Name}\n" +
                    $"Дата начала {StartDate.ToString("dd.MM.yyyy HH:mm")}\n" +
                    $"Дата окончания {EndDate.ToString("dd.MM.yyyy HH:mm")}\n" +
                    $"Дата напоминания {NotifyDate.ToString("dd.MM.yyyy HH:mm")}\n" +
                    (!String.IsNullOrWhiteSpace(Description) ? $"Описание: {Description}" : "");
        }
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ValidationException("Название встречи обязательно");

            ValidateStartDate();
            ValidateEndDate();
            ValidateNotifyDate();

            return true;
        }

        public bool IsStartDateValid => StartDate > DateTime.Now;
        public bool IsEndDateValid => EndDate > StartDate;
        public bool IsNotifyDateValid => NotifyDate <= StartDate && NotifyDate > DateTime.Now;
        public string StartDataValidErrorString => "Дата начала должна быть в будущем";
        public string EndDataValidErrorString => "Дата окончания должна быть позже даты начала";
        public string NotifyDataValidErrorString => "Напоминание должно быть до начала встречи";

        public bool ValidateStartDate()
        {
            if (!IsStartDateValid)
                throw new ValidationException(StartDataValidErrorString);
            return true;
        }
        public bool ValidateEndDate()
        {
            if (!IsEndDateValid)
                throw new ValidationException(EndDataValidErrorString);
            return true;
        }
        public bool ValidateNotifyDate()
        {
            if (!IsNotifyDateValid)
                throw new ValidationException(NotifyDataValidErrorString);
            return true;
        }

    }
}
