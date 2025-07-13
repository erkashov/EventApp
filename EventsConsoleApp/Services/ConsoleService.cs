using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Repositories
{
    public class ConsoleService : IOService
    {
        public void Write(object value)
        {
            Console.WriteLine(value);
        }
        public string Read()
        {
            return Console.ReadLine() ?? "";
        }

        public void WriteError(object value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public void WriteSuccess(object value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public void WriteNotify(object value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
