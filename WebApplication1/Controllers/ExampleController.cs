using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly AppDbContext _context;
    private IConfiguration _configuration;

    public ExampleController(IHttpClientFactory clientFactory, AppDbContext context, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetData()
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();

            // Deserialize the API data into a list of Post objects
            var posts = JsonSerializer.Deserialize<List<Post>>(data);

            // Remove Id before adding to context, if needed
            foreach (var post in posts)
            {
                post.Id = 0; // or simply do not set it, EF Core will ignore it if it’s 0.
            }

            // Save the data to the database
            _context.Posts.AddRange(posts);
            await _context.SaveChangesAsync();

            var res = new
            {
                message = "Data saved to the database successfully!",
            };

            return Ok(res);
        }

        return BadRequest("Error fetching data from API");
    }

    [HttpGet("getPosts")]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _context.Posts.ToListAsync();
        return Ok(posts); // Return the data as JSON
    }

    [HttpDelete("deletePosts")]
    public JsonResult delete_task()
    {
        string query = "TRUNCATE TABLE Posts";
        string SqlDatasource = _configuration.GetConnectionString("apidb");

        using (SqlConnection myCon = new SqlConnection(SqlDatasource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                myCommand.ExecuteNonQuery();
            }
        }
        return new JsonResult("Deleted Successfully");
    }
}
