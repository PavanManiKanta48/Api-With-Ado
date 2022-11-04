
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.WebSockets;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using WebAPI.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public IConfiguration _configuration;
        SqlConnection con;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetConnectionString("Pavan"));

        }

        [HttpGet]
        public JsonResult GetAllEmployee()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Employee";
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Email = dr["email"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return new JsonResult(employees);

        }
       
        [HttpPost]
        public JsonResult  AddEmployee(Employee employee)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into Employee(Id,Name, Email) values("+employee.Id+",'" + employee.Name + "','" + employee.Email + "')";
                con.Open();
                cmd.ExecuteNonQuery();
                return new JsonResult(employee);

            }

            catch (Exception ex)
            {

                return new JsonResult(ex.Message); ;
            }
            
            finally
            {
                con.Close();
            }
        }
        [HttpPut]
        public JsonResult UpdateEmployee(int id, Employee employee)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Update Employee Set Name='" + employee.Name + "', Email= '" + employee.Email + "' Where(ID=" + id + ");";
                con.Open();
                cmd.ExecuteNonQuery();
                return new JsonResult(employee);
            }
            catch (Exception ex)
            {
                  return new JsonResult(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        [HttpDelete]
        public JsonResult DeleteEmployee(int id)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Delete from Employee Where(ID=" + id + ");";
                con.Open();
                cmd.ExecuteNonQuery();
                return new JsonResult(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}


