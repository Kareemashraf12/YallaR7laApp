using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YallaR7la.Data;

namespace YallaR7la.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public ReportsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var result = await _appDbContext.AnalyticsReports.ToListAsync();
            return Ok(result);
        }
    }
}
