using Microsoft.AspNetCore.Mvc;
using Szlaki.Contracts.Request;
using Szlaki.Models;
using Szlaki.Models.ViewModels;
using Szlaki.Shared;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Szlaki.Services.Interfaces
{
    public interface ITrailService
    {
        public Task<Trail> Get(string trailId);
        public Task<List<Trail>> GetAll();
        public Task<ServiceResponse<trailResponse>> Post(TrailRequest companyViewModel);
        public Task<ServiceResponse<trailResponse>> Put(TrailRequest companyViewModel);
        public Task<ServiceResponse<string>> Delete(string trailId);
        public Task<ServiceResponse<trailResponse>> GettrailById(string trailId);
        public Task<bool> PostPhoto(FileDto fileDto);
        public Task<bool> PostVideo(FileDto fileDto);
    }
}
