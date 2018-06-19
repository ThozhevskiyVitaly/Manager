using DataLayer.Abstract;
using DataLayer.Domain;
using DataLayer.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer
{
    public class EmployeesRepository : IDisposable, IEmployeesRepository
    {
        private readonly Logger _logger;
        private bool disposed;
        internal readonly EmployeesContext _dbContext;

        public EmployeesRepository(DbContext context, IServiceLogger logger)
        {
            _dbContext = (EmployeesContext)context;
            _logger = logger.Logger;
        }
        public bool Add(Employee obj)
        {
            try
            {
                obj.Id = Guid.NewGuid().ToString();
                _dbContext.Employees.Add(obj);
                _dbContext.SaveChanges();
                _logger.Info($"{obj} was successfully added.");
                return true;
            }
            catch(Exception ex)
            {
                _logger.Fatal<Exception>($"Error adding: {obj}", ex);
                return false;
            }
        }

        public bool Edit(Employee obj)
        {
            try
            {
                var employee = _dbContext.Employees.SingleOrDefault(e => e.Id == obj.Id);
                if(employee == null)
                {
                    return false;
                }

                _dbContext.Employees.AddOrUpdate(obj);
                _dbContext.SaveChanges();
                _logger.Info($"Data was successfully edited from {employee}, to {obj}");
                return true;
            }
            catch(Exception ex)
            {
                _logger.Fatal<Exception>($"Error updating: {obj}", ex);
                return false;
            }
        }

        public bool Remove(Employee obj)
        {
            try
            {
                var employee = _dbContext.Employees.SingleOrDefault(e => e.Id == obj.Id);
                if(employee == null)
                {
                    return false;
                }
                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();
                _logger.Info($"{obj} was successfully removed.");
                return true;
            }
            catch(Exception ex)
            {
                _logger.Fatal<Exception>($"Error removing employee, {obj}", ex);
                return false;
            }
        }
        
        public Employee Get(string id)
        {
            try
            {
                var employee = _dbContext.Employees.Single(e => e.Id == id);
                _logger.Info($"Successfully get {employee}");
                return employee;
            }
            catch(Exception ex)
            {
                _logger.Fatal<Exception>($"Error getting employee! Id = {id}.", ex);
                return null;
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            try
            {
                var employees = _dbContext.Employees.OrderByDescending(e => e.StartDate).ToArray();
                _logger.Info("Successfully get employees.");
                return employees;
            }
            catch (Exception ex)
            {
                _logger.Fatal<Exception>("Error getting employees!", ex);
                return null;
            }
        }

        public IEnumerable<Employee> Search(string searchString)
        {
            try
            {
                var employees = _dbContext.Employees.Where(e => e.Name.Contains(searchString)||e.Position.Contains(searchString)).OrderByDescending(e => e.StartDate);
                _logger.Info($"Successfully get employees. Search string '{searchString}'.");
                return employees;
            }
            catch (Exception ex)
            {
                _logger.Fatal<Exception>($"Error getting employees! Search string '{searchString}'.", ex);
                return null;
            }
        }


        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
