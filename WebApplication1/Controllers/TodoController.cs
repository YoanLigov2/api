using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{

    [ApiController]
    public class TodoController : ControllerBase
    {
        private IConfiguration _configuration;
        public TodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_tasks")]
        public JsonResult get_tasks()
        {
            string query = "select * from todo";
            DataTable table= new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("mydb");
            SqlDataReader myReader;
            using(SqlConnection myCon=new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader=myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult(table);
        }

        [HttpPost("add_task")]
        public JsonResult add_task([FromForm] string task)
        {
            string query = "insert into todo values (@task)";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("mydb");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@task", task);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpDelete("delete_task/{id}")]
        public JsonResult delete_task(string id)
        {
            string query = "DELETE FROM todo WHERE id=@id";
            string SqlDatasource = _configuration.GetConnectionString("mydb");

            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

    }
}
