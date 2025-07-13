using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsConsoleApp.Repositories
{
    public interface IOService
    {
        void Write(object value);
        void WriteError(object value);
        void WriteSuccess(object value);
        void WriteNotify(object value);
        string Read();
    }
}
