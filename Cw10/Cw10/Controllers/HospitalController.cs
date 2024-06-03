using Cw10.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DTOs;

namespace Cw10.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IDbService _dbService;
        public HospitalController(IDbService DbService)
        {
            _dbService = DbService;
        }
        [HttpGet("patient")]
        public async Task<IActionResult> GetPatient()
        {
            return await _dbService.GetPatient();
        }
        [HttpPost("prescription")]
        public async Task<IActionResult> AddPrescription(AddPrescription request)
        {
            return await _dbService.AddPrescription(request);
        }
    }
}