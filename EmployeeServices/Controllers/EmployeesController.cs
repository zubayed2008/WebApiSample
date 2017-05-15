using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeServices.Controllers
{
    public class EmployeesController : ApiController
    {
        public HttpResponseMessage Get(string gender="all")
        {
            
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    switch (gender.ToLower())
                    {
                        case "all":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                        case "male":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(m => m.Gender == true).ToList());
                        case "female":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(m => m.Gender == false).ToList());
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Values must be male or female");
                    }
                }
            
            
        }

        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {

               var entity = entities.Employees.FirstOrDefault(e=> e.EmployeeID ==id);
                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Accepted, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.EmployeeID.ToString());
                    return message;
                }
            }catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(m => m.EmployeeID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id " + id + " not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(int id,[FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.EmployeeID == id);
                    if (entity != null)
                    {
                        entity.EmployeeID = employee.EmployeeID;
                        entity.EmployeeName = employee.EmployeeName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id " + id.ToString() + " not found");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        
    }
}
