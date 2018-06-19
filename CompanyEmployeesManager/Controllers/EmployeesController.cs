using AutoMapper;
using CompanyEmployeesManager.Models;
using DataLayer.Abstract;
using DataLayer.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using DataLayer;
using DataLayer.Domain;
using CompanyEmployeesManager.Utils;

namespace CompanyEmployeesManager.Controllers
{
    public class EmployeesController : ApiController
    {
        private readonly IEmployeesRepository _repository;
        public EmployeesController(IEmployeesRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("api/employees/delete")]
        public IHttpActionResult Delete([FromBody]EmployeeViewModel model)
        {
            var result = _repository.Remove(Mapper.Map<Employee>(model));
            if(!result)
            {
                return Json(new { Success = false, Message = "Something went wrong!" });
            }
            return Json(new { Success = true, Message = "Employee successfully deleted!" });
        }

        [HttpPost]
        [Route("api/employees/edit")]
        public IHttpActionResult Edit([FromBody]EmployeeViewModel model)
        {
            if (model.StartDate > DateTime.Now || model.StartDate.Year != DateTime.Now.Year || model.StartDate.Month != DateTime.Now.Month)
            {
                ModelState.AddModelError("StartDate", "Date is not valid!");
            }
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Message = "Not valid data!", Data = ModelState.Keys.Select(k => k.Replace("model.", "")) });
            }
            var result = _repository.Edit(Mapper.Map<Employee>(model));
            if (!result)
            {
                return Json(new { Success = false, Message = "Something went wrong!" });
            }
            return Json(new { Success = true, Message = "Employee successfully edited!" });
        }

        [HttpPost]
        [Route("api/employees/create")]
        public IHttpActionResult Create([FromBody]EmployeeCreateViewModel model)
        {
            if (model.StartDate > DateTime.Now || model.StartDate.Year != DateTime.Now.Year || model.StartDate.Month != DateTime.Now.Month)
            {
                ModelState.AddModelError("StartDate", "Date is not valid!");
            }
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false, Message = "Not valid data!", Data = ModelState.Keys.Select(k => k.Replace("model.", ""))});
            }
            var result = _repository.Add(Mapper.Map<Employee>(model));
            if (!result)
            {
                return Json(new { Success = false, Message = "Something went wrong!" });
            }
            return Json(new { Success = true, Message = "Employee successfully created!" });
        }

        [HttpGet]
        [Route("api/employees/getPage/{itemsPerPage:int}/{page}")]
        public IHttpActionResult GetPage(int itemsPerPage, int page = 1)
        {
            try
            {
                var employees = _repository.GetAll();
                if (employees == null)
                {
                    return Json(new { Success = false, Message = "Something went wrong!" });
                }
                var pageEmployees = Mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
                var pageViewModel = new PageEmployeesViewModel
                {
                    CurrentPage = page,
                    Employees = pageEmployees.Skip((page - 1) * itemsPerPage).Take(itemsPerPage),
                    SearchString = ""
                };
                var count = employees.Count();
                var totalPages =  count / itemsPerPage;
                totalPages = totalPages == 0 ? 1 :count% (totalPages*itemsPerPage) == 0?totalPages:totalPages + 1;
                pageViewModel.TotalPages = totalPages;
                return Json(new { Success = true, Data = pageViewModel });
            }
            catch
            {
                return Json(new { Success = false, Message = "Something went wrong!" });
            }
        }

        [HttpGet]
        [Route("api/employees/search/{searchStr}/{itemsPerPage:int}/{page:int}")]
        public IHttpActionResult Search(string searchStr, int itemsPerPage, int page = 1)
        {
            try
            {
                var employees = _repository.Search(searchStr);
                if (employees == null)
                {
                    return Json(new { Success = false, Message = "Something went wrong!" });
                }
                var pageEmployees = Mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
                var pageViewModel = new PageEmployeesViewModel
                {
                    CurrentPage = page,
                    Employees = pageEmployees.Skip((page - 1) * itemsPerPage).Take(itemsPerPage),
                    SearchString = searchStr
                };
                var count = employees.Count();
                var totalPages = count / itemsPerPage;
                totalPages = totalPages == 0 ? 1 : count % (totalPages * itemsPerPage) == 0 ? totalPages : totalPages + 1;
                pageViewModel.TotalPages = totalPages;
                return Json(new { Success = true, Data = pageViewModel });
            }
            catch
            {
                return Json(new { Success = false, Message = "Something went wrong!" });
            }
        }
    }
}
