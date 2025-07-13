using NUnit.Framework.Internal.Execution;
using System.ComponentModel.DataAnnotations;
using Event = EventsConsoleApp.Models.Event;

namespace EventsConsoleTest
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void Validate_EmptyName_ThrowsValidationException()
        {
            var ev = new Event
            {
                Name = "",
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(2),
                NotifyDate = DateTime.Now.AddMinutes(30)
            };
            Assert.Throws<ValidationException>(() => ev.Validate(), "Название встречи обязательно");
        }

        [Test]
        public void Validate_PastStartDate_ThrowsValidationException()
        {
            var ev = new Event
            {
                Name = "Test",
                StartDate = DateTime.Now.AddHours(-1),
                EndDate = DateTime.Now.AddHours(1),
                NotifyDate = DateTime.Now.AddMinutes(-10)
            };
            Assert.Throws<ValidationException>(() => ev.Validate(), "Дата начала должна быть в будущем");
        }

        [Test]
        public void Validate_PastEndDate_ThrowsValidationException()
        {
            var ev = new Event
            {
                Name = "Test",
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(-10),
                NotifyDate = DateTime.Now.AddMinutes(10)
            };
            Assert.Throws<ValidationException>(() => ev.Validate(), "Дата окончания должна быть позже даты начала");
        }

        [Test]
        public void ToString_Event_ReturnsCorrectFormat()
        {
            var ev = new Event
            {
                Id = 1,
                Name = "Meeting",
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(2),
                NotifyDate = DateTime.Now.AddMinutes(10),
                Description = "Test"
            };
            var result = ev.ToString();
            Assert.That(result, Contains.Substring("Встреча #1 Meeting"));
            Assert.That(result, Contains.Substring("Описание: Test"));
        }
    }
}