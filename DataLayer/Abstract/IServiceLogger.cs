using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace DataLayer.Abstract
{
    public interface IServiceLogger
    {
        string LoggerName { get; set; }
        Logger Logger { get; set; }
    }
}
