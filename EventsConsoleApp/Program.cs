// See https://aka.ms/new-console-template for more information
using EventsConsoleApp.Data;
using EventsConsoleApp.Models;
using EventsConsoleApp.Repositories;
using EventsConsoleApp.Services;
using System.ComponentModel.DataAnnotations;

IOService ioService = new ConsoleService();
IEventsRepository eventsRepository = new EventsRepository(ioService);
IExportService exportService = new TextExportService();
EventService consoleManager = new EventService(eventsRepository, ioService, exportService);
NotifyTimer notifyTimer = new NotifyTimer(eventsRepository);

consoleManager.OutputEvents(DateTime.Now);
var input = "";
while (input != "exit")
{
    try
    {
        switch (input)
        {
            case "add":
                consoleManager.ReadEvent();
                consoleManager.OutputEvents(DateTime.Now);
                break;
            case "delete":
                consoleManager.DeleteEvent();
                consoleManager.OutputEvents(DateTime.Now);
                break;
            case "date":
                consoleManager.FilterDate();
                break;
            case "correct":
                consoleManager.CorrectEvent();
                break;
            case "export":
                consoleManager.ExportEvents();
                break;
        }
    }
    catch(Exception ex)
    {
        ioService.WriteError(ex.Message);
    }
    ioService.WriteNotify("\nВыберите действие\n" +
        "add - добавить новую встречу\n" +
        "delete - удалить встречу\n" +
        "correct - редактировать встречу\n" +
        "date - перейти на дату\n" +
        "export - экспорт в файл'\n" +
        "exit - выход");
    input = ioService.Read();
    continue;
}
