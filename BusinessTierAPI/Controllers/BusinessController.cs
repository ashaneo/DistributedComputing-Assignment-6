using Microsoft.AspNetCore.Mvc;
using BusinessTierAPI.Models;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BusinessTierAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly string dataApiBaseUrl = "http://localhost:5000/api/data"; 
        private readonly string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "example-image.jpeg");
        // GET: api/business/count
        [HttpGet("count")]
        public async Task<ActionResult> GetDataCountAsync()
        {
            var client = new RestClient(dataApiBaseUrl);
            var request = new RestRequest
            {
                Method = Method.Get
            };

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var count = JsonConvert.DeserializeObject<int>(response.Content);
                return Ok(count);
            }
            return StatusCode((int)(response?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError), response?.Content);
        }

        // GET: api/business/values
        [HttpGet("values")]
        public async Task<ActionResult> GetAllDataAsync()
        {
            var client = new RestClient($"{dataApiBaseUrl}/values");
            var request = new RestRequest
            {
                Method = Method.Get
            };

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var data = JsonConvert.DeserializeObject<DataIntermed[]>(response.Content);
                return Ok(data);
            }
            return StatusCode((int)(response?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError), response?.Content);
        }

        // GET: api/business/values/{id}
        [HttpGet("values/{id}")]
        public async Task<ActionResult> GetDataByIdAsync(int id)
        {
            var client = new RestClient($"{dataApiBaseUrl}/values/{id}");
            var request = new RestRequest
            {
                Method = Method.Get
            };

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var data = JsonConvert.DeserializeObject<DataIntermed>(response.Content);
                return Ok(data);
            }
            return StatusCode((int)(response?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError), response?.Content);
        }

        // POST: api/business/search
        [HttpPost("search")]
        public async Task<ActionResult> SearchDataAsync([FromBody] SearchData searchData)
        {
            var client = new RestClient($"{dataApiBaseUrl}/search");
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddJsonBody(searchData);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var data = JsonConvert.DeserializeObject<DataIntermed>(response.Content);
                return Ok(data);
            }
            return StatusCode((int)(response?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError), response?.Content);
        }
               // GET: api/business/image
        [HttpGet("image")]
        public async Task<IActionResult> GetImageAsync()
        {
            try
            {
                // Use the correct path from the field
                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound("Image not found.");
                }

                byte[] imageData = await System.IO.File.ReadAllBytesAsync(imagePath);
                return File(imageData, "image/jpeg"); // Adjust MIME type as necessary based on your actual image
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Consider logging this instead of writing to console
                return StatusCode(500, "An error occurred while retrieving the image.");
            }
        }
    }

    public class SearchData
    {
        public string SearchStr { get; set; } = string.Empty;
    }
}
