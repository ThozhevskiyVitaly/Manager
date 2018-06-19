using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using DataLayer.Abstract;
using NLog;

namespace CompanyEmployeesManager.Utils
{
    public class EmployeesManagerServiceLogger: IServiceLogger
    {
        public Logger Logger { get; set; }
        public string LoggerName { get; set; }
        public EmployeesManagerServiceLogger(string name)
        {
            LoggerName = name;
            Logger = LogManager.GetLogger(name);
        }
    }
}
