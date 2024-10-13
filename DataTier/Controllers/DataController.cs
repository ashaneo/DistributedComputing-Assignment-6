using Microsoft.AspNetCore.Mvc;
using DataTierAPI.Models;
using DataTierAPI.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataTierAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataController> _logger;

        public DataController(ApplicationDbContext context, ILogger<DataController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/data
        [HttpGet]
        public async Task<ActionResult<int>> GetDataCountAsync()
        {
            try
            {
                var count = await _context.DataRecords.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the data count.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/data/values
        [HttpGet("values")]
        public async Task<ActionResult<IEnumerable<DataIntermed>>> GetAllDataAsync()
        {
            try
            {
                var data = await _context.DataRecords.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all data.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/data/values/{id}
        [HttpGet("values/{id}")]
        public async Task<ActionResult<DataIntermed>> GetDataByIdAsync(int id)
        {
            try
            {
                var record = await _context.DataRecords.FindAsync(id);
                if (record == null)
                {
                    return NotFound($"No record found with ID {id}");
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving data for ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // POST: api/data/search
        [HttpPost("search")]
        public async Task<ActionResult<DataIntermed>> SearchDataAsync([FromBody] SearchData searchData)
        {
            try
            {
                var record = await _context.DataRecords
                    .FirstOrDefaultAsync(d => d.FName.Contains(searchData.SearchStr) || d.LName.Contains(searchData.SearchStr));

                if (record == null)
                {
                    return NotFound("No matching record found.");
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for data.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }

    public class SearchData
    {
        public string SearchStr { get; set; } = string.Empty;
    }
}
