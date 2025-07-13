using EventsConsoleApp.Data;
using EventsConsoleApp.Models;
using EventsConsoleApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EventsConsoleApp.Services
{
    public class EventService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IOService _ioService;
        private readonly IExportService _exportServide;
        public EventService(IEventsRepository eventsRepository, IOService ioService, IExportService exportService)
        {
            _eventsRepository = eventsRepository;
            _ioService = ioService;
            _exportServide = exportService;
        }

        /// <summary>
        ///  Фильтрация встреч по дате
        /// </summary>
        /// <returns></returns>
        public List<Event> FilterDate()
        {
            DateTime? filterDate = GetConsoleDate("дату фильтра");
            if (filterDate != null)
            {
                var events = _eventsRepository.GetEvents(filterDate);
                if (events != null && events.Count > 0)
                {
                    return events;
                }
                else
                {
                    _ioService.WriteNotify("Встреч на выбранную дату нет");
                }
            }
            return new List<Event>();
        }

        /// <summary>
        /// Читает из консоли встречу
        /// </summary>
        /// <param name="ev">Встреча, если null - реализация добавления</param>
        public void ReadEvent(Event ev = null)
        {
            if (ev == null)
            {
                ev = new Event();
                _ioService.Write("Добавление встречи");
            }
            else
            {
                _ioService.Write("Редактирование встречи\n(Если поле не надо изменять - Enter)");
            }

            while (string.IsNullOrWhiteSpace(ev.Name) || ev.Id != 0)
            {
                _ioService.Write("Введите название:");
                var input = _ioService.Read()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    ev.Name = input;
                    break;
                }
                else if (ev.Id != 0)
                {
                    break;
                }
            }

            while (!ev.IsStartDateValid || ev.Id != 0)
            {
                var date = GetConsoleDate("дату начала");
                if (date != null)
                {
                    ev.StartDate = date.Value;
                    try
                    {
                        ev.ValidateStartDate();
                        break;
                    }
                    catch (ValidationException validateEx)
                    {
                        _ioService.WriteError(validateEx.Message);
                    }
                }
                else if (ev.Id != 0)
                {
                    break;
                }
            }
            while (!ev.IsEndDateValid || ev.Id != 0)
            {
                var date = GetConsoleDate("дату окончания");
                if (date != null)
                {
                    ev.EndDate = date.Value;
                    try
                    {
                        ev.ValidateEndDate();
                        break;
                    }
                    catch (ValidationException validateEx)
                    {
                        _ioService.WriteError(validateEx.Message);
                    }
                }
                else if (ev.Id != 0)
                {
                    break;
                }
            }
            while (!ev.IsNotifyDateValid || ev.Id != 0)
            {
                var date = GetConsoleDate("дату напоминания");
                if (date != null)
                {
                    ev.NotifyDate = date.Value;
                    try
                    {
                        ev.ValidateNotifyDate();
                        break;
                    }
                    catch (ValidationException validateEx)
                    {
                        _ioService.WriteError(validateEx.Message);
                    }
                }
                else if (ev.Id != 0)
                {
                    break;
                }
            }

            _ioService.Write("Введите описание или пустую строку:");
            var descr = _ioService.Read();
            if (!string.IsNullOrWhiteSpace(descr)) ev.Description = descr.Trim();

            if (ev.Id == 0)
            {
                _eventsRepository.AddEvent(ev);
            }
            else
            {
                _eventsRepository.UpdateEvent(ev);
                _ioService.Write(ev);
            }
        }

        /// <summary>
        /// Изменение встречи
        /// </summary>
        public void CorrectEvent()
        {
            while (true)
            {
                _ioService.WriteError("Введите Id встречи");
                var idString = _ioService.Read();
                if (idString == "exit") return;
                if (int.TryParse(idString, out var id))
                {
                    var ev = _eventsRepository.GetEvent(id);
                    if (ev != null)
                    {
                        if (ev.StartDate < DateTime.Now)
                        {
                            throw new Exception("Редактирование прошедшего события недоступно");
                        }
                        else
                        {
                            ReadEvent(ev);
                        }
                    }
                    else
                    {
                        continue;
                    }
                    return;
                }
                else
                {
                    _ioService.WriteError("Некорректный id, для выхода введите exit");
                }
            }
        }

        /// <summary>
        /// Вывод встреч в файл
        /// </summary>
        public void ExportEvents()
        {
            var events = FilterDate();
            if (_exportServide.ExportEvents(events))
            {
                _ioService.WriteSuccess("Встречи экспортированы в файл");
            }
        }

        /// <summary>
        /// Удаление встречи
        /// </summary>
        public void DeleteEvent()
        {
            while (true)
            {
                _ioService.WriteError("Введите Id встречи");
                var idString = _ioService.Read();
                if (idString == "exit") return;
                if (int.TryParse(idString, out var id))
                {
                    _eventsRepository.DeleteEvent(id);
                    return;
                }
                else
                {
                    _ioService.WriteError("Некорректный id, для выхода введите exit");
                }
            }
        }

        /// <summary>
        /// Вывод встреч
        /// </summary>
        /// <param name="date"></param>
        public void OutputEvents(DateTime date)
        {
            _ioService.Write("Ваши встречи на " + (date.Date == DateTime.Now.Date ? "сегодня" : date.ToString()));
            var events = _eventsRepository.GetEvents(date);
            foreach (var ev in events)
            {
                _ioService.Write(ev.ToString());
            }
        }

        /// <summary>
        /// Чтение даты
        /// </summary>
        /// <param name="outPutDate">наименование даты, которую надо ввести</param>
        /// <returns>Дата</returns>
        public DateTime? GetConsoleDate(string outPutDate)
        {
            while (true)
            {
                _ioService.Write($"Введите {outPutDate}:");
                var dateString = _ioService.Read();
                if (dateString == "exit" || string.IsNullOrWhiteSpace(dateString)) return null;
                if (DateTime.TryParse(dateString, out DateTime inputDate))
                {
                    return inputDate;
                }
                else
                {
                    _ioService.WriteError("Некорректная дата, для выхода введите exit");
                }
            }
        }

        /// <summary>
        /// Считывает даты
        /// </summary>
        /// <param name="prompt">Строка для вывода</param>
        /// <param name="validator">Условие валидации</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        /// <returns>Дата</returns>
        /// <exception cref="OperationCanceledException"></exception>
        private DateTime ReadValidDate(string prompt, Func<DateTime, bool> validator, string errorMessage)
        {
            DateTime date;
            while (true)
            {
                date = GetConsoleDate(prompt) ?? throw new OperationCanceledException();
                if (validator(date)) return date;
                _ioService.WriteError(errorMessage);
            }
        }
    }
}
