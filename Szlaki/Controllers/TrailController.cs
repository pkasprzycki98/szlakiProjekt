using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Szlaki.Contracts.Request;
using Szlaki.Contracts.Response;
using Szlaki.DbContext;
using Szlaki.Models;
using Szlaki.Models.ViewModels;
using Szlaki.Services.Interfaces;

namespace Szlaki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailService _trailService;
        private readonly ApplicationDbContext _dbContext;

        public TrailController(ITrailService trailService, ApplicationDbContext dbContext)
        {
            _trailService = trailService;
            _dbContext = dbContext;
        }
        [HttpGet("trailId/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _trailService.Get(id);

          
            return Ok(response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _trailService.GetAll();

            if (response == null)
            {
                throw new Exception("Nie znaleziono wycieczek");
            }
            return Ok(response);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] TrailRequest model)
        {
            var response = await _trailService.Post(model);

            if (response.Success == false)
            {
                throw new Exception(response.Message);
            }
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TrailRequest model)
        {
            var response = await _trailService.Put(model);

            if(response.Success == false)
            {
                throw new Exception(response.Message);
            }
            return Ok(response);
        }
        [HttpDelete("delete/{trailId}")]
        public async Task<IActionResult> Delete(string trailId)
        {
            var response = await _trailService.Delete(trailId);
            if (response.Success == false)
            {
                throw new Exception("Nie usunięto klienta");
            }
            return Ok(response);
        }

        [HttpPost("photo")]
        public async Task<IActionResult> PostFile([FromForm] FileDto postPhoto)
        {

            var response = await _trailService.PostPhoto(postPhoto);
            return Ok(response);

        }
        [HttpPost("video")]
        public async Task<IActionResult> PostVideoFile([FromForm] FileDto postPhoto)
        {

            var response = await _trailService.PostVideo(postPhoto);
            return Ok(response);

        }

        [HttpGet("video")]
        public async Task<IActionResult> GetVideo(string id)
        {
            var entity = await _dbContext.Videos.FirstOrDefaultAsync(x => x.TrailId == id);
            if (entity != null)
            {
                var path = entity.Path;
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "video/mp4", Path.GetFileName(path));
            }
            return Ok(null);

        }
    }
}
