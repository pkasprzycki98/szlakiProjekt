using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Szlaki.Contracts.Request;
using Szlaki.DbContext;
using Szlaki.Models;
using Szlaki.Models.ViewModels;
using Szlaki.Services.Interfaces;
using Szlaki.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Szlaki.Services.User;

namespace Szlaki.Services.TrailService
{
    public class TrailService : ITrailService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;
        public TrailService(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IUserService userService)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }

        public async Task<ServiceResponse<string>> Delete(string trailId)
        {
            var response = new ServiceResponse<string>();
            var model = await _applicationDbContext.Trails.Include(x => x.Photos).Include(x=>x.Videos).FirstOrDefaultAsync(i => i.Id == trailId);
            if (model == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";

                return response;
            }
            _applicationDbContext.Remove(model);
            await _applicationDbContext.SaveChangesAsync();

            response.Success = true;
            response.Data = trailId;

            return response;
        }

        public async Task<Trail> Get(string trailId)
        {
            var response = new ServiceResponse<Trail>();
            var model = await _applicationDbContext.Trails.Include(x => x.Photos).Include(x => x.Videos).FirstOrDefaultAsync(i => i.Id == trailId);
            if (model == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";

                return model;
            }

            return model;
        }

        public async Task<ServiceResponse<trailResponse>> Post(TrailRequest trailViewModel)
        {
            var response = new ServiceResponse<trailResponse>();
            var model = trailViewModel.Adapt<Trail>();

            model.Id = Guid.NewGuid().ToString();
            ApplicationUser user = await _userService.GetCurrentUserAsync();
            model.UserId = user.Id;
            if (model == null)
            {
                response.Success = false;
                response.Message = "Nie dodano klienta!";

                return response;
            }

            await _applicationDbContext.Trails.AddAsync(model);
            await _applicationDbContext.SaveChangesAsync();

            response.Success = true;
            response.Data = model.Adapt<trailResponse>();
            return response;
        }

        public async Task<ServiceResponse<trailResponse>> Put(TrailRequest trailViewModel)
        {
            var response = new ServiceResponse<trailResponse>();
            var model = trailViewModel.Adapt<Trail>();
            model.UserId = await _userService.GetCurrentUserIdAsync();

            if (model == null)
            {
                response.Success = false;
                response.Message = "Nie dodano klienta!";

                return response;
            }
            _applicationDbContext.Entry(model).State = EntityState.Modified;

            await _applicationDbContext.SaveChangesAsync();

            response.Success = true;
            response.Data = model.Adapt<trailResponse>();
            return response;
        }

        public async Task<ServiceResponse<trailResponse>> GettrailById(string trailId)
        {
            var response = new ServiceResponse<trailResponse>();
            var model = await _applicationDbContext.Trails.FirstOrDefaultAsync(i => i.Id == trailId);
            if (model == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";

                return response;
            }

            response.Success = true;
            response.Data = model.Adapt<trailResponse>();

            return response;
        }

        public async Task<List<Trail>> GetAll()
        {
            var model = await _applicationDbContext.Trails.ToListAsync();
            if (model == null)
            {
                return model;
            }

            return model;
        }

        public async Task<bool> PostPhoto(FileDto fileDto)
        {
            var trail = await _applicationDbContext.Trails.FirstOrDefaultAsync(x => x.Id == fileDto.TrailId);
            if (trail == null)
            {
                return false;
            }

            var newEntity = new Photo
            {
                Id = Guid.NewGuid().ToString(),
                TrailId = trail.Id,
                PhotoArray = await GetFileAsByteArrayAsync(fileDto.TrailPhoto),
                Trail = trail
            };

            await _applicationDbContext.Photos.AddAsync(newEntity);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PostVideo(FileDto postVideo)
        {
            var trail = await _applicationDbContext.Trails.FirstOrDefaultAsync(x => x.Id == postVideo.TrailId);
            var pathName = Guid.NewGuid();
            var dir = _webHostEnvironment.ContentRootPath;
            var fullPath = Path.Combine(dir, pathName + ".mp4");

            using (var filestream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await postVideo.TrailPhoto.CopyToAsync(filestream);
            }

            var newEntity = new Video
            {
                Id = Guid.NewGuid().ToString(),
                TrailId = trail.Id,
                Trail = trail,
                Path = fullPath
            };

            await _applicationDbContext.Videos.AddAsync(newEntity);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        private async Task<byte[]> GetFileAsByteArrayAsync(IFormFile ProductPhoto)
        {
            if (ProductPhoto != null)
            {
                if (ProductPhoto.Length > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = ProductPhoto.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        await fs1.CopyToAsync(ms1);
                        p1 = ms1.ToArray();
                    }
                    return p1;
                }

            }
            return null;
        }
    }
}
